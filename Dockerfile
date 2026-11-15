# Build stage: restores and publishes the Web API in Release mode
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY DevTech/DevTech.csproj DevTech/
RUN dotnet restore DevTech/DevTech.csproj
COPY . .
RUN dotnet publish DevTech/DevTech.csproj -c Release -o /app/publish --no-restore

# Runtime stage: only the ASP.NET Core runtime and the published files
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
# Render provides the port in $PORT (10000 by default)
ENV PORT=10000
EXPOSE 10000
CMD ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT} dotnet DevTech.dll"]
