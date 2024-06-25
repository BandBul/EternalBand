#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EternalBAND.Win/EternalBAND.Win.csproj", "EternalBAND.Win/"]
COPY ["EternalBAND.Api/EternalBAND.Api.csproj", "EternalBAND.Api/"]
COPY ["EternalBAND.DataAccess/EternalBAND.DataAccess.csproj", "EternalBAND.DataAccess/"]
COPY ["EternalBAND.DomainObjects/EternalBAND.DomainObjects.csproj", "EternalBAND.DomainObjects/"]
COPY ["EternalBAND.Common/EternalBAND.Common.csproj", "EternalBAND.Common/"]
COPY ["EternalBAND.Migrations/EternalBAND.Migrations.csproj", "EternalBAND.Migrations/"]
COPY "../../cert/bandbulCert.pfx" "app/cert/" 
RUN dotnet restore "EternalBAND.Win/EternalBAND.Win.csproj"
COPY . .
WORKDIR "/src/EternalBAND.Win"
RUN dotnet build "EternalBAND.Win.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EternalBAND.Win.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EternalBAND.Win.dll"]