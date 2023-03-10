FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeachBoard.EducationService.Domain/TeachBoard.EducationService.Domain.csproj", "TeachBoard.EducationService.Domain/TeachBoard.EducationService.Domain.csproj"]
COPY ["TeachBoard.EducationService.Application/TeachBoard.EducationService.Application.csproj", "TeachBoard.EducationService.Application/TeachBoard.EducationService.Application.csproj"]
COPY ["TeachBoard.EducationService.Persistence/TeachBoard.EducationService.Persistence.csproj", "TeachBoard.EducationService.Persistence/TeachBoard.EducationService.Persistence.csproj"]
COPY ["TeachBoard.EducationService.WebApi/TeachBoard.EducationService.WebApi.csproj", "TeachBoard.EducationService.WebApi/TeachBoard.EducationService.WebApi.csproj"]

COPY . .
WORKDIR "/src/TeachBoard.EducationService.WebApi"
RUN dotnet build "TeachBoard.EducationService.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeachBoard.EducationService.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeachBoard.EducationService.WebApi.dll"]