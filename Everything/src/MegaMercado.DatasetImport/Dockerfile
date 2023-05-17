FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MegaMercado.DatasetImport/MegaMercado.DatasetImport.csproj", "MegaMercado.DatasetImport/"]
RUN dotnet restore "MegaMercado.DatasetImport/MegaMercado.DatasetImport.csproj"
COPY . .
WORKDIR "/src/MegaMercado.DatasetImport"
RUN dotnet build "MegaMercado.DatasetImport.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MegaMercado.DatasetImport.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MegaMercado.DatasetImport.dll"]
