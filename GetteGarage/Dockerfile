# BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the Solution and Project files 
COPY ["GetteGarage.sln", "./"]
COPY ["GetteGarage/GetteGarage.csproj", "GetteGarage/"]
COPY ["GetteGarage.Client/GetteGarage.Client.csproj", "GetteGarage.Client/"]

# Restore dependencies
RUN dotnet restore "GetteGarage/GetteGarage.csproj"

# Copy the rest of the source code
COPY . .

# Build and Publish the app in Release mode
WORKDIR "/src/GetteGarage"
RUN dotnet publish "GetteGarage.csproj" -c Release -o /app/publish /p:UseAppHost=false

# RUNTIME STAGE
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy the compiled files
COPY --from=build /app/publish .

# Set the environment variable so .NET knows it's running in production
ENV ASPNETCORE_ENVIRONMENT=Production

# Start the application
ENTRYPOINT ["dotnet", "GetteGarage.dll"]