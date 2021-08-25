using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;

namespace CDM_Generator
{
    public partial class frmManifest : Form
    {
        public frmManifest()
        {
            InitializeComponent();
        }

        private void btnSelectFiles_Click(object sender, EventArgs e)
        {
            SelectFiles();
        }
        private void SelectFiles()
        {
            StringBuilder sb = new StringBuilder();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|Parquet files (*.parquet)|*.parquet";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
              
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //get the file names selected
                    foreach(String fileName in openFileDialog.SafeFileNames)
                    {
                        sb.AppendLine(fileName);
                    }

                    txtEntityName.Text = sb.ToString();
                }
            }
        }

        private string GetFileName(string fullFileName)
        {
            //get only the file name for the initial entity name
            int index = fullFileName.LastIndexOf('.');
            string fileName = fullFileName.Substring(0, index).ToLower();
            return fileName;
        }

        private string GetFileExtension(string fullFileName)
        {
            //get only the file name for the initial entity name
            int index = fullFileName.LastIndexOf('.');
            string fileExtension = fullFileName.Substring(index + 1).ToLower();
            return fileExtension;
        }

        private string GenerateManifest()
        {
            StringBuilder entityManifest = new StringBuilder();
            string trait = string.Empty;
            string fileExtension = string.Empty;
            string fileName = string.Empty;

            entityManifest.Append("{'manifestName': 'default','entities': [");

            foreach (String entityName in txtEntityName.Lines)
            {
                if (entityName.Length > 0)
                { 
                    fileExtension = GetFileExtension(entityName);
                    fileName = GetFileName(entityName);

                    if (fileExtension == "parquet")
                        trait = "'is.partition.format.parquet'";

                    entityManifest.Append("{'type': 'LocalEntity','entityName': '"+ fileName + "','entityPath': 'resolved/" + fileName + ".cdm.json/" + fileName + "',");
                    entityManifest.Append("'dataPartitionPatterns': [{ 'name': '" + fileName + "Partitions','rootLocation': 'Replace with Root location','regularExpression': '"+txtRegex.Text.ToString()+"','parameters': [],'exhibitsTraits': ["+ trait + "]}]");
                    entityManifest.Append(",'definitions': [{'traitName': 'is.formatted','extendsTrait': 'is'},");
                    entityManifest.Append("{'traitName': 'is.formatted.dateTime','extendsTrait': 'is.formatted',");
                    entityManifest.Append("'hasParameters': [{'name': 'format','dataType': 'stringFormat','defaultValue': 'YYYY-MM-DDThh:mmZ'}]},");
                    entityManifest.Append("{'traitName': 'is.formatted.date','extendsTrait': 'is.formatted',");
                    entityManifest.Append("'hasParameters': [{'name': 'format','dataType': 'stringFormat','defaultValue': 'YYYY-MM-DD'}]},");
                    entityManifest.Append("{'traitName': 'is.formatted.time','extendsTrait': 'is.formatted',");
                    entityManifest.Append("'hasParameters': [{'name': 'format','dataType': 'stringFormat','defaultValue': 'hh:mm:ss'}]}]},");
                }
            }
            //remove the trailing comma
            entityManifest.Remove(entityManifest.Length - 1, 1);
            entityManifest.Append("],'jsonSchemaSemanticVersion': '1.0.0'}");

            entityManifest.Replace("'", "\"");

            //format the JSON
            try
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };

                var jsonElement = JsonSerializer.Deserialize<JsonElement>(entityManifest.ToString());

                return JsonSerializer.Serialize(jsonElement, options);
            }
            catch
            {
                return (entityManifest.ToString());
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //verify
           if (txtEntityName.TextLength == 0)
                MessageBox.Show("Please select one or more files;");

           richDefaultManifest.Text = GenerateManifest();
        }

        private void btnJSON_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = "default.manifest.cdm.json";
                string filePath = Directory.GetCurrentDirectory().ToString();
                System.IO.File.WriteAllText(fileName, richDefaultManifest.Text);

                DialogResult results = MessageBox.Show("The file " + fileName + " saved successfully to " + filePath, "Save dialog");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an exception saving the file:" + ex.Message, "Exception");
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richDefaultManifest.Text);
            MessageBox.Show("Manifest Copied to Clipboard!");
        }
    }
}
