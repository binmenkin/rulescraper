namespace rulescraper {
    partial class Form3 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.dataSepLabel = new System.Windows.Forms.Label();
            this.dataSepComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.clearDataBeforeLaunchCheckBox = new System.Windows.Forms.CheckBox();
            this.hideBrowserCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // dataSepLabel
            // 
            this.dataSepLabel.AutoSize = true;
            this.dataSepLabel.Location = new System.Drawing.Point(12, 15);
            this.dataSepLabel.Name = "dataSepLabel";
            this.dataSepLabel.Size = new System.Drawing.Size(105, 17);
            this.dataSepLabel.TabIndex = 0;
            this.dataSepLabel.Text = "Data Separator";
            // 
            // dataSepComboBox
            // 
            this.dataSepComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataSepComboBox.Location = new System.Drawing.Point(123, 12);
            this.dataSepComboBox.Name = "dataSepComboBox";
            this.dataSepComboBox.Size = new System.Drawing.Size(40, 24);
            this.dataSepComboBox.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(15, 96);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 27);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(96, 96);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 27);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // clearDataBeforeLaunchCheckBox
            // 
            this.clearDataBeforeLaunchCheckBox.AutoSize = true;
            this.clearDataBeforeLaunchCheckBox.Location = new System.Drawing.Point(15, 42);
            this.clearDataBeforeLaunchCheckBox.Name = "clearDataBeforeLaunchCheckBox";
            this.clearDataBeforeLaunchCheckBox.Size = new System.Drawing.Size(194, 21);
            this.clearDataBeforeLaunchCheckBox.TabIndex = 4;
            this.clearDataBeforeLaunchCheckBox.Text = "Clear Data Before Launch";
            this.clearDataBeforeLaunchCheckBox.UseVisualStyleBackColor = true;
            // 
            // hideBrowserCheckBox
            // 
            this.hideBrowserCheckBox.AutoSize = true;
            this.hideBrowserCheckBox.Location = new System.Drawing.Point(15, 69);
            this.hideBrowserCheckBox.Name = "hideBrowserCheckBox";
            this.hideBrowserCheckBox.Size = new System.Drawing.Size(138, 21);
            this.hideBrowserCheckBox.TabIndex = 5;
            this.hideBrowserCheckBox.Text = "Hide the Browser";
            this.hideBrowserCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 135);
            this.Controls.Add(this.hideBrowserCheckBox);
            this.Controls.Add(this.clearDataBeforeLaunchCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.dataSepComboBox);
            this.Controls.Add(this.dataSepLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(239, 182);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(239, 182);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label dataSepLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.ComboBox dataSepComboBox;
        public System.Windows.Forms.CheckBox clearDataBeforeLaunchCheckBox;
        public System.Windows.Forms.CheckBox hideBrowserCheckBox;
    }
}