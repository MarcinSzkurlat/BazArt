FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BazArtAPI/BazArtAPI.csproj", "BazArtAPI/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "BazArtAPI/BazArtAPI.csproj"
COPY . .
WORKDIR "/src/BazArtAPI"
RUN dotnet build "BazArtAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BazArtAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BazArtAPI.dll"]