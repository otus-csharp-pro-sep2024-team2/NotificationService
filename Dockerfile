# Stage 1: Base the image for executing the applicationя
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: The image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
# Copying project files (assuming that's all .the csproj files are in the appropriate folders)
COPY ["NotificationService.API/NotificationService.API.csproj", "NotificationService.API/"]
COPY ["NotificationService.Application/NotificationService.Application.csproj", "NotificationService.Application/"]
COPY ["NotificationService.Domain/NotificationService.Domain.csproj", "NotificationService.Domain/"]
COPY ["NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "NotificationService.Infrastructure/"]
RUN dotnet restore "NotificationService.API/NotificationService.API.csproj"
COPY . .
WORKDIR "NotificationService.API"
RUN dotnet build "NotificationService.API.csproj" -c Release -o /app/build

# Stage 3: Publishing an application
FROM build AS publish
RUN dotnet publish "NotificationService.API.csproj" -c Release -o /app/publish

# Stage 4: The final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
