FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LocalizationPreview.API/LocalizationPreview.API.csproj", "LocalizationPreview.API/"]
COPY ["LocalizationPreview.Infrastructure/LocalizationPreview.Infrastructure.csproj", "LocalizationPreview.Infrastructure/"]
COPY ["LocalizationPreview.Core/LocalizationPreview.Core.csproj", "LocalizationPreview.Core/"]
COPY ["LocalizationPreview.Shared/LocalizationPreview.Shared.csproj", "LocalizationPreview.Shared/"]
RUN dotnet restore "LocalizationPreview.API/LocalizationPreview.API.csproj"
COPY . .
WORKDIR "/src/LocalizationPreview.API"
RUN dotnet build "LocalizationPreview.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LocalizationPreview.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocalizationPreview.API.dll"]
