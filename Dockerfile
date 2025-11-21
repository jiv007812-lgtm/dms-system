@"
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY ./dms-system .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "DMS.Presentation.dll"]
"@ | Set-Content Dockerfile