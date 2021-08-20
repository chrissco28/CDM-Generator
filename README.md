# CDM-Generator
The CDM Generator is designed to generate entity manifest files for AUI instances that use the attach to CDM. 

Intructions:
1) Select a CS or parquet file. Ensure that the first row contains the column names for CSV files.
2) The entity name is defaulted based on the file name. Update the name as needed
3) Load the file to preview the records and to generate the CDM JSON
4) Save the JSON. The JSON file will use the entity name provided to the directory where the CDM Generator is running.


Notes:
- For CSV files All columns default all data types to strings.
- For CSV files Special characters, spaces, etc. are removed from the column names. Alphanumeric values are use for column names.
