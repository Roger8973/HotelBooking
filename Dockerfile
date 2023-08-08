#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BookingService/Consumers/API/API.csproj", "BookingService/Consumers/API/"]
COPY ["BookingService/Adapters/Data/Data.csproj", "BookingService/Adapters/Data/"]
COPY ["BookingService/Core/Domain/Domain.csproj", "BookingService/Core/Domain/"]
COPY ["BookingService/Core/Application/Application.csproj", "BookingService/Core/Application/"]
RUN dotnet restore "BookingService/Consumers/API/API.csproj"
COPY . .
WORKDIR "/src/BookingService/Consumers/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]