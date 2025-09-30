FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tenisu.Api/Tenisu.Api.csproj", "Tenisu.Api/"]
COPY ["Tenisu.Application/Tenisu.Application.csproj", "Tenisu.Application/"]
COPY ["Tenisu.Common/Tenisu.Common.csproj", "Tenisu.Common/"]
COPY ["Tenisu.Domain/Tenisu.Domain.csproj", "Tenisu.Domain/"]
RUN dotnet restore "Tenisu.Api/Tenisu.Api.csproj"
COPY . .
WORKDIR "/src/Tenisu.Api"
RUN dotnet build "./Tenisu.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tenisu.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tenisu.Api.dll"]
