FROM containers.intersystems.com/intersystems/irishealth-community:2023.2.0.227.0
WORKDIR /home/irisowner/
COPY --chown=irisowner:irisowner Integration-*.zip .
USER root
RUN apt-get update
RUN apt-get install -y curl
RUN curl https://packages.microsoft.com/keys/microsoft.asc | tee /etc/apt/trusted.gpg.d/microsoft.asc
RUN curl https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/prod.list | tee /etc/apt/sources.list.d/mssql-release.list
RUN apt-get update
RUN apt-get install zip -y
#RUN apt-get install libreoffice --no-install-recommends -y
#RUN apt-get install clamav -y
#RUN freshclam
RUN ACCEPT_EULA=Y apt-get install -y msodbcsql18
COPY --chmod=777 <<EOF /etc/odbc.ini
[ERS_SQL]
Driver = ODBC Driver 18 for SQL Server  
Server = tcp:sql,1433
Encrypt = yes
TrustServerCertificate = yes
EOF
USER irisowner
RUN unzip Integration-*.zip
ENTRYPOINT [ "/iris-main", "-c echo -n ${IRIS_PASSWORD} > password.txt", "-p password.txt"]
EXPOSE 1972 2188 52773 53773 54773 9985