FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /App
COPY DocumentReviewApplication*.zip DocumentReviewApplication.zip
RUN apt-get update
RUN apt-get install zip -y
RUN unzip DocumentReviewApplication.zip
ENTRYPOINT ["dotnet", "app/eRS.API.dll"]
EXPOSE 5001