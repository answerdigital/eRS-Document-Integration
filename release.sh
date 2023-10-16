#!/bin/bash

set -x

RELEASE_TAG=${1:-vSNAPSHOT}

rm -rf target

mkdir target

cd Integration
zip -r ../target/Integration-$RELEASE_TAG.zip *
cd -

cp SmartcardAuth-Build/ers-user-auth-app-*.jar target/ers-user-auth-app-$RELEASE_TAG.jar

mkdir target/WindowsServiceDeployment

cp SmartcardAuth/WindowsServiceDeployment/* target/WindowsServiceDeployment
cp SmartcardAuth-Build/ers-user-auth-app-*.jar target/WindowsServiceDeployment

cd target/WindowsServiceDeployment
zip ../WindowsServiceDeployment-$RELEASE_TAG.zip *
cd -

mkdir target/DocumentReviewApplication
cp -r Client-Build/* target/DocumentReviewApplication/ 
mkdir target/DocumentReviewApplication/app
cp -r API-Build/* target/DocumentReviewApplication/app

cd target/DocumentReviewApplication
zip -r ../DocumentReviewApplication-$RELEASE_TAG.zip *
cd -

cd SQL
zip -r ../target/SQL-$RELEASE_TAG.zip *
cd -

cp Release.txt target
cp LICENSE target