﻿
namespace CDM_Generator
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkAlphaColumns = new System.Windows.Forms.CheckBox();
            this.chkManifest = new System.Windows.Forms.CheckBox();
            this.chkEnableDataPreview = new System.Windows.Forms.CheckBox();
            this.chkDataTypes = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkAlphaColumns
            // 
            this.chkAlphaColumns.AutoSize = true;
            this.chkAlphaColumns.Checked = true;
            this.chkAlphaColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlphaColumns.Location = new System.Drawing.Point(41, 29);
            this.chkAlphaColumns.Name = "chkAlphaColumns";
            this.chkAlphaColumns.Size = new System.Drawing.Size(230, 29);
            this.chkAlphaColumns.TabIndex = 14;
            this.chkAlphaColumns.Text = "Alphanumeric Attributes";
            this.chkAlphaColumns.UseVisualStyleBackColor = true;
            // 
            // chkManifest
            // 
            this.chkManifest.AutoSize = true;
            this.chkManifest.Checked = true;
            this.chkManifest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkManifest.Location = new System.Drawing.Point(41, 78);
            this.chkManifest.Name = "chkManifest";
            this.chkManifest.Size = new System.Drawing.Size(181, 29);
            this.chkManifest.TabIndex = 15;
            this.chkManifest.Text = "Generate Manifest";
            this.chkManifest.UseVisualStyleBackColor = true;
            // 
            // chkEnableDataPreview
            // 
            this.chkEnableDataPreview.AutoSize = true;
            this.chkEnableDataPreview.Checked = true;
            this.chkEnableDataPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableDataPreview.Location = new System.Drawing.Point(326, 78);
            this.chkEnableDataPreview.Name = "chkEnableDataPreview";
            this.chkEnableDataPreview.Size = new System.Drawing.Size(140, 29);
            this.chkEnableDataPreview.TabIndex = 16;
            this.chkEnableDataPreview.Text = "Preview Data";
            this.chkEnableDataPreview.UseVisualStyleBackColor = true;
            // 
            // chkDataTypes
            // 
            this.chkDataTypes.AutoSize = true;
            this.chkDataTypes.Checked = true;
            this.chkDataTypes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDataTypes.Location = new System.Drawing.Point(326, 29);
            this.chkDataTypes.Name = "chkDataTypes";
            this.chkDataTypes.Size = new System.Drawing.Size(252, 29);
            this.chkDataTypes.TabIndex = 17;
            this.chkDataTypes.Text = "Infer Data Types (CSV only)";
            this.chkDataTypes.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(546, 141);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 34);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(412, 141);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(112, 34);
            this.btnOkay.TabIndex = 19;
            this.btnOkay.Text = "Save";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 187);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkDataTypes);
            this.Controls.Add(this.chkEnableDataPreview);
            this.Controls.Add(this.chkManifest);
            this.Controls.Add(this.chkAlphaColumns);
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAlphaColumns;
        private System.Windows.Forms.CheckBox chkManifest;
        private System.Windows.Forms.CheckBox chkEnableDataPreview;
        private System.Windows.Forms.CheckBox chkDataTypes;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOkay;
    }
}