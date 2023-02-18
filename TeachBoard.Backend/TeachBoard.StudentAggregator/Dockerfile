FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeachBoard.StudentAggregator.csproj", "./"]
RUN dotnet restore "TeachBoard.StudentAggregator.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TeachBoard.StudentAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeachBoard.StudentAggregator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeachBoard.StudentAggregator.dll"]
