FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeachBoard.Gateway.WebApi/TeachBoard.Gateway.WebApi.csproj", "TeachBoard.Gateway.WebApi/"]
RUN dotnet restore "TeachBoard.Gateway.WebApi/TeachBoard.Gateway.WebApi.csproj"
COPY . .
WORKDIR "/src/TeachBoard.Gateway.WebApi"
RUN dotnet build "TeachBoard.Gateway.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeachBoard.Gateway.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeachBoard.Gateway.WebApi.dll"]
