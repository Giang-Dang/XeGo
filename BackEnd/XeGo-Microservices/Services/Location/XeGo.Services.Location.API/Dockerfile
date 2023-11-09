#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Location/XeGo.Services.Location.API/XeGo.Services.Location.API.csproj", "Services/Location/XeGo.Services.Location.API/"]
COPY ["Shared/XeGo.Shared.GrpcConsumer/XeGo.Shared.GrpcConsumer.csproj", "Shared/XeGo.Shared.GrpcConsumer/"]
COPY ["Shared/XeGo.Shared.Lib/XeGo.Shared.Lib.csproj", "Shared/XeGo.Shared.Lib/"]
RUN dotnet restore "Services/Location/XeGo.Services.Location.API/XeGo.Services.Location.API.csproj"
COPY . .
WORKDIR "/src/Services/Location/XeGo.Services.Location.API"
RUN dotnet build "XeGo.Services.Location.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XeGo.Services.Location.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XeGo.Services.Location.API.dll"]