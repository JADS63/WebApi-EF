FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
WORKDIR /app

ARG configuration=Release

# Copier tous les fichiers du dépôt dans l'image
COPY . .

# (Optionnel) Afficher l'arborescence pour vérifier
RUN ls -R /app

# Restaurer, construire et publier le projet Web API
# Utilisez un chemin absolu dans l'image (si nécessaire, vous pouvez forcer la copie du dossier)
RUN dotnet restore "/app/WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj" \
    && dotnet build "/app/WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj" -c $configuration -o build \
    && dotnet publish "/app/WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj" -c $configuration -o publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 80

ENV ASPNETCORE_HTTP_PORTS=80
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

ENTRYPOINT ["dotnet", "WtaApi.dll"]
