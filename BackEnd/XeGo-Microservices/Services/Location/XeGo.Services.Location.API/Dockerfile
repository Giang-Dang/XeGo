#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Location/XeGo.Services.Location.API/XeGo.Services.Location.API.csproj", "Services/Location/XeGo.Services.Location.API/"]
COPY ["Shared/XeGo.Shared.GrpcConsumer/XeGo.Shared.GrpcConsumer.csproj", "Shared/XeGo.Shared.GrpcConsumer/"]
COPY ["Shared/XeGo.Shared.Lib/XeGo.Shared.Lib.csproj", "Shared/XeGo.Shared.Lib/"]
RUN dotnet restore "./Services/Location/XeGo.Services.Location.API/./XeGo.Services.Location.API.csproj"
COPY . .
WORKDIR "/src/Services/Location/XeGo.Services.Location.API"
RUN dotnet build "./XeGo.Services.Location.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./XeGo.Services.Location.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XeGo.Services.Location.API.dll"]

USER root
RUN apt-get update && apt-get install -y curl