FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8001
ENV ASPNETCORE_URLS=http://+:8001

# Byg image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Tag release versionen (byg det som produktion)
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopier alle filer der er behov for
COPY 3SemesterProjekt.sln .

# Kopier afhængigheder
COPY Application/*.csproj Application/
COPY Common/*.csproj Common/
COPY Domain/*.csproj Domain/
COPY Infrastructure/*.csproj Infrastructure/
COPY Persistence/*.csproj Persistence/
COPY Presentation/Presentation.Client/*.csproj Presentation/Presentation.Client/
COPY Presentation/Presentation.Server/*.csproj Presentation/Presentation.Server/
COPY UnitTest/*.csproj UnitTest/

# Forbind projectreferencer
RUN dotnet restore "3SemesterProjekt.sln"

# Kopier kildekode
COPY Application/ Application/
COPY Common/ Common/
COPY Domain/ Domain/
COPY Infrastructure/ Infrastructure/
COPY Persistence/ Persistence/
COPY Presentation/Presentation.Client Presentation/Presentation.Client/
COPY Presentation/Presentation.Server Presentation/Presentation.Server/

# Byg projektet med tidligere sat konfiguration
RUN dotnet build "Presentation/Presentation.Server/Presentation.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Presentation/Presentation.Server/Presentation.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Run
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.Server.dll"]