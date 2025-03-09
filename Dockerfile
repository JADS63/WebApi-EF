FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PartieAPI/WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj", "PartieAPI/WebApplicationJuaraujoda/WebApplicationJuaraujoda/"]
COPY ["PartieAPI/WebApplicationJuaraujoda/Dto/Dto.csproj", "PartieAPI/WebApplicationJuaraujoda/Dto/"]
COPY ["PartieAPI/WebApplicationJuaraujoda/Services/Shared.csproj", "PartieAPI/WebApplicationJuaraujoda/Services/"]
COPY ["PartieAPI/WebApplicationJuaraujoda/Stub/StubbedDtoLib.csproj", "PartieAPI/WebApplicationJuaraujoda/Stub/"]
COPY ["PartieAPI/WebApplicationJuaraujoda/WebApiUtilisation/WebApiUtilisation.csproj", "PartieAPI/WebApplicationJuaraujoda/WebApiUtilisation/"]
COPY ["PartieAPI/WebApplicationJuaraujoda/Extensions/Extensions.csproj", "PartieAPI/WebApplicationJuaraujoda/Extensions/"]
COPY ["PartieAPI/WebApplicationJuaraujoda/Entities/Entities.csproj", "PartieAPI/WebApplicationJuaraujoda/Entities/"]
COPY ["PartieAPI/WebApplicationJuaraujoda/Tests/Tests.csproj", "PartieAPI/WebApplicationJuaraujoda/Tests/"]
COPY PartieAPI/WebApplicationJuaraujoda/WebApplicationJuaraujoda.sln .
RUN dotnet restore "PartieAPI/WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj"
COPY . .
WORKDIR "/src/PartieAPI/WebApplicationJuaraujoda/WebApplicationJuaraujoda"
RUN dotnet build "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WtaApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENV ASPNETCORE_HTTP_PORTS=80
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENTRYPOINT ["dotnet", "WtaApi.dll"]