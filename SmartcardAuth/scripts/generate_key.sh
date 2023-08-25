#!/bin/bash

read -p "Enter a key ID for the new key [test-1]: " KID
KID=${KID:-test-1}

openssl genrsa -out ../src/main/resources/keys/$KID.pem 4096
openssl rsa -in ../src/main/resources/keys/$KID.pem -pubout -outform PEM -out ../src/main/resources/certs/$KID.pem.pub


MODULUS=$(
    openssl rsa -pubin -in ../src/main/resources/certs/$KID.pem.pub -noout -modulus `# Print modulus of public key` \
    | cut -d '=' -f2                                    `# Extract modulus value from output` \
    | xxd -r -p                                         `# Convert from string to bytes` \
    | openssl base64 -A                                 `# Base64 encode without wrapping lines` \
    | sed 's|+|-|g; s|/|_|g; s|=||g'                    `# URL encode as JWK standard requires`
)


echo '{
  "keys": [
    {
      "kty": "RSA",
      "n": "'"$MODULUS"'",
      "e": "AQAB",
      "alg": "RS512",
      "kid": "'"$KID"'",
      "use": "sig"
    }
  ]
}' > ../src/main/resources/static/jwks/$KID.json


echo "private key written to ../src/main/resources/keys/$KID.pem"
echo "public key written to ../src/main/resources/certs/$KID.pem.pub"
echo "JWKS written to ../src/main/resources/static/jwks/$KID.json and available at http{s}://[base]/jwks/$KID.json"
echo "DO NOT COMMIT private keys"

