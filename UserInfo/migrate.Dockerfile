FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_URLS="http://*:8080"
WORKDIR /src
COPY ["./UserInfo/WebAPI/WebAPI.csproj", "UserInfo/WebAPI/"]
COPY ["./UserInfo/DataAccess/DataAccess.csproj", "UserInfo/DataAccess/"]
COPY ["./UserInfo/Service/Service.csproj", "UserInfo/Service/"]
COPY ["./Library/Library.csproj", "Library/"]
RUN dotnet restore "./UserInfo/WebAPI/WebAPI.csproj"
COPY ["./UserInfo/", "./UserInfo/"]
COPY ["./Library/", "./Library/"]
WORKDIR "/src/UserInfo"
COPY ["./UserInfo/migrations.sh", "."]
RUN chmod +x migrations.sh
ENTRYPOINT ["bash", "./migrations.sh"]
