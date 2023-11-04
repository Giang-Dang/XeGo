#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/CodeValue/XeGo.Services.CodeValue.API/XeGo.Services.CodeValue.API.csproj", "Services/CodeValue/XeGo.Services.CodeValue.API/"]
COPY ["Shared/XeGo.Shared.Lib/XeGo.Shared.Lib.csproj", "Shared/XeGo.Shared.Lib/"]
RUN dotnet restore "Services/CodeValue/XeGo.Services.CodeValue.API/XeGo.Services.CodeValue.API.csproj"
COPY . .
WORKDIR "/src/Services/CodeValue/XeGo.Services.CodeValue.API"
RUN dotnet build "XeGo.Services.CodeValue.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XeGo.Services.CodeValue.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XeGo.Services.CodeValue.API.dll"]