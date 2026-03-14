FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8090
EXPOSE 8091

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eCommerce.API/eCommerce.API.csproj", "eCommerce.API/"]
COPY ["eCommerce.Core/eCommerce.Core.csproj", "eCommerce.Core/"]
COPY ["eCommerce.Infrastructure/eCommerce.Infrastructure.csproj", "eCommerce.Infrastructure/"]
RUN dotnet restore "eCommerce.API/eCommerce.API.csproj"
COPY . .
WORKDIR "/src/eCommerce.API"
RUN dotnet build "./eCommerce.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./eCommerce.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV POSTGRES_HOST=localhost
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=admin

ENTRYPOINT ["dotnet", "eCommerce.API.dll"]
