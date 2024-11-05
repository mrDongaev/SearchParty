FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./UserInfo/WebAPI/WebAPI.csproj", "./UserInfo/WebAPI/"]
COPY ["./UserInfo/DataAccess/DataAccess.csproj", "./UserInfo/DataAccess/"]
COPY ["./UserInfo/Service/Service.csproj", "./UserInfo/Service/"]
COPY ["./Library/*.csproj", "./Library/"]
RUN dotnet restore "./UserInfo/WebAPI/WebAPI.csproj"
COPY ["./UserInfo/", "./UserInfo/"]
COPY ["./Library/", "./Library/"]
WORKDIR "/src/UserInfo/WebAPI"
RUN dotnet build "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_URLS="http://*:8080"
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]