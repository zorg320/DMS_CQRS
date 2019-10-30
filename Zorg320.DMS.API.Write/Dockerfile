FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Zorg320.DMS.API.Write/Zorg320.DMS.API.Write.csproj", "Zorg320.DMS.API.Write/"]
RUN dotnet restore "Zorg320.DMS.API.Write/Zorg320.DMS.API.Write.csproj"
COPY . .
WORKDIR "/src/Zorg320.DMS.API.Write"
RUN dotnet build "Zorg320.DMS.API.Write.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Zorg320.DMS.API.Write.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Zorg320.DMS.API.Write.dll"]