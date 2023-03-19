#!/bin/bash

openssl req -x509 -newkey rsa:4096 -keyout ../src/main/resources/keys/server.pem -out ../src/main/resources/certs/server.pem.pub -sha256 -days 365 -nodes -subj '/CN=localhost'
openssl x509 -outform der -in ../src/main/resources/certs/server.pem.pub -out ../src/main/resources/certs/server.crt

echo "server private key written to ../src/main/resources/keys/server.pem"
echo "server key written to ../src/main/resources/certs/server.pem.pub"
echo "server key written to ../src/main/resources/certs/server.crt - add this to Windows Trusted CAs"
echo "DO NOT COMMIT private keys"

