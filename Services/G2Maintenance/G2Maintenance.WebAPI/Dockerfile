FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/G2Maintenance/G2Maintenance.WebAPI/G2Maintenance.WebAPI.csproj", "Services/G2Maintenance/G2Maintenance.WebAPI/"]
COPY ["Services/G2Maintenance/G2Maintenance.Application/G2Maintenance.Application.csproj", "Services/G2Maintenance/G2Maintenance.Application/"]
COPY ["Services/G2Maintenance/G2Maintenance.Domain/G2Maintenance.Domain.csproj", "Services/G2Maintenance/G2Maintenance.Domain/"]
COPY ["Services/G2Maintenance/G2Maintenance.Infrastructure/G2Maintenance.Infrastructure.csproj", "Services/G2Maintenance/G2Maintenance.Infrastructure/"]
COPY ["Shared/G2_Shared_Infrastructure/G2_Shared_Infrastructure.csproj", "Shared/G2_Shared_Infrastructure/"]
RUN dotnet restore "Services/G2Maintenance/G2Maintenance.WebAPI/G2Maintenance.WebAPI.csproj"
COPY . .
WORKDIR "/src/Services/G2Maintenance/G2Maintenance.WebAPI"
RUN dotnet build "./G2Maintenance.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./G2Maintenance.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "G2Maintenance.WebAPI.dll"]
