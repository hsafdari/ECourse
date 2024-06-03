#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Test/ECourse.Services.CourseAPI.Tests/ECourse.Services.CourseAPI.Tests.csproj", "Test/ECourse.Services.CourseAPI.Tests/"]
RUN dotnet restore "./Test/ECourse.Services.CourseAPI.Tests/ECourse.Services.CourseAPI.Tests.csproj"
COPY . .
WORKDIR "/src/Test/ECourse.Services.CourseAPI.Tests"
RUN dotnet build "./ECourse.Services.CourseAPI.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ECourse.Services.CourseAPI.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECourse.Services.CourseAPI.Tests.dll"]