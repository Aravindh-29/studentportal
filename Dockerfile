# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore

# Fix 2 — update Microsoft.Build to patched version (CVE-2025-55247)
RUN dotnet add package Microsoft.Build --version 17.10.46

RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

# Fix 1 — patch libgnutls30 from 3.7.9-2+deb12u6 → 3.7.9-2+deb12u7
# Kills: CVE-2026-33845, CVE-2026-42010 (CRITICAL) + CVE-2026-33846, CVE-2026-3833, CVE-2026-42009 (HIGH)
RUN apt-get update \
    && apt-get upgrade -y \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "StudentPortal.dll"]