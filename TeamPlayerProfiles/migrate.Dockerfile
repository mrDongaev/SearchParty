FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_URLS="http://*:8080"
WORKDIR /src
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Service/Service.csproj", "Service/"]
RUN dotnet restore "./WebAPI/WebAPI.csproj"
COPY . .
COPY migrations.sh .
RUN chmod +x migrations.sh
ENTRYPOINT ["./migrations.sh"]
