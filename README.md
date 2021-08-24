# CDM-Generator
The CDM Generator is designed to generate entity manifest files for AUI instances that use the attach to CDM. 

Intructions:
1) Select a CS or parquet file. Ensure that the first row contains the column names for CSV files.
2) The entity name is defaulted based on the file name. Update the name as needed
3) Load the file to preview the records and to generate the CDM JSON
4) Save the JSON. The JSON file will use the entity name provided to the directory where the CDM Generator is running.


Settings:

Under the Settings menu, select Options for the following configurations:
  - Alphanumeric Attributes (CSV only) - Removes any special characters, spaces, etc. from the column names and leaves only alphanumeric values are use for column names.
  - Infer Data Types (CSV only) - CDM Generator will sample values in each column to test data types (Int, Decimal, and DateTime). Any values that do not parse correctly will default to string.
  - Preview Data - turn on/off the tool's data preview feature.
  - Generate Manifest - Generate the JSON needed for the supporting default.manifest.cdm.json file
