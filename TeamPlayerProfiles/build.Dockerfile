FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./TeamPlayerProfiles/WebAPI/WebAPI.csproj", "./TeamPlayerProfiles/WebAPI/"]
COPY ["./TeamPlayerProfiles/Common/Common.csproj", "./TeamPlayerProfiles/Common/"]
COPY ["./TeamPlayerProfiles/DataAccess/DataAccess.csproj", "./TeamPlayerProfiles/DataAccess/"]
COPY ["./TeamPlayerProfiles/Service/Service.csproj", "./TeamPlayerProfiles/Service/"]
COPY ["./Library/*.csproj", "./Library/"]
RUN dotnet restore "./TeamPlayerProfiles/WebAPI/WebAPI.csproj"
COPY ["./TeamPlayerProfiles/", "./TeamPlayerProfiles/"]
COPY ["./Library/", "./Library/"]
WORKDIR "/src/TeamPlayerProfiles/WebAPI"
RUN dotnet build "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN apt-get update && apt-get install -y curl && apt-get clean
ENV ASPNETCORE_URLS="http://*:8080"
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]