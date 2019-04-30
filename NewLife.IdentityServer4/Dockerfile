FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["NewLife.IdentityServer4/NewLife.IdentityServer4.csproj", "NewLife.IdentityServer4/"]
COPY ["IdentityServer4.XCode/IdentityServer4.XCode.csproj", "IdentityServer4.XCode/"]
RUN dotnet restore "NewLife.IdentityServer4/NewLife.IdentityServer4.csproj"
COPY . .
WORKDIR "/src/NewLife.IdentityServer4"
RUN dotnet build "NewLife.IdentityServer4.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NewLife.IdentityServer4.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NewLife.IdentityServer4.dll"]
