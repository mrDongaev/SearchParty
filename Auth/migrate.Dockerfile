FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_URLS="http://*:8080"
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
WORKDIR "/src/Auth/"
COPY ["./Auth/migrations.sh", "."]
RUN chmod +x migrations.sh
RUN sed -i 's/\r$//' migrations.sh
ENTRYPOINT ["bash", "./migrations.sh"]
