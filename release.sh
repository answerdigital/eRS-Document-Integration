#!/bin/sh

set -x

RELEASE_TAG=v0.0.2

rm -rf target

mkdir target

cd Integration
zip ../target/Integration-$RELEASE_TAG.zip *
cd -

cp SmartcardAuth-Build/ers-user-auth-app-$RELEASE_TAG.jar target/

mkdir target/WindowsServiceDeployment

cp SmartcardAuth/WindowsServiceDeployment/* target/WindowsServiceDeployment
cp SmartcardAuth-Build/ers-user-auth-app-$RELEASE_TAG.jar target/WindowsServiceDeployment

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