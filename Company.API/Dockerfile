#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Company.API/Company.API.csproj", "Company.API/"]
RUN dotnet restore "Company.API/Company.API.csproj"
COPY . .
WORKDIR "/src/Company.API"
RUN dotnet build "Company.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Company.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.API.dll"]