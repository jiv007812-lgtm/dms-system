echo FROM mcr.microsoft.com/dotnet/aspnet:9.0 > Dockerfile
echo WORKDIR /app >> Dockerfile
echo COPY . . >> Dockerfile
echo ENV ASPNETCORE_URLS=http://+:80 >> Dockerfile
echo ENV ASPNETCORE_ENVIRONMENT=Production >> Dockerfile
echo EXPOSE 80 >> Dockerfile
echo CMD ["dotnet", "DMS.Presentation.dll"] >> Dockerfile
