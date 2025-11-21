FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY . .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=
EXPOSE 80
CMD ["dotnet", "DMS.Presentation.dll"]
