#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WPPublisher.API/WPPublisher.API.csproj", "WPPublisher.API/"]
RUN dotnet restore "WPPublisher.API/WPPublisher.API.csproj"
COPY . .
WORKDIR "/src/WPPublisher.API"
RUN dotnet build "WPPublisher.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WPPublisher.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WPPublisher.API.dll"]