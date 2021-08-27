
namespace CDM_Generator
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnJSON = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCDMJson = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.lblEntityName = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gridEntities = new System.Windows.Forms.DataGridView();
            this.lblManifest = new System.Windows.Forms.Label();
            this.richDefaultManifest = new System.Windows.Forms.RichTextBox();
            this.btnPreviewData = new System.Windows.Forms.Button();
            this.btnCDM = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEntities)).BeginInit();
            this.SuspendLayout();
            // 
            // btnJSON
            // 
            this.btnJSON.Location = new System.Drawing.Point(815, 813);
            this.btnJSON.Name = "btnJSON";
            this.btnJSON.Size = new System.Drawing.Size(125, 34);
            this.btnJSON.TabIndex = 0;
            this.btnJSON.Text = "Save";
            this.btnJSON.UseVisualStyleBackColor = true;
            this.btnJSON.Click += new System.EventHandler(this.btnJSON_Click);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(815, 208);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(125, 34);
            this.btnSelectFile.TabIndex = 3;
            this.btnSelectFile.Text = "Select File(s)";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 260);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Data Preview";
            // 
            // txtCDMJson
            // 
            this.txtCDMJson.Location = new System.Drawing.Point(134, 665);
            this.txtCDMJson.Name = "txtCDMJson";
            this.txtCDMJson.Size = new System.Drawing.Size(806, 133);
            this.txtCDMJson.TabIndex = 6;
            this.txtCDMJson.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 665);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "Entity JSON";
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(135, 260);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersWidth = 62;
            this.dataGridView.RowTemplate.Height = 33;
            this.dataGridView.Size = new System.Drawing.Size(805, 169);
            this.dataGridView.TabIndex = 8;
            // 
            // lblEntityName
            // 
            this.lblEntityName.AutoSize = true;
            this.lblEntityName.Location = new System.Drawing.Point(12, 36);
            this.lblEntityName.Name = "lblEntityName";
            this.lblEntityName.Size = new System.Drawing.Size(56, 25);
            this.lblEntityName.TabIndex = 11;
            this.lblEntityName.Text = "Entity";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(952, 33);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem1});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(92, 29);
            this.optionsToolStripMenuItem.Text = "Settings";
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(178, 34);
            this.optionsToolStripMenuItem1.Text = "Options";
            this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click);
            // 
            // gridEntities
            // 
            this.gridEntities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridEntities.Location = new System.Drawing.Point(135, 36);
            this.gridEntities.MultiSelect = false;
            this.gridEntities.Name = "gridEntities";
            this.gridEntities.RowHeadersWidth = 62;
            this.gridEntities.RowTemplate.Height = 33;
            this.gridEntities.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridEntities.Size = new System.Drawing.Size(805, 166);
            this.gridEntities.TabIndex = 16;
            // 
            // lblManifest
            // 
            this.lblManifest.AutoSize = true;
            this.lblManifest.Location = new System.Drawing.Point(12, 490);
            this.lblManifest.Name = "lblManifest";
            this.lblManifest.Size = new System.Drawing.Size(80, 25);
            this.lblManifest.TabIndex = 28;
            this.lblManifest.Text = "Manifest";
            // 
            // richDefaultManifest
            // 
            this.richDefaultManifest.Location = new System.Drawing.Point(135, 478);
            this.richDefaultManifest.Name = "richDefaultManifest";
            this.richDefaultManifest.Size = new System.Drawing.Size(805, 169);
            this.richDefaultManifest.TabIndex = 27;
            this.richDefaultManifest.Text = "";
            // 
            // btnPreviewData
            // 
            this.btnPreviewData.Location = new System.Drawing.Point(815, 435);
            this.btnPreviewData.Name = "btnPreviewData";
            this.btnPreviewData.Size = new System.Drawing.Size(125, 34);
            this.btnPreviewData.TabIndex = 29;
            this.btnPreviewData.Text = "Preview Data";
            this.btnPreviewData.UseVisualStyleBackColor = true;
            this.btnPreviewData.Click += new System.EventHandler(this.btnPreviewData_Click);
            // 
            // btnCDM
            // 
            this.btnCDM.Location = new System.Drawing.Point(662, 813);
            this.btnCDM.Name = "btnCDM";
            this.btnCDM.Size = new System.Drawing.Size(136, 34);
            this.btnCDM.TabIndex = 30;
            this.btnCDM.Text = "Generate CDM";
            this.btnCDM.UseVisualStyleBackColor = true;
            this.btnCDM.Click += new System.EventHandler(this.btnCDM_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 859);
            this.Controls.Add(this.btnCDM);
            this.Controls.Add(this.btnPreviewData);
            this.Controls.Add(this.lblManifest);
            this.Controls.Add(this.richDefaultManifest);
            this.Controls.Add(this.gridEntities);
            this.Controls.Add(this.lblEntityName);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCDMJson);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.btnJSON);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CDM Generator";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEntities)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnJSON;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtCDMJson;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label lblEntityName;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
        private System.Windows.Forms.DataGridView gridEntities;
        private System.Windows.Forms.Label lblManifest;
        private System.Windows.Forms.RichTextBox richDefaultManifest;
        private System.Windows.Forms.Button btnPreviewData;
        private System.Windows.Forms.Button btnCDM;
    }
}

