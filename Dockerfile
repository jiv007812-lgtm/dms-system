FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY dms-system/ .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "DMS.Presentation.dll"]
