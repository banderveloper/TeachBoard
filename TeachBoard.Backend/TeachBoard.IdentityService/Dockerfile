FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeachBoard.WebApi/TeachBoard.WebApi.csproj", "TeachBoard.WebApi/"]
RUN dotnet restore "TeachBoard.WebApi/TeachBoard.WebApi.csproj"
COPY . .
WORKDIR "/src/TeachBoard.WebApi"
RUN dotnet build "TeachBoard.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeachBoard.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeachBoard.WebApi.dll"]
