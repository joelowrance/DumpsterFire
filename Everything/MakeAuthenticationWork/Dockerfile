FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MakeAuthenticationWork/MakeAuthenticationWork.csproj", "MakeAuthenticationWork/"]
RUN dotnet restore "MakeAuthenticationWork/MakeAuthenticationWork.csproj"
COPY . .
WORKDIR "/src/MakeAuthenticationWork"
RUN dotnet build "MakeAuthenticationWork.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MakeAuthenticationWork.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MakeAuthenticationWork.dll"]
