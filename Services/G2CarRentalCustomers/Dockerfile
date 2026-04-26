FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CarRental.sln", "."]
COPY ["Services/G2CarRentalCustomers/G2CarRentalCustomers.csproj", "Services/G2CarRentalCustomers/"]
COPY ["Shared/G2_Shared_Infrastructure/G2_Shared_Infrastructure.csproj", "Shared/G2_Shared_Infrastructure/"]

RUN dotnet restore "Services/G2CarRentalCustomers/G2CarRentalCustomers.csproj"

COPY . .
WORKDIR "/src/Services/G2CarRentalCustomers"
RUN dotnet publish "G2CarRentalCustomers.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "G2CarRentalCustomers.dll"]