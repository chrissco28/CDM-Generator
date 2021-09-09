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
        DataGridView gridView = new DataGridView();
        

        public Main()
        {
            InitializeComponent();
            SetDataGrid();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
           SelectFiles();
        }

        private void SetDataGrid()
        {
            //add the columns to the data grid
            gridEntities.Columns.Add("FileName", "File");
            gridEntities.Columns.Add("FileType", "Type");
            gridEntities.Columns.Add("EntityName", "Entity Name");
            gridEntities.Columns.Add("RootLocation", "Root Location");
            gridEntities.Columns.Add("Regex", "Regex");
            gridEntities.Columns.Add("FilePath", "File Path");

            DataGridViewCheckBoxColumn colHeaders = new DataGridViewCheckBoxColumn();
            {
                colHeaders.HeaderText = "Has Headers";
                colHeaders.Name = "ColumnHeaders";
            }
            gridEntities.Columns.Add(colHeaders);

            gridEntities.Columns[0].ReadOnly = true;
            gridEntities.Columns[1].ReadOnly = true;
            gridEntities.Columns[5].ReadOnly = true;
        }

        private void SelectFiles()
        {
            StringBuilder sb = new StringBuilder();
            string fileName;
            string fullFileName;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|Parquet files (*.parquet)|*.parquet";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                string regex = string.Empty;
                bool hasHeaders = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //clear the grid
                    this.gridEntities.Rows.Clear();

                    //get the file names selected
                    for (int i=0; i< openFileDialog.FileNames.Length; i++)
                    {

                        fileName = openFileDialog.SafeFileNames[i].ToString();
                        fullFileName = openFileDialog.FileNames[i].ToString();

                        regex = @".+\\." + GetFileExtension(fileName) + "$";
                        this.gridEntities.Rows.Add(GetFileName(fileName), GetFileExtension(fileName), GetFileName(fileName), GetFileName(fileName), regex, fullFileName, hasHeaders);
                    }
                }
            }
        }

        private void LoadCSVStructureData(string fileName, bool loadData, bool tryDataTypes)
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

                if (fileStructure.Columns.Count > 0)
                {
                    fileStructure.Rows.Clear();
                    fileStructure.Columns.Clear();
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
                    //reset the row counter
                    rowCounter = 0;
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

        private void LoadParquetStructureData(string fileName, bool loadData)
        {
            try
            {
                if (loadData)
                    dataGridView.DataSource = ReadParquet(fileName, loadData);
                else//ignore the output; only call the function to build the structure
                    ReadParquet(fileName, loadData);
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

        private DataTable ReadParquet(string fileName, bool loadData)
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

                    if (loadData)
                    { 
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
            }

            return results;
        }

        private void PreviewData(string filePath, string fileExtension)
        {

            bool loadData = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("PreviewData").ToString());
            bool tryDataTypes = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("DataTypes").ToString());
            
            if (filePath.Length > 0)
            {
                try
                { 
                    //change the cursor to waiting while the data is loaded
                    Cursor curWait = Cursors.WaitCursor;
             
                    this.Cursor = curWait;

                    if (fileExtension == "csv")
                    {

                        if (dataGridView.Columns.Count > 0)
                        {
                            dataGridView.DataSource = null;
                            dataGridView.Rows.Clear();
                            dataGridView.Columns.Clear();
                        }

                        LoadCSVStructureData(filePath, loadData, tryDataTypes);
                    }
                    if (fileExtension == "parquet")
                    {
                        LoadParquetStructureData(filePath, loadData);
                    }
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

        private string GenerateJSONCDM(string entityName)
        {
            
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

            //get the file path
            DialogResult result = this.folderBrowserDialog.ShowDialog();

            try
            {
                //change the cursor to waiting while the data is loaded
                Cursor curWait = Cursors.WaitCursor;

                this.Cursor = curWait;
                PreviewSaveCDM(true, folderBrowserDialog.SelectedPath);
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

        private void saveFile(string fileName, string content, string location)
        {
            if (location.Length == 0)
                location = Directory.GetCurrentDirectory().ToString();

            string fullFilePath = location + @"\" + fileName;

            System.IO.File.WriteAllText(fullFilePath, content);
            
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Text = "CDM Generator (" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
        }


        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmSettings settings = new frmSettings();
            settings.Show();
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

        private void btnPreviewData_Click(object sender, EventArgs e)
        {
            //get the file path of the item selected
            if (gridEntities.SelectedRows.Count > 0)
            {
               
                string filePath = gridEntities.CurrentRow.Cells["FilePath"].Value.ToString();
                string fileExtension = gridEntities.CurrentRow.Cells["FileType"].Value.ToString();
                PreviewData(filePath, fileExtension);
            }
            else
                MessageBox.Show("Please select a row to preview the data", "CDM Generator");
                
        }

        private void btnCDM_Click(object sender, EventArgs e)
        {
            try
            { 
            //change the cursor to waiting while the data is loaded
            Cursor curWait = Cursors.WaitCursor;

            this.Cursor = curWait;
            PreviewSaveCDM(false, string.Empty);

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


        private string GenerateManifest()
        {
            StringBuilder entityManifest = new StringBuilder();
            string trait = string.Empty;
            string fileExtension = string.Empty;
            string fileName = string.Empty;
            string entityName = string.Empty;
            string rootLocation = string.Empty;
            string regEx = string.Empty;
            string hasHeaders = string.Empty;

            entityManifest.Append("{'manifestName': 'default','entities': [");
         
            //verify the files are loaded
            if (gridEntities.Rows.Count == 0)
            {
                MessageBox.Show("Please select one or more files.", "CDM Generator");
                return string.Empty;
            }

            foreach (DataGridViewRow row in gridEntities.Rows)
            {
                if (row.Cells[2].Value != null)
                { 
                    entityName = row.Cells[2].Value.ToString();
                    fileExtension = row.Cells[1].Value.ToString();


                    if (row.Cells[3].Value !=null)
                        rootLocation = row.Cells[3].Value.ToString();
                    if (row.Cells[4].Value != null)
                        regEx = @row.Cells[4].Value.ToString();
                    if (row.Cells[6].Value != null)
                        hasHeaders = @row.Cells[6].Value.ToString().ToLower();

                    if (entityName.Length > 0)
                    {
                        
                        if (fileExtension.ToLower() == "parquet")
                        { 
                            trait = "'traitReference': 'is.partition.format.parquet'";
                        }
                        if (fileExtension.ToLower() == "csv")
                        {
                            trait = "'traitReference': 'is.partition.format.CSV', ";
                            trait = trait + "'arguments': [{'name': 'columnHeaders','value': '" + hasHeaders + "', 'name': 'delimiter','value': ','}]";
                        }

                        entityManifest.Append("{'type': 'LocalEntity','entityName': '" + entityName + "','entityPath': 'resolved/" + entityName + ".cdm.json/" + entityName + "',");
                        entityManifest.Append("'dataPartitionPatterns': [{ 'name': '" + entityName + "Partitions','rootLocation': '" + rootLocation + "','regularExpression': '" + regEx + "','parameters': [],'exhibitsTraits': [{" + trait + "}]}]");
                        entityManifest.Append(",'definitions': [{'traitName': 'is.formatted','extendsTrait': 'is'},");
                        entityManifest.Append("{'traitName': 'is.formatted.dateTime','extendsTrait': 'is.formatted',");
                        entityManifest.Append("'hasParameters': [{'name': 'format','dataType': 'stringFormat','defaultValue': 'YYYY-MM-DDThh:mmZ'}]},");
                        entityManifest.Append("{'traitName': 'is.formatted.date','extendsTrait': 'is.formatted',");
                        entityManifest.Append("'hasParameters': [{'name': 'format','dataType': 'stringFormat','defaultValue': 'YYYY-MM-DD'}]},");
                        entityManifest.Append("{'traitName': 'is.formatted.time','extendsTrait': 'is.formatted',");
                        entityManifest.Append("'hasParameters': [{'name': 'format','dataType': 'stringFormat','defaultValue': 'hh:mm:ss'}]}]},");
                    }
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
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
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

        private void PreviewSaveCDM(bool save, string location)
        {
            StringBuilder sb = new StringBuilder();
            string entityName;
            string fullFilePath;
            string fileExtension;
            string cdmName;
            string JSON;
            bool exception = false;
           
            //verify the files are loaded
            if (gridEntities.Rows.Count == 0)
            {
                MessageBox.Show("Please select one or more files", "CDM Generator");
                return;
            }

            //write out each of the entity JSON first
            foreach (DataGridViewRow row in gridEntities.Rows)
            {
                if (row.Cells[2].Value != null)
                {
                    fullFilePath = row.Cells[5].Value.ToString();
                    entityName = row.Cells[2].Value.ToString();
                    fileExtension = row.Cells[1].Value.ToString();

                    if (fileExtension == "csv")
                    {
                        LoadCSVStructureData(fullFilePath, true, true);
                    }
                    if (fileExtension == "parquet")
                    {
                        LoadParquetStructureData(fullFilePath, false);
                    }

                    JSON = GenerateJSONCDM(entityName);
                    sb.Append("# Start of " + entityName + " Manifest ");
                    sb.Append(Environment.NewLine);
                    sb.Append(JSON);
                    sb.Append(Environment.NewLine);
                    sb.Append("# end of " + entityName + " Manifest ");
                    sb.Append(Environment.NewLine);

                    if (save)
                    {
                        try
                        { 
                            cdmName = entityName + ".cdm.json";
                            saveFile(cdmName, JSON, location);
                        }
                        catch(Exception ex)
                        {
                            exception = true;
                            MessageBox.Show("There was an exception saving the manifest file for " + entityName + "file: " + ex.Message, "CDM Generator");
                        }
                    }
                }
            }
            txtCDMJson.Text = sb.ToString();
            richDefaultManifest.Text = GenerateManifest();
            //generate the default manifest content
            if (save)
            {
                try
                {
                    cdmName = "default.manifest.cdm.json";
                    saveFile(cdmName, richDefaultManifest.Text, location);

                    if (exception == false)
                        MessageBox.Show("The files have been saved.", "CDM Generator");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an exception saving the default manifest file: " + ex.Message, "CDM Generator");
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
