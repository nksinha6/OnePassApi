# ===============================
# Build Stage
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore deps
COPY ["OnePassAPI/OnePass.API.csproj", "OnePassAPI/"]
COPY ["OnePass.Domain.Services/OnePass.Domain.Services.csproj", "OnePass.Domain.Services/"]
COPY ["OnePass.Domain/OnePass.Domain.csproj", "OnePass.Domain/"]
COPY ["OnePass.Dto/OnePass.Dto.csproj", "OnePass.Dto/"]
COPY ["OnePass.Infrastructure/OnePass.Infrastructure.csproj", "OnePass.Infrastructure/"]
RUN dotnet restore "OnePassAPI/OnePass.API.csproj"

# Copy all and publish
COPY . .
WORKDIR "/src/OnePassAPI"
RUN dotnet publish "OnePass.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===============================
# Runtime Stage
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 5000 (App Platform maps it automatically to 80/443)
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "OnePass.API.dll"]
