# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the project file and restore dependencies *first*
COPY WebApplicationJuaraujoda/WtaApi.csproj WebApplicationJuaraujoda/
COPY *.sln ./  # Copy the solution file from the repository root
RUN dotnet restore WebApplicationJuaraujoda/WtaApi.csproj

# Copy the entire source code
COPY WebApplicationJuaraujoda/ WebApplicationJuaraujoda/

# Build the application in Release mode
WORKDIR /src/WebApplicationJuaraujoda
RUN dotnet build WtaApi.csproj -c Release --no-restore

# Publish the application
FROM build AS publish
RUN dotnet publish WtaApi.csproj -c Release --no-restore -o /app/publish

# Use the ASP.NET Core runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Set the working directory
WORKDIR /app

# Copy the published output from the publish stage
COPY --from=publish /app/publish .

# Expose port 8080
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Define the entry point to run the application
ENTRYPOINT ["dotnet", "WtaApi.dll"]