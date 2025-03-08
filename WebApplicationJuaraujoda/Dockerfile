FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj", "WebApplicationJuaraujoda/WebApplicationJuaraujoda/"]
COPY ["WebApplicationJuaraujoda/Dto/Dto.csproj", "WebApplicationJuaraujoda/Dto/"]
COPY ["WebApplicationJuaraujoda/Services/Services.csproj", "WebApplicationJuaraujoda/Services/"]
COPY ["WebApplicationJuaraujoda/Stub/StubbedDtoLib.csproj", "WebApplicationJuaraujoda/Stub/"]
COPY ["WebApplicationJuaraujoda/WebApiUtilisation/WebApiUtilisation.csproj", "WebApplicationJuaraujoda/WebApiUtilisation/"]
COPY ["WebApplicationJuaraujoda/Extensions/Extensions.csproj", "WebApplicationJuaraujoda/Extensions/"]
COPY ["WebApplicationJuaraujoda/Entities/Entities.csproj", "WebApplicationJuaraujoda/Entities/"]
COPY ["WebApplicationJuaraujoda/Tests/Tests.csproj", "WebApplicationJuaraujoda/Tests/"]
COPY WebApplicationJuaraujoda/WebApplicationJuaraujoda.sln .
RUN dotnet restore "WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj"
COPY . .
WORKDIR "/src/WebApplicationJuaraujoda/WebApplicationJuaraujoda"
RUN dotnet build "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WtaApi.dll"]