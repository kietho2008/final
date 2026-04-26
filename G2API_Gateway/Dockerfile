FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CarRental.sln", "."]
COPY ["G2API_Gateway/G2API_Gateway.csproj", "G2API_Gateway/"]

RUN dotnet restore "G2API_Gateway/G2API_Gateway.csproj"

COPY . .
WORKDIR "/src/G2API_Gateway"
RUN dotnet publish "G2API_Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "G2API_Gateway.dll"]