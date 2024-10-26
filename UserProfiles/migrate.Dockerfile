FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_URLS="http://*:8080"
WORKDIR /src
COPY ["./UserProfiles/WebAPI/WebAPI.csproj", "UserProfiles/WebAPI/"]
COPY ["./UserProfiles/Common/Common.csproj", "UserProfiles/Common/"]
COPY ["./UserProfiles/DataAccess/DataAccess.csproj", "UserProfiles/DataAccess/"]
COPY ["./UserProfiles/Service/Service.csproj", "UserProfiles/Service/"]
COPY ["./Library/Library.csproj", "Library/"]
RUN dotnet restore "./UserProfiles/WebAPI/WebAPI.csproj"
COPY ["./UserProfiles/", "./UserProfiles/"]
COPY ["./Library/", "./Library/"]
WORKDIR "/src/UserProfiles"
COPY ["./UserProfiles/migrations.sh", "."]
RUN chmod +x migrations.sh
ENTRYPOINT ["./migrations.sh"]
