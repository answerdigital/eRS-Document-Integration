# Rotherham e-RS Integration Web-Application

Clone the project:

`$ git clone git@github.com:AnswerConsulting/eRS-Document-Integration.git`

## SQL DATABASE

Ensure you have an instance of [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) installed and running on your machine

Install the latest version of [SQL Server Management Studio](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms) and connect to your server

Navigate to the `Setup` folder and run the script `ers_database_create.sql` in SQL Server Management Studio to generate the database

Using a text editor, open `eRS-Document-Integration/API/eRS.API/appsettings.json` and edit the line `DefaultConnection` to associate to your SQL Server's database connection string.

## API

Ensure you have the latest [.NET Core 6 SDK](https://www.microsoft.com/net/download/) installed

CD into the project API folder:

`$ cd eRS-Document-Integration/API`

Run the project:

`$ dotnet run --project eRS.API/eRS.API.csproj`

To confirm that the application is running, navigate to `https://localhost:7204` in your browser to view the api interface

## WEB APPLICATION

CD into the project API folder:

`$ cd eRS-Document-Integration/Client`

Install the required packages:

`$ npm install`

Run the project:

`$ npm run start`

Navigate to `https://localhost:3000` while the API is running to view and use the web application
