# Étape 1 : Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copier le fichier de projet de l’API
COPY ["WebApplicationJuaraujoda/WtaApi.csproj", "WebApplicationJuaraujoda/"]
# Copier l’ensemble de la solution
COPY . .
# Restaurer les packages pour le projet API
RUN dotnet restore "WebApplicationJuaraujoda/WtaApi.csproj"
WORKDIR "/src/WebApplicationJuaraujoda"
# Compiler le projet en mode Release
RUN dotnet build "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Étape 2 : Publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Étape 3 : Image d'exécution
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# Exposer le port 80 (ASP.NET Core écoute sur ce port par défaut)
EXPOSE 80
# Optionnel : vous pouvez définir l'environnement en Production
ENV ASPNETCORE_ENVIRONMENT=Production
# Copier les fichiers publiés depuis l'étape de publication
COPY --from=publish /app/publish .
# Point d'entrée de l'application
ENTRYPOINT ["dotnet", "WtaApi.dll"]
