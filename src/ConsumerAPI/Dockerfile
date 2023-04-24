FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["ConsumerAPI.csproj", "ConsumerAPI/"]
WORKDIR "ConsumerAPI/"
RUN dotnet restore "ConsumerAPI.csproj"
COPY . .
RUN dotnet build "ConsumerAPI.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "ConsumerAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsumerAPI.dll"]
