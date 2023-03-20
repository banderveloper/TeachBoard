FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ARG POSTGRES_DB_USERNAME
ARG POSTGRES_DB_PASSWORD

ENV Database__User=$POSTGRES_DB_USERNAME
ENV Database__Password=$POSTGRES_DB_PASSWORD


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeachBoard.IdentityService.Domain/TeachBoard.IdentityService.Domain.csproj", "TeachBoard.IdentityService.Domain/TeachBoard.IdentityService.Domain.csproj"]
COPY ["TeachBoard.IdentityService.Application/TeachBoard.IdentityService.Application.csproj", "TeachBoard.IdentityService.Application/TeachBoard.IdentityService.Application.csproj"]
COPY ["TeachBoard.IdentityService.Persistence/TeachBoard.IdentityService.Persistence.csproj", "TeachBoard.IdentityService.Persistence/TeachBoard.IdentityService.Persistence.csproj"]
COPY ["TeachBoard.IdentityService.WebApi/TeachBoard.IdentityService.WebApi.csproj", "TeachBoard.IdentityService.WebApi/TeachBoard.IdentityService.WebApi.csproj"]

COPY . .
WORKDIR "/src/TeachBoard.IdentityService.WebApi"
RUN dotnet build "TeachBoard.IdentityService.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeachBoard.IdentityService.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeachBoard.IdentityService.WebApi.dll"]