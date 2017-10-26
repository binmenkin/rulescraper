using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using System.Diagnostics;
using System.Globalization;

namespace rulescraper {
    public partial class Form1 : Form {
        private string urlPlaceholderText = "site.com";
        private List<string> rules = new List<string>();
        private string ruleLineSep = "[:]";
        private ChromeDriver driver;

        private char dataLineSep = ',';
        private bool clearDataBeforeLaunch = true;
        private bool hideBrowser = false;

        public Form1() {
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(OnProcessExit);
            LoadSettings();
        }

        private void AddRule(string ruleName, string rule) {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = "Column" + dataGridView.Columns.Count;
            col.HeaderText = ruleName;
            col.SortMode = DataGridViewColumnSortMode.Programmatic;

            dataGridView.Columns.Add(col);
            rules.Add(rule);
        }

        private void RemoveRules() {
            dataGridView.ClearSelection();
            dataGridView.Columns.Clear();
            rules.Clear();
        }

        private bool PageElementExist(By by) {
            try {
                driver.FindElement(by);
                return true;
            } catch {
                return false;
            }
        }

        private List<string> FindDataByPattern(string page, string pattern) {
            List<string> data = new List<string>();
            int srcIndex = 0;

            while (srcIndex < page.Length && page.IndexOf(pattern.Substring(0, pattern.IndexOfAny(new char[] { '?', '*', '#' })), srcIndex) > -1) {
                int patIndex = 0;

                while (patIndex < pattern.Length) {
                    if (backgroundWorker.CancellationPending == true)
                        return null;

                    int curPatIndex = pattern.IndexOfAny(new char[] { '?', '*', '#' }, patIndex);
                    int curSrcIndex = page.IndexOf(pattern.Substring(patIndex, curPatIndex - patIndex), srcIndex);

                    if (curSrcIndex > -1) {
                        srcIndex = curSrcIndex + curPatIndex - patIndex;
                        patIndex = curPatIndex;

                        switch (pattern[curPatIndex]) {
                            case '?':
                                if (page[srcIndex + 1] == pattern[patIndex + 1]) {
                                    patIndex += 1;
                                } else {
                                    patIndex = pattern.Length;
                                }

                                srcIndex += 1;
                                break;
                            case '*':
                                curSrcIndex = page.IndexOf(pattern[curPatIndex + 1], srcIndex);

                                if (curSrcIndex > -1) {
                                    srcIndex = curSrcIndex + 1;
                                    patIndex += 2;
                                } else {
                                    patIndex = pattern.Length;
                                }

                                break;
                            case '#':
                                curSrcIndex = page.IndexOf(pattern[curPatIndex + 1], srcIndex);

                                if (curSrcIndex > -1) {
                                    if (curSrcIndex + pattern.Length - (curPatIndex + 1) < page.Length) {
                                        if (page.Substring(curSrcIndex, pattern.Length - (curPatIndex + 1)) == pattern.Substring(curPatIndex + 1)) {
                                            string res = page.Substring(srcIndex, curSrcIndex - srcIndex);
                                            data.Add(res);
                                        }
                                    }
                                }

                                patIndex = pattern.Length;
                                break;
                        }
                    } else {
                        patIndex = pattern.Length;
                    }
                }
            }

            return data;
        }

        private void GatherPageData(string page) {
            int startRow = dataGridView.RowCount;

            for (int i = 0; i < rules.Count; i++) {
                int curRow = startRow;

                foreach (string res in FindDataByPattern(page, rules[i])) {
                    string buf = curRow + "{,}" + i + "{,}" + res.Trim();
                    backgroundWorker.ReportProgress(0, buf);
                    curRow++;
                }
            }
        }

        private void SaveSettings() {
            string fileData = "";
            fileData += "Data Separator=" + dataLineSep + Environment.NewLine;
            fileData += "Clear Data Before Launch=" + clearDataBeforeLaunch + Environment.NewLine;
            fileData += "Hide the Browser=" + hideBrowser + Environment.NewLine;

            File.WriteAllText("settings.ini", fileData);
        }

        private void LoadSettings() {
            if (File.Exists("settings.ini")) {
                foreach (string line in File.ReadLines("settings.ini")) {
                    switch (line.Split('=')[0]) {
                        case "Data Separator":
                            dataLineSep = line.Split('=')[1][0];
                            break;
                        case "Clear Data Before Launch":
                            clearDataBeforeLaunch = (line.Split('=')[1] == "False") ? false : true;
                            break;
                        case "Hide the Browser":
                            hideBrowser = (line.Split('=')[1] == "False") ? false : true;
                            break;
                    }
                }
            }
            SaveSettings();
        }

        private void importRulesToolStripMenuItem_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK) {
                RemoveRules();

                foreach (string line in File.ReadLines(dialog.FileName)) {
                    if (line.Contains(ruleLineSep)) {
                        AddRule(line.Substring(0, line.IndexOf(ruleLineSep)), line.Substring(line.IndexOf(ruleLineSep) + ruleLineSep.Length));
                    } else {
                        urlTextBox.Text = urlPlaceholderText;
                        urlTextBox_Enter(null, null);
                        urlTextBox.Text = line;
                        urlTextBox.Focus();
                        urlTextBox.Select(urlTextBox.Text.Length, 0);
                    }
                }
            }
        }

        private void exportRulesToolStripMenuItem_Click(object sender, EventArgs e) {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK) {
                string fileData = urlTextBox.Text + Environment.NewLine;

                for (int i = 0; i < rules.Count; i++) {
                    fileData += dataGridView.Columns[i].HeaderText + ruleLineSep;
                    fileData += rules[i] + Environment.NewLine;
                }

                File.WriteAllText(dialog.FileName, fileData);
            }
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e) {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK) {
                dataGridView.SelectAll();
                string fileData = "\uFEFF";

                if (dataGridView.RowCount != 0) {
                    fileData += dataGridView.GetClipboardContent().GetText();
                }

                fileData = fileData.Replace('\t', dataLineSep);

                File.WriteAllText(dialog.FileName, fileData);
                dataGridView.ClearSelection();
            }
        }

        private void clearDataToolStripMenuItem_Click(object sender, EventArgs e) {
            dataGridView.ClearSelection();
            dataGridView.Rows.Clear();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e) {
            urlTextBox.Text = urlPlaceholderText;
            urlTextBox.Focus();
            urlTextBox_Enter(null, null);
            currentPageCheckBox.Checked = false;
            RemoveRules();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
            Form3 dialog = new Form3("" + dataLineSep, clearDataBeforeLaunch, hideBrowser);
            var dialogResult = dialog.ShowDialog();

            if (dialogResult == DialogResult.OK) {
                dataLineSep = dialog.dataSepComboBox.GetItemText(dialog.dataSepComboBox.SelectedItem)[0];
                clearDataBeforeLaunch = dialog.clearDataBeforeLaunchCheckBox.Checked;
                hideBrowser = dialog.hideBrowserCheckBox.Checked;

                SaveSettings();
            }
        }

        private void SetUIState(bool isActive) {
            urlTextBox.Enabled = !currentPageCheckBox.Checked;
            addRuleButton.Enabled = isActive;
            editRuleButton.Enabled = isActive;
            removeRuleButton.Enabled = isActive;
            currentPageCheckBox.Enabled = isActive;
        }

        private void urlTextBox_Enter(object sender, EventArgs e) {
            if (urlTextBox.Text == urlPlaceholderText) {
                urlTextBox.Text = "";
                urlTextBox.ForeColor = Color.Black;
            }
        }

        private void urlTextBox_Leave(object sender, EventArgs e) {
            if (urlTextBox.Text == "") {
                urlTextBox.Text = urlPlaceholderText;
                urlTextBox.ForeColor = Color.Gray;
            }
        }

        private void scrapeButton_Click(object sender, EventArgs e) {
            if (!backgroundWorker.IsBusy) {
                if (clearDataBeforeLaunch) {
                    dataGridView.Rows.Clear();
                    dataGridView.Refresh();
                }

                scrapeButton.Text = "Stop";
                SetUIState(false);
                backgroundWorker.RunWorkerAsync();
            } else {
                backgroundWorker.CancelAsync();
                SetUIState(true);
                scrapeButton.Text = "Scrape";
            }
        }

        private void addRuleButton_Click(object sender, EventArgs e) {
            Form2 dialog = new Form2(true);
            dialog.Text = "Add Rule";
            var dialogResult = dialog.ShowDialog();

            if (dialogResult == DialogResult.OK) {
                AddRule(dialog.ruleNameTextBox.Text, dialog.ruleTextBox.Text);
            }
        }

        private void editRuleButton_Click(object sender, EventArgs e) {
            if (dataGridView.SelectedColumns.Count != 0) {
                Form2 dialog = new Form2(false);
                dialog.Text = "Edit Rule";
                dialog.ruleNameTextBox.Text = dataGridView.SelectedColumns[0].HeaderText;
                dialog.ruleTextBox.Text = rules[dataGridView.SelectedColumns[0].Index];
                var dialogResult = dialog.ShowDialog();

                if (dialogResult == DialogResult.OK) {
                    dataGridView.SelectedColumns[0].HeaderText = dialog.ruleNameTextBox.Text;
                    rules[dataGridView.SelectedColumns[0].Index] = dialog.ruleTextBox.Text;
                }
            }
        }

        private void removeRuleButton_Click(object sender, EventArgs e) {
            for (int i = dataGridView.Columns.Count - 1; i >= 0; i--) {
                if (dataGridView.Columns[i].Selected) {
                    dataGridView.Columns.RemoveAt(i);
                    rules.RemoveAt(i);
                }
            }
        }

        private void currentPageCheckBox_CheckedChanged(object sender, EventArgs e) {
            urlTextBox.Enabled = !currentPageCheckBox.Checked;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            try {
                driver.PageSource.ToString();
            } catch {
                if (driver != null)
                    driver.Quit();

                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                var driverOptions = new ChromeOptions();
                if (hideBrowser)
                    driverOptions.AddArgument("headless");
                driver = new ChromeDriver(driverService, driverOptions);
            }

            try {
                string url = urlTextBox.Text;

                if (currentPageCheckBox.Checked) {
                    GatherPageData(driver.PageSource);
                    return;
                }

                string iter = "";
                int startPage = -1;
                int maxPage = 0;
                int inc = 1;

                if (url.Contains("{iter")) {
                    iter = url.Substring(url.IndexOf("{iter") + 1, url.IndexOf('}', url.IndexOf("{iter")) - url.IndexOf("{iter") - 1);
                    string[] iterVals = iter.Split(':');

                    if (iterVals.Length > 1) {
                        startPage = int.Parse(iterVals[1]);

                        if (iterVals.Length > 2) {
                            maxPage = int.Parse(iterVals[2]);

                            if (iterVals.Length > 3) {
                                inc = int.Parse(iterVals[3]);
                            }
                        } else {
                            maxPage = 100;
                        }
                    }
                }

                for (int i = startPage; i < maxPage; i += inc) {
                    if (backgroundWorker.CancellationPending == true)
                        return;

                    if (iter != "")
                        url = urlTextBox.Text.Replace('{' + iter + '}', i.ToString());

                    string croppedPageUrl = (url.Contains(" ")) ? url.Substring(0, url.IndexOf(" ")) : url;

                    driver.Navigate().GoToUrl((croppedPageUrl.Contains("http")) ? croppedPageUrl : "http://" + croppedPageUrl);

                    string clickSelector = "";
                    bool parseClickType = true;
                    int clickDelay = 2;
                    int maxClicks = -1;

                    if (url.Contains("{click")) {
                        parseClickType = url.Contains("{click-expand") ? false : true;
                        string click = url.Substring(url.IndexOf("{click") + 1, url.IndexOf('}', url.IndexOf("{click")) - url.IndexOf("{click") - 1);

                        if (click.Contains(':')) {
                            clickSelector = click.Substring(click.IndexOf(':') + 1);

                            if (clickSelector.Contains(":")) {
                                if (!int.TryParse(clickSelector.Substring(0, clickSelector.IndexOf(':')), out clickDelay)) {
                                    clickDelay = 2;
                                } else {
                                    clickSelector = clickSelector.Substring(clickSelector.IndexOf(':') + 1);

                                    if (clickSelector.Contains(":")) {
                                        if (!int.TryParse(clickSelector.Substring(0, clickSelector.IndexOf(':')), out maxClicks)) {
                                            maxClicks = -1;
                                        } else {
                                            clickSelector = clickSelector.Substring(clickSelector.IndexOf(':') + 1);
                                        }
                                    }
                                }
                            }

                            if (!parseClickType) {
                                for (int j = 0; (j < maxClicks || maxClicks == -1) && PageElementExist(By.CssSelector(clickSelector)); j++) {
                                    if (backgroundWorker.CancellationPending == true)
                                        return;

                                    driver.FindElementByCssSelector(clickSelector).Click();
                                    Thread.Sleep(clickDelay * 1000);
                                }
                            }
                        }
                    }

                    string itemPrefix = "";
                    string itemRule = "";

                    if (url.Contains("{item")) {
                        itemRule = url.Substring(url.IndexOf("{item") + 1, url.IndexOf('}', url.IndexOf("{item")) - url.IndexOf("{item") - 1);

                        if (itemRule.Contains(':')) {
                            string[] itemVals = itemRule.Split(':');

                            if (itemVals.Length > 2) {
                                itemPrefix = itemVals[1];
                                itemRule = itemVals[2];
                            } else {
                                itemRule = itemVals[1];
                            }
                        }
                    }

                    string prevUrl = "";

                    for (int j = 0; j <= maxClicks || maxClicks == -1; j++) {
                        if (backgroundWorker.CancellationPending == true)
                            return;

                        if (itemRule != "") {
                            prevUrl = driver.Url;

                            foreach (string item in FindDataByPattern(driver.PageSource, itemRule)) {
                                if (backgroundWorker.CancellationPending == true)
                                    return;

                                string itemUrl = item.Contains("http") ? item : itemPrefix + item;
                                driver.Navigate().GoToUrl((itemUrl.Contains("http")) ? itemUrl : "http://" + itemUrl);
                                GatherPageData(driver.PageSource);
                            }

                            driver.Navigate().GoToUrl(prevUrl);
                        } else {
                            GatherPageData(driver.PageSource);
                        }

                        if (parseClickType && (j < maxClicks || maxClicks == -1) && (PageElementExist(By.CssSelector(clickSelector)))) {
                            driver.FindElementByCssSelector(clickSelector).Click();
                            Thread.Sleep(clickDelay * 1000);
                        } else {
                            maxClicks = 0;
                        }
                    }
                }
            } catch {
                
            }
        }

        private DateTime lastGridUpdateTime = DateTime.Now;
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            string[] vals = (e.UserState as String).Split(new string[] { "{,}" }, StringSplitOptions.None);
            int row = int.Parse(vals[0]);
            int col = int.Parse(vals[1]);
            string res = vals[2];

            if (row >= dataGridView.Rows.Count || !String.IsNullOrEmpty(dataGridView.Rows[row].Cells[col].Value as String)) {
                dataGridView.Rows.Add();
            }

            dataGridView.Rows[row].Cells[col].Value = res;

            if ((DateTime.Now - lastGridUpdateTime).TotalMilliseconds > 100) {
                dataGridView.Refresh();
                lastGridUpdateTime = DateTime.Now;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            SetUIState(true);
            scrapeButton.Text = "Scrape";
        }

        void KillProcessTree(Process process) {
            using (var procKiller = new Process()) {
                procKiller.StartInfo.FileName = "taskkill";
                procKiller.StartInfo.Arguments = string.Format("/PID {0} /T /F", process.Id);
                procKiller.StartInfo.CreateNoWindow = true;
                procKiller.StartInfo.UseShellExecute = false;
                procKiller.Start();
                procKiller.WaitForExit(1000);
            }
        }

        private void OnProcessExit(object sender, EventArgs e) {
            backgroundWorker.CancelAsync();
            KillProcessTree(Process.GetCurrentProcess());
        }
    }
}