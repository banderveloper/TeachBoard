FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

ARG CLOUDINARY_SECRET
ARG CLOUDINARY_KEY
ARG CLOUDINARY_CLOUDNAME

ENV Cloudinary__Secret=$CLOUDINARY_SECRET
ENV Cloudinary__Key=$CLOUDINARY_KEY
ENV Cloudinary__CloudName=$CLOUDINARY_CLOUDNAME

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeachBoard.FileService.csproj", "./"]
RUN dotnet restore "TeachBoard.FileService.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TeachBoard.FileService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeachBoard.FileService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeachBoard.FileService.dll"]
