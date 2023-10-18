FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ShBarcelona.API/ShBarcelona.API.csproj", "ShBarcelona.API/"]
RUN dotnet restore "ShBarcelona.API/ShBarcelona.API.csproj"
COPY . .
WORKDIR "/src/ShBarcelona.API"
RUN dotnet build "ShBarcelona.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShBarcelona.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShBarcelona.API.dll"]