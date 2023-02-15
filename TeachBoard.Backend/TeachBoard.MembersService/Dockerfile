FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeachBoard.MembersService.WebApi/TeachBoard.MembersService.WebApi.csproj", "TeachBoard.MembersService.WebApi/"]
RUN dotnet restore "TeachBoard.MembersService.WebApi/TeachBoard.MembersService.WebApi.csproj"
COPY . .
WORKDIR "/src/TeachBoard.MembersService.WebApi"
RUN dotnet build "TeachBoard.MembersService.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeachBoard.MembersService.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeachBoard.MembersService.WebApi.dll"]
