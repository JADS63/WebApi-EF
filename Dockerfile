FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copie des fichiers .csproj individuels (pour le caching)
# Les chemins sont RELATIFS à la racine du dépôt.
COPY ["WebApplicationJuaraujoda/WtaApi.csproj", "WebApplicationJuaraujoda/"]
COPY ["Dto/Dto.csproj", "Dto/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Stub/StubbedDtoLib.csproj", "Stub/"]
COPY ["WebApiUtilisation/WebApiUtilisation.csproj", "WebApiUtilisation/"]
COPY ["Extensions/Extensions.csproj", "Extensions/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Tests/Tests.csproj", "Tests/"]

# Copie du fichier de solution
COPY WebApplicationJuaraujoda.sln .

# Restauration des dépendances du projet principal
RUN dotnet restore "WebApplicationJuaraujoda/WtaApi.csproj"

# Copie de *tout* le code source
COPY . .

# Définition du répertoire de travail pour le build
WORKDIR "/src/WebApplicationJuaraujoda"
RUN dotnet build "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WtaApi.dll"]