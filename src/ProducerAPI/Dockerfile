FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["ProducerAPI.csproj", "ProducerAPI/"]
WORKDIR "ProducerAPI/"
RUN dotnet restore "ProducerAPI.csproj"
COPY . .
RUN dotnet build "ProducerAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProducerAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProducerAPI.dll"]
