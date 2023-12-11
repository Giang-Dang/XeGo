#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Vehicle/XeGo.Services.Vehicle.API/XeGo.Services.Vehicle.API.csproj", "Services/Vehicle/XeGo.Services.Vehicle.API/"]
COPY ["Shared/XeGo.Shared.Lib/XeGo.Shared.Lib.csproj", "Shared/XeGo.Shared.Lib/"]
RUN dotnet restore "./Services/Vehicle/XeGo.Services.Vehicle.API/./XeGo.Services.Vehicle.API.csproj"
COPY . .
WORKDIR "/src/Services/Vehicle/XeGo.Services.Vehicle.API"
RUN dotnet build "./XeGo.Services.Vehicle.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./XeGo.Services.Vehicle.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XeGo.Services.Vehicle.API.dll"]

USER root
RUN apt-get update && apt-get install -y curl