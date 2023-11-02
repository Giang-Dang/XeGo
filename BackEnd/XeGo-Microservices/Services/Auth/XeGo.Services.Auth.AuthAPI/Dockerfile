#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Auth/XeGo.Services.Auth.AuthAPI/XeGo.Services.Auth.AuthAPI.csproj", "Services/Auth/XeGo.Services.Auth.AuthAPI/"]
RUN dotnet restore "Services/Auth/XeGo.Services.Auth.AuthAPI/XeGo.Services.Auth.AuthAPI.csproj"
COPY . .
WORKDIR "/src/Services/Auth/XeGo.Services.Auth.AuthAPI"
RUN dotnet build "XeGo.Services.Auth.AuthAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XeGo.Services.Auth.AuthAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XeGo.Services.Auth.AuthAPI.dll"]