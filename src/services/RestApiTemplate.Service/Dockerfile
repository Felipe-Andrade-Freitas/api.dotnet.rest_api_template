FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/ProductService/ProductService.csproj", "src/services/ProductService/"]
COPY ["src/core/Infrastructure/Infrastructure.csproj", "src/core/Infrastructure/"]
COPY ["src/core/Application/Application.csproj", "src/core/Application/"]
COPY ["src/core/Domain/Domain.csproj", "src/core/Domain/"]
COPY ["src/core/Shared/Shared.csproj", "src/core/Shared/"]
RUN dotnet restore "src/services/ProductService/ProductService.csproj"
COPY . .
WORKDIR "/src/src/services/ProductService"
RUN dotnet build "ProductService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]