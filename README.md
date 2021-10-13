# CDM-Generator

The CDM Generator is designed to generate the default and entity manifest files for AUI instances that use the attach to CDM.

Intructions:

1) Select one or more CSV or parquet files. In the CSV file, the first row should contain the column name. If a column name isn't present then the utility will use "Column" + Column Number so Column0, Column1, etc.
2) Within the entity grid, update the entity name (default is the file name), the root location for the default manifest file, and regex pattern that the files for CI to use within the default manifest file.
3) To preview the data, select the entity row and click on Preview Data.
4) Click on 'Generate CDM' to preview the default and entity manifest files.
5) Click on 'Save' to save the default manifest file, each entity manifest file, and data maps for each entity.

Notes:
a) The utility will try to infer the correct data types in CSV files. Because of the potential for values to be converted properly with multiple data types, it is important to verify the right data type is selected. To turn this feature off, go to Settings -> Options
b) The utility by default will create column names that are alphanumeric only in the entity manifest files (for CSV only). To turn this off, go to Settings -> Options
