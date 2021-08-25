using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Text.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;
using Parquet.Data;
using Parquet;
using System.Collections;
using System.Collections.Specialized;

using DataColumn = System.Data.DataColumn;

namespace CDM_Generator
{
    public partial class Main : Form
    {
        string fileName = string.Empty;
        string fileExtension = string.Empty;
        DataTable fileStructure = new DataTable();

        public Main()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            GetFile();
        }

        private void GetFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|Parquet files (*.parquet)|*.parquet";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    txtFilePath.Text = openFileDialog.FileName;

                    //get only the file name for the initial entity name
                    int index = openFileDialog.SafeFileName.LastIndexOf('.');
                    int length = openFileDialog.SafeFileName.Length;

                    fileName = openFileDialog.SafeFileName.Substring(0, index).ToLower();
                    fileExtension = openFileDialog.SafeFileName.Substring(index+1).ToLower();
                    txtEntityName.Text = fileName;
                }
            }
        }

        private void LoadCSVOnDataGridView(string fileName, bool loadData, bool tryDataTypes)
        {
            try
            {
                ReadCSV csv = new ReadCSV(fileName);
                int maxRowCounter = 50;
                int rowCounter = 0;
                string dataType = "string";
                DateTime DTresult;
                int Intresult;
                Decimal Decresult;
                bool failedParse = false;

                if (dataGridView.Columns.Count > 0)
                {
                    ClearTables();
                }

                //get the first row of the results to get the file structure
                for (int i = 0; i < csv.readCSV.Columns.Count; i++)
                {
                    System.Data.DataColumn col = new System.Data.DataColumn(csv.readCSV.Columns[i].ColumnName, typeof(string));
                    fileStructure.Columns.Add(col);
                }
                //add a second row and populate with data type
                DataRow dataRow = fileStructure.NewRow();

                //if the try data types flag is set to true
                //go through some of the values and see if there is a the data is date, int, or decimal

                foreach(DataColumn col in csv.readCSV.Columns)
                {
                    if (tryDataTypes)
                    {
                        if (csv.readCSV.Rows.Count < maxRowCounter)
                            maxRowCounter = csv.readCSV.Rows.Count;

                        //loop through a few of the values in the column to see what the data type
                        //could be

                       foreach(DataRow row in csv.readCSV.Rows)
                       {
                            if (rowCounter == maxRowCounter)
                            {
                                break;
                            }
                            //get the value to check
                            if (row[col] != null)
                            {
                                //check for the dates
                                if (Int32.TryParse(row[col].ToString(), out Intresult) && failedParse == false)
                                {
                                    dataType = "int32";
                                }
                                else if (Decimal.TryParse(row[col].ToString(), out Decresult) && failedParse == false)
                                {
                                    dataType = "decimal";
                                }
                                else if (DateTime.TryParse(row[col].ToString(), out DTresult) && failedParse == false)
                                {
                                    dataType = "dateTime";
                                }
                                else
                                {
                                    failedParse = true;
                                    dataType = "string";
                                }
                            }
                            rowCounter++;
                            
                        }
                        dataRow[col.Ordinal] = dataType;
                        
                        //reset
                        failedParse = false;
                        dataType = "string";
                    }
                    else
                        dataRow[col.Ordinal] = dataType; 
                }
                
                fileStructure.Rows.Add(dataRow);

                try
                {
                    if (loadData)
                        dataGridView.DataSource = csv.readCSV;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LoadParquetOnDataGridView(string fileName, bool loadData)
        {
            try
            {
                if (loadData)
                    dataGridView.DataSource = ReadParquet(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private class ReadCSV
        {
            
            public DataTable readCSV;

            public ReadCSV(string fileName, bool firstRowContainsFieldNames = true)
            {
                readCSV = GenerateDataTable(fileName, firstRowContainsFieldNames);
            }

            private static DataTable GenerateDataTable(string fileName, bool firstRowContainsFieldNames = true)
            {
                // get the key
                bool alphaColumnNames = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("AttributeNames").ToString());

                DataTable result = new DataTable();

                if (fileName == "")
                {
                    return result;
                }

                int rowLimit = 1000;
                string columnName = string.Empty;
                int k = 0;
                string delimiters = ",";
                string extension = Path.GetExtension(fileName);

                if (extension.ToLower() == "csv")
                    delimiters = ",";

                using (TextFieldParser tfp = new TextFieldParser(fileName))
                {
                    tfp.SetDelimiters(delimiters);

                    // Get The Column Names
                    if (!tfp.EndOfData)
                    {
                        string[] fields = tfp.ReadFields();

                        for (int i = 0; i < fields.Count(); i++)
                        {
                            if (firstRowContainsFieldNames)
                            {
                                //remove any special characters from the column names
                                if (alphaColumnNames)
                                    columnName = Regex.Replace(fields[i], @"[^0-9a-zA-Z]+", "");

                                result.Columns.Add(columnName);
                            }
                            else
                            {
                                result.Columns.Add("Col" + i);
                            }
                        }

                        // If first line is data then add it
                        if (!firstRowContainsFieldNames)
                            result.Rows.Add(fields);
                    }

                    // Get Remaining Rows from the CSV
                    while (!tfp.EndOfData)
                    {
                        //only load the first thousand records into grid
                        if (k < rowLimit)
                        {
                            result.Rows.Add(tfp.ReadFields());
                            k++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                
                return result;
            }
        }

        private DataTable ReadParquet(string fileName)
        {

            DataTable results = new DataTable();
            long maxRowCount = 50;
            long maxGroupCount = 1;

            // open file stream
            using (Stream fileStream = System.IO.File.OpenRead(fileName))
            {
                // open parquet file reader
                using (var parquetReader = new ParquetReader(fileStream))
                {
                    // get file schema (available straight after opening parquet reader)
                    // however, get only data fields as only they contain data values
                    DataField[] dataFields = parquetReader.Schema.GetDataFields();
                    
                    if (fileStructure.Columns.Count > 0)
                    {
                        fileStructure.Rows.Clear();
                        fileStructure.Columns.Clear();
                    }

                    //add the columns as names
                    for (int k=0; k< dataFields.Length;k++)
                    {
                        System.Data.DataColumn col = new System.Data.DataColumn(dataFields[k].Name, typeof(string));
                        fileStructure.Columns.Add(col);
                    }

                    //copy the column names from the structure to the data results
                    results = fileStructure.Clone();

                    //add a second row and populate with data type
                    DataRow dataRow = fileStructure.NewRow();
                    
                    for (int k = 0; k < dataFields.Length; k++)
                    {
                        dataRow[k] = dataFields[k].DataType;
                    }
                    fileStructure.Rows.Add(dataRow);

                    // enumerate through row groups in this file
                    for (int i = 0; i < maxGroupCount; i++)
                    {
                        // create row group reader
                        using (Parquet.ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(i))
                        {
                            // read all columns inside each row group (you have an option to read only
                            // required columns if you need to.
                            Parquet.Data.DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();

                            //go through each row and then column, limit to the first 5K records.
                            if (groupReader.RowCount < maxRowCount)
                                maxRowCount = groupReader.RowCount;

                             for (int a = 0; a < maxRowCount; a++)
                             {
                               // DataRow row = results.NewRow();
                                object[] array = new object[columns.Length];

                                for (int k = 0; k< columns.Length; k++)
                                {

                                    Parquet.Data.DataColumn cols = columns[k];
                             
                                    // .Data member contains a typed array of column data you can cast to the type of the column
                                    Array data = cols.Data;
                                    try
                                    {
                                        if (data.GetValue(k) != null)
                                        {
                                            array[k] = data.GetValue(k).ToString();
                                        }
                                        else
                                        {
                                            array[k] = null;
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine(ex.Message);
                                    }
                                }

                                //add the row to the results table
                                results.Rows.Add(array);
                            }
                        }
                    } 
                }
            }

            return results;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {

            bool loadData = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("PreviewData").ToString());
            bool tryDataTypes = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("DataTypes").ToString());

            if (txtFilePath.Text.Length > 0)
            {
                try
                { 

                    
                    //change the cursor to waiting while the data is loaded
                    Cursor curWait = Cursors.WaitCursor;
             
                    this.Cursor = curWait;

                    if (fileExtension == "csv")
                    { 
                        LoadCSVOnDataGridView(txtFilePath.Text, loadData, tryDataTypes);
                    }
                    if (fileExtension == "parquet")
                    {
                        LoadParquetOnDataGridView(txtFilePath.Text, loadData);
                    }
                    //generate the JSON
                    txtCDMJson.Text = GenerateJSONCDM();

                
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred: " + ex.Message);

                }
                finally
                {
                    Cursor curDefault = Cursors.Default;
                    this.Cursor = curDefault;
                }
            }
            else
            {
                MessageBox.Show("Please select a file.");
            }
        }

        private string GenerateJSONCDM()
        {
            string entityName = txtEntityName.Text.ToString();

            StringBuilder JSON = new StringBuilder();
            JSON.Append("{'jsonSchemaSemanticVersion':'1.1.0','imports':[{'corpusPath':'/" + entityName +".cdm.json','moniker':'resolvedFrom'}],");
            JSON.Append("'definitions':[{'entityName':'" + entityName + "','attributeContext':{'type':'entity','name':'" + entityName + "','definition':'resolvedFrom/" + entityName + "'},");
            JSON.Append("'hasAttributes':[");
            
           
            int i = 0;
            string columnName = string.Empty;
            string dataType = string.Empty;
            
           
            //get the header rows from the data grid and append them to the JSON
            while(i < fileStructure.Columns.Count)
            {
                columnName = fileStructure.Columns[i].ColumnName;
                dataType = fileStructure.Rows[0][i].ToString();
                
                JSON.Append("{'name':'" + columnName + "','dataFormat':'" + dataType + "'},");
                
                i++;
            }
            


            //remove the last comma
            JSON.Remove(JSON.Length - 1, 1);
            JSON.Append("],'version':'1.0.0.0'}]}");
            JSON.Replace("'", "\"");  
            
            //format the JSON
            try
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };

                var jsonElement = JsonSerializer.Deserialize<JsonElement>(JSON.ToString());

                return JsonSerializer.Serialize(jsonElement, options);
            }
            catch
            {
                return (JSON.ToString());
            }
        }

        private void btnJSON_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveFile()
        {
            try 
            { 
                string fileName = txtEntityName.Text + ".cdm.json";
                string filePath = Directory.GetCurrentDirectory().ToString();
                System.IO.File.WriteAllText(fileName, txtCDMJson.Text);

                DialogResult results = MessageBox.Show("The file " + fileName + " saved successfully to " + filePath,"Save dialog");
            }
            catch(Exception ex)
            {
                MessageBox.Show("There was an exception saving the file:"+ ex.Message,"Exception");
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Text = "CDM Generator (" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
        }

        private void ClearTables()
        {
            fileStructure.Rows.Clear();
            fileStructure.Columns.Clear();

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtCDMJson.Text);
            MessageBox.Show("CDM Copied to Clipboard!");
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmSettings settings = new frmSettings();
            settings.Show();
        }

        private void btnManifest_Click(object sender, EventArgs e)
        {
            frmManifest frmManifest = new frmManifest();
            frmManifest.Show();

        }
    }
}
