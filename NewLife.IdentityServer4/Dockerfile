FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-stretch AS build
WORKDIR /src
COPY ["NewLife.IdentityServer4/NewLife.IdentityServer4.csproj", "NewLife.IdentityServer4/"]
COPY ["NewLife.IdentityServer4/Nuget.config", "NewLife.IdentityServer4/"]
COPY ["IdentityServer4.XCode/IdentityServer4.XCode.csproj", "IdentityServer4.XCode/"]
RUN dotnet restore "NewLife.IdentityServer4/NewLife.IdentityServer4.csproj" --configfile "NewLife.IdentityServer4/Nuget.config"
COPY . .
WORKDIR "/src/NewLife.IdentityServer4"
RUN dotnet build "NewLife.IdentityServer4.csproj" -c Release -o /app  --no-restore

FROM build AS publish
RUN dotnet publish "NewLife.IdentityServer4.csproj" -c Release -o /app  --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NewLife.IdentityServer4.dll"]
