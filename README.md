# CDM-Generator
The CDM Generator is designed to generate entity manifest files for AUI instances that use the attach to CDM. 

Intructions:
1) Select a CSV. Ensure that the first row contains the column names
2) The entity name is defaulted based on the file name. Update the name as needed
3) Load the file to preview the first 1K records in the CSV file and to generate the CDM JSON
4) Save the JSON. The JSON file will use the entity name provided to the directory where the CDM Generator is running.


Limitations/Notes:
- Version 1.0 only supports CSV files.
- All columns default all data types to strings.
- Special characters, spaces, etc. are removed from the column names. Alphanumeric values are use for column names.
