UI for the Rotherham e-RS system  

Instructions for setup:  
1. Create the database using the ers_database_create.sql and ers_workflow_master_insert.sql scripts in the Setup folder  
2. In API\eRS.API\appsettings.json, edit the DefaultConnection entry to use your database connection string  
3. Run the API  
4. In Client\.env make sure the REACT_APP_API_Address variable points to the API address  
5. In the Client folder, run the commands npm install, then npm run  