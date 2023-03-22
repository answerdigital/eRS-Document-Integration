FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=password123!

ADD Setup/*.sql ./

RUN ( /opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'password123!' -i ers_database_create.sql \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'password123!' -i ers_testdata_insert.sql \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'password123!' -i ers_workflow_master_insert.sql \
    && pkill sqlservr