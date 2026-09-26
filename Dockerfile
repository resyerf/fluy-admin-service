# Build context must be the repo root (C:\code\FLUY), because this service
# references ../../../shared/Fluy.SharedKernel, which lives outside this folder.
# Build from the repo root with:
#   docker build -f fluy-admin-service/Dockerfile -t resyerf/fluy-admin-service:v1 .

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY fluy-admin-service/src/FluyAdmin.Api/FluyAdmin.Api.csproj fluy-admin-service/src/FluyAdmin.Api/
COPY fluy-admin-service/src/FluyAdmin.Application/FluyAdmin.Application.csproj fluy-admin-service/src/FluyAdmin.Application/
COPY fluy-admin-service/src/FluyAdmin.Domain/FluyAdmin.Domain.csproj fluy-admin-service/src/FluyAdmin.Domain/
COPY fluy-admin-service/src/FluyAdmin.Infrastructure/FluyAdmin.Infrastructure.csproj fluy-admin-service/src/FluyAdmin.Infrastructure/
COPY shared/Fluy.SharedKernel/Fluy.SharedKernel.csproj shared/Fluy.SharedKernel/

RUN dotnet restore fluy-admin-service/src/FluyAdmin.Api/FluyAdmin.Api.csproj

COPY fluy-admin-service/src/ fluy-admin-service/src/
COPY shared/ shared/

RUN dotnet publish fluy-admin-service/src/FluyAdmin.Api/FluyAdmin.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "FluyAdmin.Api.dll"]
