FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApplicationJuaraujoda/WtaApi.csproj", "WebApplicationJuaraujoda/"]
COPY ["Dto/Dto.csproj", "Dto/"]
COPY ["Services/Shared.csproj", "Services/"]
COPY ["Stub/StubbedDtoLib.csproj", "Stub/"]
COPY ["WebApiUtilisation/WebApiUtilisation.csproj", "WebApiUtilisation/"]
COPY ["Extensions/Extensions.csproj", "Extensions/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Tests/Tests.csproj", "Tests/"]
COPY WebApplicationJuaraujoda.sln .
RUN dotnet restore "WebApplicationJuaraujoda/WtaApi.csproj"
COPY . .
WORKDIR "/src/WebApplicationJuaraujoda"
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