FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_URLS="http://*:8080"
WORKDIR /src
COPY ["./UserMessaging/WebAPI/WebAPI.csproj", "./UserMessaging/WebAPI/"]
COPY ["./UserMessaging/DataAccess/DataAccess.csproj", "./UserMessaging/DataAccess/"]
COPY ["./UserMessaging/Service/Service.csproj", "./UserMessaging/Service/"]
COPY ["./Library/*.csproj", "./Library/"]
RUN dotnet restore "./UserMessaging/WebAPI/WebAPI.csproj"
COPY ["./UserMessaging/", "./UserMessaging/"]
COPY ["./Library/", "./Library/"]
WORKDIR "/src/UserMessaging/"
COPY ["./UserMessaging/migrations.sh", "."]
RUN chmod +x migrations.sh
RUN sed -i 's/\r$//' migrations.sh
ENTRYPOINT ["bash", "./migrations.sh"]
