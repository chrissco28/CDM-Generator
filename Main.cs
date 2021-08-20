﻿using System;
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

namespace CDM_Generator
{
    public partial class Main : Form
    {
        string fileName = string.Empty;
        string fileExtension = string.Empty;

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
                    fileExtension = openFileDialog.SafeFileName.Substring(length - index, index).ToLower();
                    txtEntityName.Text = fileName;
                }
            }
        }

        private void LoadCSVOnDataGridView(string fileName)
        {
            try
            {
                ReadCSV csv = new ReadCSV(fileName);

                try
                {
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

        private void LoadParquetOnDataGridView(string fileName)
        {
            try
            {
                ReadParquet(fileName);

                try
                {
                    //dataGridView.DataSource = csv.readCSV;
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

        private class ReadCSV
        {
            public DataTable readCSV;

            public ReadCSV(string fileName, bool firstRowContainsFieldNames = true)
            {
                readCSV = GenerateDataTable(fileName, firstRowContainsFieldNames);
            }

            private static DataTable GenerateDataTable(string fileName, bool firstRowContainsFieldNames = true)
            {
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

        private void ReadParquet(string fileName)
        {
            // open file stream
            using (Stream fileStream = System.IO.File.OpenRead(fileName))
            {
                // open parquet file reader
                using (var parquetReader = new ParquetReader(fileStream))
                {
                    // get file schema (available straight after opening parquet reader)
                    // however, get only data fields as only they contain data values
                    DataField[] dataFields = parquetReader.Schema.GetDataFields();
                    dataGridView.ColumnCount = dataFields.Length;
                    //System.Diagnostics.Debug.WriteLine(dataFields[k].IsArray);

                    //add the columns as names
                    for (int k=0; k< dataFields.Length;k++)
                    {
                        dataGridView.Columns[k].Name = dataFields[k].Name;
                        System.Diagnostics.Debug.WriteLine(dataFields[k].DataType);
                        
                    }
                    //add a second row and populate with data type
                    DataGridViewRow row = (DataGridViewRow)dataGridView.Rows[0].Clone();
                    for (int k = 0; k < dataFields.Length; k++)
                    {
                        row.Cells[k].Value = dataFields[k].DataType;
                    }
                    dataGridView.Rows.Add(row);

                    /*
                    // enumerate through row groups in this file
                    for (int i = 0; i < parquetReader.RowGroupCount; i++)
                    {
                        // create row group reader
                        using (Parquet.ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(i))
                        {
                            // read all columns inside each row group (you have an option to read only
                            // required columns if you need to.
                            Parquet.Data.DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();

                           // get first column, for instance
                            Parquet.Data.DataColumn firstColumn = columns[0];

                            // .Data member contains a typed array of column data you can cast to the type of the column
                            Array data = firstColumn.Data;
                            int[] ids = (int[])data;
                        }
                       
                    } */
                }
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            if (txtFilePath.Text.Length > 0)
            {
                if (fileExtension == "csv")
                { 
                    LoadCSVOnDataGridView(txtFilePath.Text);
                }
                if (fileExtension == "parquet")
                {
                    LoadParquetOnDataGridView(txtFilePath.Text);
                }
                //generate the JSON
                txtCDMJson.Text = GenerateJSONCDM();
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
            
           
            //get the header rows from the data grid and append them to the JSON
            while(i < dataGridView.Columns.Count)
            {
                columnName = dataGridView.Columns[i].HeaderText;
                if (fileExtension == "csv")
                {
                    JSON.Append("{'name':'" + columnName + "','dataFormat':'String'},");
                }
                if (fileExtension == "parquet")
                {
                    if (dataGridView.Rows[0].Cells[i].Value != null)
                    { 
                        JSON.Append("{'name':'" + columnName + "','dataFormat':'" + dataGridView.Rows[0].Cells[i].Value.ToString()+ "'},");
                    }
                }
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
    }
}
