#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Auth/XeGo.Services.Auth.API/XeGo.Services.Auth.API.csproj", "Services/Auth/XeGo.Services.Auth.API/"]
COPY ["Shared/XeGo.Shared.Lib/XeGo.Shared.Lib.csproj", "Shared/XeGo.Shared.Lib/"]
RUN dotnet restore "./Services/Auth/XeGo.Services.Auth.API/./XeGo.Services.Auth.API.csproj"
COPY . .
WORKDIR "/src/Services/Auth/XeGo.Services.Auth.API"
RUN dotnet build "./XeGo.Services.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./XeGo.Services.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XeGo.Services.Auth.API.dll"]

USER root
RUN apt-get update && apt-get install -y curl