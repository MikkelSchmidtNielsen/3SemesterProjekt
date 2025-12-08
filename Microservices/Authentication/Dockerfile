# See https://aka.ms/customizecontainer to learn how to customize your debug container.

# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution file
COPY ["Authentication.sln", "."]

# Copy project dependencies first (csproj files only)
COPY ["Authentication.Api/*.csproj", "Authentication.Api/"]
COPY ["Common/*.csproj", "Common/"]
COPY ["Domain/*.csproj", "Domain/"]
COPY ["Persistence/*.csproj", "Persistence/"]
COPY ["Application/*.csproj", "Application/"]
COPY ["Infrastructure/*.csproj", "Infrastructure/"]
COPY ["UnitTest/*.csproj", "UnitTest/"]
COPY ["InversionOfControlContainers/*.csproj", "InversionOfControlContainers/"]

# Restore NuGet packages
RUN dotnet restore "Authentication.sln"

# Copy full source code
COPY Application/ Application/
COPY Common/ Common/
COPY Domain/ Domain/
COPY Persistence/ Persistence/
COPY Authentication.Api/ Authentication.Api/
COPY Infrastructure/ Infrastructure/
COPY InversionOfControlContainers/ InversionOfControlContainers/

# Install NSwag globally
RUN dotnet tool install -g NSwag.ConsoleCore

# Add dotnet tools to PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Build the project
RUN dotnet build "Authentication.Api/Authentication.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Authentication.Api/Authentication.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.Api.dll"]