FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Auth/APIAuth/APIAuth.csproj", "./Auth/APIAuth/"]
COPY ["./Auth/Application/Application.csproj", "./Auth/Application/"]
COPY ["./Auth/Domain/Domain.csproj", "./Auth/Domain/"]
COPY ["./Auth/EFData/EFData.csproj", "./Auth/EFData/"]
COPY ["./Auth/Infrastructure/Infrastructure.csproj", "./Auth/Infrastructure/"]
COPY ["./Library/*.csproj", "./Library/"]
RUN dotnet restore "./Auth/APIAuth/APIAuth.csproj"
COPY ["./Auth/", "./Auth/"]
COPY ["./Library/", "./Library/"]
WORKDIR "/src/Auth/APIAuth"
RUN dotnet build "./APIAuth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./APIAuth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN apt-get update && apt-get install -y curl && apt-get clean
ENV ASPNETCORE_URLS="http://*:8080"
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIAuth.dll"]