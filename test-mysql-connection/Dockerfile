#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
ENV MYSQLCONNSTR="Server=host.docker.internal;Database=db1;Uid=root;Pwd=mysqlPW;"
ENV MYSQLCMD="SELECT COUNT(*) FROM T1"
ENV INTERVAL=5
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["test-mysql-connection/test-mysql-connection.csproj", "test-mysql-connection/"]
RUN dotnet restore "./test-mysql-connection/test-mysql-connection.csproj"
COPY . .
WORKDIR "/src/test-mysql-connection"
RUN dotnet build "./test-mysql-connection.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./test-mysql-connection.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test-mysql-connection.dll"]