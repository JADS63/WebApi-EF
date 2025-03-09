FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copie des fichiers .csproj individuels (pour le caching)
COPY ["WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj", "WebApplicationJuaraujoda/WebApplicationJuaraujoda/"]
COPY ["WebApplicationJuaraujoda/Dto/Dto.csproj", "WebApplicationJuaraujoda/Dto/"]
COPY ["WebApplicationJuaraujoda/Services/Services.csproj", "WebApplicationJuaraujoda/Services/"]
COPY ["WebApplicationJuaraujoda/Stub/StubbedDtoLib.csproj", "WebApplicationJuaraujoda/Stub/"]
COPY ["WebApplicationJuaraujoda/WebApiUtilisation/WebApiUtilisation.csproj", "WebApplicationJuaraujoda/WebApiUtilisation/"]
COPY ["WebApplicationJuaraujoda/Extensions/Extensions.csproj", "WebApplicationJuaraujoda/Extensions/"]
COPY ["WebApplicationJuaraujoda/Entities/Entities.csproj", "WebApplicationJuaraujoda/Entities/"]
COPY ["WebApplicationJuaraujoda/Tests/Tests.csproj", "WebApplicationJuaraujoda/Tests/"]
# Copie du fichier de solution
COPY WebApplicationJuaraujoda/WebApplicationJuaraujoda.sln .
RUN dotnet restore "WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj"
COPY . .
WORKDIR "/src/WebApplicationJuaraujoda/WebApplicationJuaraujoda"
RUN dotnet build "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final # Étape 'final' directement basée sur aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80 # Exposition du port 80 (standard HTTP sur CodeFirst)
ENV ASPNETCORE_HTTP_PORTS=80 # Configuration pour écouter sur le port 80
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false # Désactivation du rechargement de config au changement
ENTRYPOINT ["dotnet", "WtaApi.dll"]