
namespace CDM_Generator
{
    partial class frmManifest
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
            this.lblManifest = new System.Windows.Forms.Label();
            this.richDefaultManifest = new System.Windows.Forms.RichTextBox();
            this.lblRegex = new System.Windows.Forms.Label();
            this.txtRegex = new System.Windows.Forms.TextBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnJSON = new System.Windows.Forms.Button();
            this.lblEntityName = new System.Windows.Forms.Label();
            this.txtEntityName = new System.Windows.Forms.TextBox();
            this.btnSelectFiles = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblManifest
            // 
            this.lblManifest.AutoSize = true;
            this.lblManifest.Location = new System.Drawing.Point(13, 190);
            this.lblManifest.Name = "lblManifest";
            this.lblManifest.Size = new System.Drawing.Size(80, 25);
            this.lblManifest.TabIndex = 26;
            this.lblManifest.Text = "Manifest";
            // 
            // richDefaultManifest
            // 
            this.richDefaultManifest.Location = new System.Drawing.Point(138, 187);
            this.richDefaultManifest.Name = "richDefaultManifest";
            this.richDefaultManifest.Size = new System.Drawing.Size(512, 318);
            this.richDefaultManifest.TabIndex = 25;
            this.richDefaultManifest.Text = "";
            // 
            // lblRegex
            // 
            this.lblRegex.AutoSize = true;
            this.lblRegex.Location = new System.Drawing.Point(13, 150);
            this.lblRegex.Name = "lblRegex";
            this.lblRegex.Size = new System.Drawing.Size(119, 25);
            this.lblRegex.TabIndex = 24;
            this.lblRegex.Text = "Regex Pattern";
            // 
            // txtRegex
            // 
            this.txtRegex.Location = new System.Drawing.Point(139, 150);
            this.txtRegex.Name = "txtRegex";
            this.txtRegex.Size = new System.Drawing.Size(512, 31);
            this.txtRegex.TabIndex = 3;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(394, 551);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(125, 34);
            this.btnCopy.TabIndex = 28;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnJSON
            // 
            this.btnJSON.Location = new System.Drawing.Point(525, 551);
            this.btnJSON.Name = "btnJSON";
            this.btnJSON.Size = new System.Drawing.Size(125, 34);
            this.btnJSON.TabIndex = 27;
            this.btnJSON.Text = "Save";
            this.btnJSON.UseVisualStyleBackColor = true;
            this.btnJSON.Click += new System.EventHandler(this.btnJSON_Click);
            // 
            // lblEntityName
            // 
            this.lblEntityName.AutoSize = true;
            this.lblEntityName.Location = new System.Drawing.Point(13, 12);
            this.lblEntityName.Name = "lblEntityName";
            this.lblEntityName.Size = new System.Drawing.Size(126, 25);
            this.lblEntityName.TabIndex = 30;
            this.lblEntityName.Text = "Entity Name(s)";
            this.lblEntityName.Click += new System.EventHandler(this.lblEntityName_Click);
            // 
            // txtEntityName
            // 
            this.txtEntityName.Location = new System.Drawing.Point(139, 12);
            this.txtEntityName.Multiline = true;
            this.txtEntityName.Name = "txtEntityName";
            this.txtEntityName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEntityName.Size = new System.Drawing.Size(511, 91);
            this.txtEntityName.TabIndex = 29;
            this.txtEntityName.TextChanged += new System.EventHandler(this.txtEntityName_TextChanged);
            // 
            // btnSelectFiles
            // 
            this.btnSelectFiles.Location = new System.Drawing.Point(525, 110);
            this.btnSelectFiles.Name = "btnSelectFiles";
            this.btnSelectFiles.Size = new System.Drawing.Size(125, 34);
            this.btnSelectFiles.TabIndex = 1;
            this.btnSelectFiles.Text = "Select File(s)";
            this.btnSelectFiles.UseVisualStyleBackColor = true;
            this.btnSelectFiles.Click += new System.EventHandler(this.btnSelectFiles_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(525, 511);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(125, 34);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // frmManifest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 600);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnSelectFiles);
            this.Controls.Add(this.lblEntityName);
            this.Controls.Add(this.txtEntityName);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnJSON);
            this.Controls.Add(this.lblManifest);
            this.Controls.Add(this.richDefaultManifest);
            this.Controls.Add(this.lblRegex);
            this.Controls.Add(this.txtRegex);
            this.Name = "frmManifest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manifest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblManifest;
        private System.Windows.Forms.RichTextBox richDefaultManifest;
        private System.Windows.Forms.Label lblRegex;
        private System.Windows.Forms.TextBox txtRegex;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnJSON;
        private System.Windows.Forms.Label lblEntityName;
        private System.Windows.Forms.TextBox txtEntityName;
        private System.Windows.Forms.Button btnSelectFiles;
        private System.Windows.Forms.Button btnGenerate;
    }
}