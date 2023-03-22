FROM mcr.microsoft.com/mssql/server:2019-latest

ADD Setup/*.sql ./

RUN sqlcmd -S localhost -i ers_database_create.sql

RUN sqlcmd -S localhost -i ers_workflow_master_insert.sql