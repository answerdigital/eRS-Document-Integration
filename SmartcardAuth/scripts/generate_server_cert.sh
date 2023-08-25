#!/bin/bash

SERVER_NAME=localhost
PRIVATE_KEY=../src/main/resources/keys/server.pem
PUBLIC_CERT=../src/main/resources/certs/server.pem.pub
PUBLIC_CERT_FOR_BROWSER=../src/main/resources/certs/server.crt

openssl req -x509 -newkey rsa:4096 -keyout $PRIVATE_KEY -out $PUBLIC_CERT -sha256 -days 365 -nodes -subj "/CN=$SERVER_NAME"
openssl x509 -outform der -in $PUBLIC_CERT -out $PUBLIC_CERT_FOR_BROWSER

echo "server private key written to $PRIVATE_KEY"
echo "server certificate written to $PUBLIC_CERT"
echo "server certificate in .cert form written to $PUBLIC_CERT_FOR_BROWSER - add this to Windows Trusted CAs"
echo "DO NOT COMMIT private keys"

