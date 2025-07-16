# Build stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Web/UserWebservice.Web.csproj", "src/Web/"]
COPY ["src/Application/UserWebservice.Application.csproj", "src/Application/"]
COPY ["src/Domain/UserWebservice.Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/UserWebservice.Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/Web/UserWebservice.Web.csproj"
COPY . .
RUN dotnet publish "src/Web/UserWebservice.Web.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bison.UserWebservice.Web.dll"]