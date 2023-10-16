FROM amazoncorretto:17
COPY ers-user-auth-app-*.jar /App/ers-user-auth-app.jar
COPY *.pem /App/config/

ENV SERVER_PORT 8443
ENV CLIENT_SECRET=
ENV ERS_SESSIONHANDOVERENDPOINT=
ENV CLIENT_ID=
ENV SERVER_SSL_CERTIFICATEPRIVATEKEY=
ENV REDIRECT_URI https://localhost:8443/login/oauth2/code/cis2
ENV OAUTH_ENDPOINT https://int.api.service.nhs.uk/oauth2-mock
ENV SERVER_HTTPPORT 80
ENV SERVER_SSL_CERTIFICATE=
ENV ODS_CODE=
ENV SERVER_ENABLETLS true
EXPOSE 8080 80 8443
ENTRYPOINT ["java","-jar","/App/ers-user-auth-app.jar"]