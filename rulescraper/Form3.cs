using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rulescraper {
    public partial class Form3 : Form {
        public Form3(string dataLineSep, bool clearDataBeforeLaunch, bool hideBrowser) {
            InitializeComponent();
            dataSepComboBox.Items.Add(",");
            dataSepComboBox.Items.Add(";");
            dataSepComboBox.Items.Add("|");
            dataSepComboBox.SelectedIndex = dataSepComboBox.FindString(dataLineSep);
            clearDataBeforeLaunchCheckBox.Checked = clearDataBeforeLaunch;
            hideBrowserCheckBox.Checked = hideBrowser;
        }

        private void okButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
