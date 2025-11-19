FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "DMS.sln"
RUN dotnet publish "DMS.Presentation/DMS.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
ENV ASPNETCORE_HTTPS_PORT=
ENTRYPOINT ["dotnet", "DMS.Presentation.dll"]