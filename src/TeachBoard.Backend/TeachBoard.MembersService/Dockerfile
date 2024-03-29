﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ARG POSTGRES_DB_USERNAME
ARG POSTGRES_DB_PASSWORD

ENV Database__User=$POSTGRES_DB_USERNAME
ENV Database__Password=$POSTGRES_DB_PASSWORD

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeachBoard.MembersService.Domain/TeachBoard.MembersService.Domain.csproj", "TeachBoard.MembersService.Domain/TeachBoard.MembersService.Domain.csproj"]
COPY ["TeachBoard.MembersService.Application/TeachBoard.MembersService.Application.csproj", "TeachBoard.MembersService.Application/TeachBoard.MembersService.Application.csproj"]
COPY ["TeachBoard.MembersService.Persistence/TeachBoard.MembersService.Persistence.csproj", "TeachBoard.MembersService.Persistence/TeachBoard.MembersService.Persistence.csproj"]
COPY ["TeachBoard.MembersService.WebApi/TeachBoard.MembersService.WebApi.csproj", "TeachBoard.MembersService.WebApi/TeachBoard.MembersService.WebApi.csproj"]

COPY . .
WORKDIR "/src/TeachBoard.MembersService.WebApi"
RUN dotnet build "TeachBoard.MembersService.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeachBoard.MembersService.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeachBoard.MembersService.WebApi.dll"]