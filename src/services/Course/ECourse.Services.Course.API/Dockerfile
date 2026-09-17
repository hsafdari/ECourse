# Runtime stage used by Visual Studio Fast Mode
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080


# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["src/services/Course/ECourse.Services.Course.Domain/ECourse.Services.Course.Domain.csproj", "src/services/Course/ECourse.Services.Course.Domain/"]
COPY ["src/services/Course/ECourse.Services.Course.Application/ECourse.Services.Course.Application.csproj", "src/services/Course/ECourse.Services.Course.Application/"]
COPY ["src/services/Course/ECourse.Services.Course.Infrastructure/ECourse.Services.Course.Infrastructure.csproj", "src/services/Course/ECourse.Services.Course.Infrastructure/"]
COPY ["src/services/Course/ECourse.Services.Course.API/ECourse.Services.Course.API.csproj", "src/services/Course/ECourse.Services.Course.API/"]

RUN dotnet restore "src/services/Course/ECourse.Services.Course.API/ECourse.Services.Course.API.csproj"

COPY . .

WORKDIR "/src/src/services/Course/ECourse.Services.Course.API"

RUN dotnet build "ECourse.Services.Course.API.csproj" --configuration $BUILD_CONFIGURATION --output /app/build --no-restore


# Publish stage
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "ECourse.Services.Course.API.csproj" --configuration $BUILD_CONFIGURATION --output /app/publish --no-restore /p:UseAppHost=false


# Final runtime stage
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ECourse.Services.Course.API.dll"]