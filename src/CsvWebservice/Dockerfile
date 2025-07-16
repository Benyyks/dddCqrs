# Build stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Web/CsvWebservice.Web.csproj", "src/Web/"]
COPY ["src/Application/CsvWebservice.Application.csproj", "src/Application/"]
COPY ["src/Domain/CsvWebservice.Domain.csproj", "src/Domain/"]
RUN dotnet restore "src/Web/CsvWebservice.Web.csproj"
COPY . .
RUN dotnet publish "src/Web/CsvWebservice.Web.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bison.CsvWebservice.Web.dll"]