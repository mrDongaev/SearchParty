FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./UserProfiles/WebAPI/WebAPI.csproj", "./UserProfiles/WebAPI/"]
COPY ["./UserProfiles/Common/Common.csproj", "./UserProfiles/Common/"]
COPY ["./UserProfiles/DataAccess/DataAccess.csproj", "./UserProfiles/DataAccess/"]
COPY ["./UserProfiles/Service/Service.csproj", "./UserProfiles/Service/"]
COPY ["./Library/*.csproj", "./Library/"]
RUN dotnet restore "./UserProfiles/WebAPI/WebAPI.csproj"
COPY ["./UserProfiles/", "./UserProfiles/"]
COPY ["./Library/", "./Library/"]
WORKDIR "/src/UserProfiles/WebAPI"
RUN dotnet build "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_URLS="http://*:8080"
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]