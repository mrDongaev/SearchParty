FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_URLS="http://*:8080"
WORKDIR /src
COPY ["./TeamPlayerProfiles/WebAPI/WebAPI.csproj", "./TeamPlayerProfiles/WebAPI/"]
COPY ["./TeamPlayerProfiles/Common/Common.csproj", "./TeamPlayerProfiles/Common/"]
COPY ["./TeamPlayerProfiles/DataAccess/DataAccess.csproj", "./TeamPlayerProfiles/DataAccess/"]
COPY ["./TeamPlayerProfiles/Service/Service.csproj", "./TeamPlayerProfiles/Service/"]
COPY ["./Library/*.csproj", "./Library/"]
RUN dotnet restore "./TeamPlayerProfiles/WebAPI/WebAPI.csproj"
COPY ["./TeamPlayerProfiles/", "./TeamPlayerProfiles/"]
COPY ["./Library/", "./Library/"]
WORKDIR "/src/TeamPlayerProfiles/"
COPY ["./TeamPlayerProfiles/migrations.sh", "."]
RUN chmod +x migrations.sh
RUN sed -i 's/\r$//' migrations.sh
ENTRYPOINT ["bash", "./migrations.sh"]
