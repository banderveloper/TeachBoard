FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeachBoard.FileService.Api/TeachBoard.FileService.Api.csproj", "TeachBoard.FileService.Api/"]
RUN dotnet restore "TeachBoard.FileService.Api/TeachBoard.FileService.Api.csproj"
COPY . .
WORKDIR "/src/TeachBoard.FileService.Api"
RUN dotnet build "TeachBoard.FileService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeachBoard.FileService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeachBoard.FileService.Api.dll"]
