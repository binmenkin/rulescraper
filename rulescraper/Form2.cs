using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rulescraper
{
    public partial class Form2 : Form
    {
        private string ruleNamePlaceholderText = "Rule Name";
        private string rulePlaceholderText = "Rule, e.g. <div *>#<";

        private bool CheckRule() {
            string rule = ruleTextBox.Text;
            bool flag = false;

            if (rule.Contains('#')) {
                if (rule.IndexOf('#') != rule.Length - 1 && rule.IndexOf('#') != 0) {
                    flag = true;
                } else {
                    flag = false;
                }
            } else {
                flag = false;
            }

            if (!flag)
                ruleTextBox.ForeColor = Color.Red;

            return flag;
        }

        public Form2(bool useRulePlaceholder)
        {
            InitializeComponent();
            if (useRulePlaceholder) {
                ruleTextBox_Leave(null, null);
            }
            cancelButton.Focus();
        }

        private void ruleNameTextBox_Enter(object sender, EventArgs e)
        {
            if (ruleNameTextBox.Text == ruleNamePlaceholderText) {
                ruleNameTextBox.Text = "";
                ruleNameTextBox.ForeColor = Color.Black;
            }
        }

        private void ruleNameTextBox_Leave(object sender, EventArgs e)
        {
            if (ruleNameTextBox.Text == "") {
                ruleNameTextBox.Text = ruleNamePlaceholderText;
                ruleNameTextBox.ForeColor = Color.Gray;
            }
        }

        private void ruleTextBox_Enter(object sender, EventArgs e)
        {
            if (ruleTextBox.Text == rulePlaceholderText) {
                ruleTextBox.Text = "";
                ruleTextBox.ForeColor = Color.Black;
            } else {
                ruleTextBox.ForeColor = Color.Black;
            }
        }

        private void ruleTextBox_Leave(object sender, EventArgs e)
        {
            if (ruleTextBox.Text == "") {
                ruleTextBox.Text = rulePlaceholderText;
                ruleTextBox.ForeColor = Color.Gray;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (ruleNameTextBox.Text != ruleNamePlaceholderText && ruleTextBox.Text != rulePlaceholderText && CheckRule()) {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
