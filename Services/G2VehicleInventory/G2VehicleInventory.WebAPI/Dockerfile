FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CarRental.sln", "."]
COPY ["Services/G2VehicleInventory/G2VehicleInventory.WebAPI/G2VehicleInventory.WebAPI.csproj", "Services/G2VehicleInventory/G2VehicleInventory.WebAPI/"]
COPY ["Services/G2VehicleInventory/G2VehicleInventory.Application/G2VehicleInventory.Application.csproj", "Services/G2VehicleInventory/G2VehicleInventory.Application/"]
COPY ["Services/G2VehicleInventory/G2VehicleInventory.Domain/G2VehicleInventory.Domain.csproj", "Services/G2VehicleInventory/G2VehicleInventory.Domain/"]
COPY ["Services/G2VehicleInventory/G2VehicleInventory.Infrastructure/G2VehicleInventory.Infrastructure.csproj", "Services/G2VehicleInventory/G2VehicleInventory.Infrastructure/"]
COPY ["Shared/G2_Shared_Infrastructure/G2_Shared_Infrastructure.csproj", "Shared/G2_Shared_Infrastructure/"]

RUN dotnet restore "Services/G2VehicleInventory/G2VehicleInventory.WebAPI/G2VehicleInventory.WebAPI.csproj"

COPY . .
WORKDIR "/src/Services/G2VehicleInventory/G2VehicleInventory.WebAPI"
RUN dotnet publish "G2VehicleInventory.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "G2VehicleInventory.WebAPI.dll"]