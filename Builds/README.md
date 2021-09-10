# CDM-Generator
The CDM Generator is designed to generate default entity manifest files for AUI instances that use the attach to CDM. 

Intructions:
1) Select CSV or parquet files. Ensure that the first row contains the column names for CSV files.
2) Within the entiy table:
	- The entity name is defaulted based on the file name. Update the name as needed as it should be reflect in the manifest files
	- The regex patter is a sample. Updated as needed.
	- The root location is initially the entity name. Updated as needed.
3) (Optional) For the data preview, select the row within the entity table to preview the data. This is not required to generate the manifest files.
4) Click on Generate CDM to preview the default and entity manifest files.
4) Click on Save to save the default and entity manifest files. 

Settings:

Under the Settings menu, select Options for the following configurations:
  - Alphanumeric Attributes (CSV only) - Removes any special characters, spaces, etc. from the column names and leaves only alphanumeric values are use for column names.
  - Infer Data Types (CSV only) - CDM Generator will sample values in each column to test data types (Int, Decimal, and DateTime). Any values that do not parse correctly will default to string.
