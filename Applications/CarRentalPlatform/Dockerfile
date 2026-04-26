FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CarRental.sln", "."]
COPY ["Applications/CarRentalPlatform/CarRentalPlatform.csproj", "Applications/CarRentalPlatform/"]
COPY ["Shared/G2_Shared_Infrastructure/G2_Shared_Infrastructure.csproj", "Shared/G2_Shared_Infrastructure/"]
COPY ["Services/G2VehicleInventory/G2VehicleInventory.Domain/G2VehicleInventory.Domain.csproj", "Services/G2VehicleInventory/G2VehicleInventory.Domain/"]

RUN dotnet restore "Applications/CarRentalPlatform/CarRentalPlatform.csproj"

COPY . .
WORKDIR "/src/Applications/CarRentalPlatform"
RUN dotnet publish "CarRentalPlatform.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "CarRentalPlatform.dll"]