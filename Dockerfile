FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- DIAGNOSTIC: Lister le contenu de /src (qui devrait être vide au début) ---
RUN echo "Contenu de /src AVANT toute copie :"
RUN ls -la /src

# Copie des fichiers .csproj individuels (pour le caching)
# --- DIAGNOSTIC: Lister le contenu AVANT chaque COPY ---
RUN echo "Contenu de /src AVANT de copier WtaApi.csproj :"
RUN ls -la /src
COPY ["WebApplicationJuaraujoda/WtaApi.csproj", "WebApplicationJuaraujoda/"]

RUN echo "Contenu de /src/WebApplicationJuaraujoda APRES avoir copié WtaApi.csproj :"
RUN ls -la /src/WebApplicationJuaraujoda

COPY ["Dto/Dto.csproj", "Dto/"]
RUN echo "Contenu de /src APRES avoir copié Dto.csproj :"
RUN ls -la /src

COPY ["Services/Services.csproj", "Services/"]
RUN echo "Contenu de /src APRES avoir copié Services.csproj :"
RUN ls -la /src

COPY ["Stub/StubbedDtoLib.csproj", "Stub/"]
COPY ["WebApiUtilisation/WebApiUtilisation.csproj", "WebApiUtilisation/"]
COPY ["Extensions/Extensions.csproj", "Extensions/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Tests/Tests.csproj", "Tests/"]
# Copie du fichier de solution
COPY WebApplicationJuaraujoda.sln .
RUN dotnet restore "WebApplicationJuaraujoda/WtaApi.csproj"
COPY . .
WORKDIR "/src/WebApplicationJuaraujoda"
RUN dotnet build "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WtaApi.dll"]