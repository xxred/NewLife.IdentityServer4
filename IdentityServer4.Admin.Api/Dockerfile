FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["IdentityServer4.Admin.Api/IdentityServer4.Admin.Api.csproj", "IdentityServer4.Admin.Api/"]
RUN dotnet restore "IdentityServer4.Admin.Api/IdentityServer4.Admin.Api.csproj"
COPY . .
WORKDIR "/src/IdentityServer4.Admin.Api"
RUN dotnet build "IdentityServer4.Admin.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IdentityServer4.Admin.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IdentityServer4.Admin.Api.dll"]