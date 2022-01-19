#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Demo/TomTom.Useful.Demo.WebApi/TomTom.Useful.Demo.WebApi.csproj", "Demo/TomTom.Useful.Demo.WebApi/"]
RUN dotnet restore "Demo/TomTom.Useful.Demo.WebApi/TomTom.Useful.Demo.WebApi.csproj"
COPY . .
WORKDIR "/src/Demo/TomTom.Useful.Demo.WebApi"
RUN dotnet build "TomTom.Useful.Demo.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TomTom.Useful.Demo.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TomTom.Useful.Demo.WebApi.dll"]