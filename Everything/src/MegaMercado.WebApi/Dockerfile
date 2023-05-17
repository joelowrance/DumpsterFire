FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MegaMercado.WebApi/MegaMercado.WebApi.csproj", "MegaMercado.WebApi/"]
RUN dotnet restore "MegaMercado.WebApi/MegaMercado.WebApi.csproj"
COPY . .
WORKDIR "/src/MegaMercado.WebApi"
RUN dotnet build "MegaMercado.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MegaMercado.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MegaMercado.WebApi.dll"]
