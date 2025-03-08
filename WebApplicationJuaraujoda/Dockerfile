# -------------------------------
# Stage 1: Build & Publish the API
# -------------------------------
    FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
    ARG configuration=Release
    WORKDIR /src
    
    # Copy only the project files first (to leverage Docker layer caching)
    COPY ["WebApplicationJuaraujoda/WebApplicationJuaraujoda/WtaApi.csproj", "WebApplicationJuaraujoda/"]
    
    # Copy the rest of the source code
    COPY . .
    
    # Change directory to the API project folder
    WORKDIR "/src/WebApplicationJuaraujoda/WebApplicationJuaraujoda"
    
    # Restore NuGet packages, build the project, and publish it
    RUN dotnet restore "WtaApi.csproj" \
        && dotnet build "WtaApi.csproj" -c $configuration --no-restore -o /app/build \
        && dotnet publish "WtaApi.csproj" -c $configuration --no-restore -o /app/publish /p:UseAppHost=false
    
    # -------------------------------
    # Stage 2: Create the final runtime image
    # -------------------------------
    FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
    WORKDIR /app
    
    # Copy the published files from the build stage
    COPY --from=build /app/publish .
    
    # Set environment variables
    ENV ASPNETCORE_HTTP_PORTS=80
    ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
    
    # Expose the port your API will listen on
    EXPOSE 80
    
    # Define the entrypoint that runs your API
    ENTRYPOINT ["dotnet", "WtaApi.dll"]
    