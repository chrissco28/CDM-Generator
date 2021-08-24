using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

namespace CDM_Generator
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            //update the settings file
            ConfigurationManager.AppSettings.Set("AttributeNames",chkAlphaColumns.Checked.ToString());
            ConfigurationManager.AppSettings.Set("PreviewData", chkEnableDataPreview.Checked.ToString());
            ConfigurationManager.AppSettings.Set("DataTypes", chkDataTypes.Checked.ToString());
            ConfigurationManager.AppSettings.Set("GenerateManifest", chkManifest.Checked.ToString());

            MessageBox.Show("Settings updated.", "CDM Generator Savings");

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //close the form but don't save any settings
            this.Close();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            chkAlphaColumns.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("AttributeNames").ToString());
            chkEnableDataPreview.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("PreviewData").ToString());
            chkDataTypes.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("DataTypes").ToString());
            chkManifest.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("GenerateManifest").ToString());
        }
    }
}
