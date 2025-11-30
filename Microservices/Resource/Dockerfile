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
COPY ["Resource.sln", "."]

# Copy project dependencies first (csproj files only)
COPY ["Api/*.csproj", "Api/"]
COPY ["Common/*.csproj", "Common/"]
COPY ["Domain/*.csproj", "Domain/"]
COPY ["Persistence/*.csproj", "Persistence/"]
COPY ["Application/*.csproj", "Application/"]
COPY ["UnitTest/*.csproj", "UnitTest/"]
COPY ["InversionOfControl/*.csproj", "InversionOfControl/"]

# Restore NuGet packages
RUN dotnet restore "Resource.sln"

# Copy full source code
COPY Application/ Application/
COPY Common/ Common/
COPY Domain/ Domain/
COPY Persistence/ Persistence/
COPY Api/ Api/
COPY InversionOfControl/ InversionOfControl/
COPY UnitTest/ UnitTest/

## Install NSwag globally
#RUN dotnet tool install -g NSwag.ConsoleCore
#
## Add dotnet tools to PATH
#ENV PATH="$PATH:/root/.dotnet/tools"

# Build the project
RUN dotnet build "Resource.sln" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Resource.sln" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]