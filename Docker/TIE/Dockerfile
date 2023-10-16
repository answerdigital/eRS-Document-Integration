FROM containers.intersystems.com/intersystems/irishealth-community:2023.2.0.227.0
ARG IRIS_PASSWORD
WORKDIR /home/irisowner/
COPY --chown=irisowner:irisowner Integration-*.zip .
COPY --chown=irisowner:irisowner --chmod=+700 <<EOF set_password.sh
#!/bin/bash
echo -n ${IRIS_PASSWORD} > password.txt
EOF
USER root
RUN apt-get update
RUN apt-get install zip -y
RUN apt-get install libreoffice --no-install-recommends -y
RUN apt-get install clamav -y
RUN freshclam
USER irisowner
RUN unzip Integration-*.zip
EXPOSE 1972 2188 52773 53773 54773