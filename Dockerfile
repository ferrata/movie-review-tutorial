FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GabrielesProject.MovieReviewSystem.WebApi/GabrielesProject.MovieReviewSystem.WebApi.csproj", "src/GabrielesProject.MovieReviewSystem.WebApi/"]
COPY ["src/GabrielesProject.MovieReviewSystem.Infrastracture/GabrielesProject.MovieReviewSystem.Infrastracture.csproj", "src/GabrielesProject.MovieReviewSystem.Infrastracture/"]
COPY ["src/GabrielesProject.MovieReviewSystem.Application/GabrielesProject.MovieReviewSystem.Application.csproj", "src/GabrielesProject.MovieReviewSystem.Application/"]
COPY ["src/GabrielesProject.MovieReviewSystem.Domain/GabrielesProject.MovieReviewSystem.Domain.csproj", "src/GabrielesProject.MovieReviewSystem.Domain/"]
RUN dotnet restore "src/GabrielesProject.MovieReviewSystem.WebApi/GabrielesProject.MovieReviewSystem.WebApi.csproj"
COPY . .
WORKDIR "/src/src/GabrielesProject.MovieReviewSystem.WebApi"
RUN dotnet build "GabrielesProject.MovieReviewSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GabrielesProject.MovieReviewSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GabrielesProject.MovieReviewSystem.WebApi.dll"]
