FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "Arslan.Vms.Migration.sln" "Arslan.Vms.Migration.sln"

COPY "_migration/src/Arslan.Vms.DbIntegration/Arslan.Vms.DbIntegration.csproj" "_migration/src/Arslan.Vms.DbIntegration/Arslan.Vms.DbIntegration.csproj"

# Microservices Administration Service
COPY "services/administration/src/Arslan.Vms.AdministrationService.Domain/Arslan.Vms.AdministrationService.Domain.csproj" "services/administration/src/Arslan.Vms.AdministrationService.Domain/Arslan.Vms.AdministrationService.Domain.csproj"
COPY "services/administration/src/Arslan.Vms.AdministrationService.Domain.Shared/Arslan.Vms.AdministrationService.Domain.Shared.csproj" "services/administration/src/Arslan.Vms.AdministrationService.Domain.Shared/Arslan.Vms.AdministrationService.Domain.Shared.csproj"
COPY "services/administration/src/Arslan.Vms.AdministrationService.EntityFrameworkCore/Arslan.Vms.AdministrationService.EntityFrameworkCore.csproj" "services/administration/src/Arslan.Vms.AdministrationService.EntityFrameworkCore/Arslan.Vms.AdministrationService.EntityFrameworkCore.csproj"
# Microservices Identity Service
COPY "services/identity/src/Arslan.Vms.IdentityService.Domain/Arslan.Vms.IdentityService.Domain.csproj" "services/identity/src/Arslan.Vms.IdentityService.Domain/Arslan.Vms.IdentityService.Domain.csproj"
COPY "services/identity/src/Arslan.Vms.IdentityService.Domain.Shared/Arslan.Vms.IdentityService.Domain.Shared.csproj" "services/identity/src/Arslan.Vms.IdentityService.Domain.Shared/Arslan.Vms.IdentityService.Domain.Shared.csproj"
COPY "services/identity/src/Arslan.Vms.IdentityService.EntityFrameworkCore/Arslan.Vms.IdentityService.EntityFrameworkCore.csproj" "services/identity/src/Arslan.Vms.IdentityService.EntityFrameworkCore/Arslan.Vms.IdentityService.EntityFrameworkCore.csproj"
# Microservices Payment Service
COPY "services/payment/src/Arslan.Vms.PaymentService.Domain/Arslan.Vms.PaymentService.Domain.csproj" "services/payment/src/Arslan.Vms.PaymentService.Domain/Arslan.Vms.PaymentService.Domain.csproj"
COPY "services/payment/src/Arslan.Vms.PaymentService.Domain.Shared/Arslan.Vms.PaymentService.Domain.Shared.csproj" "services/payment/src/Arslan.Vms.PaymentService.Domain.Shared/Arslan.Vms.PaymentService.Domain.Shared.csproj"
COPY "services/payment/src/Arslan.Vms.PaymentService.EntityFrameworkCore/Arslan.Vms.PaymentService.EntityFrameworkCore.csproj" "services/payment/src/Arslan.Vms.PaymentService.EntityFrameworkCore/Arslan.Vms.PaymentService.EntityFrameworkCore.csproj"
# Gateways
COPY "gateways/web/src/Arslan.Vms.WebGateway/Arslan.Vms.WebGateway.csproj" "gateways/web/src/Arslan.Vms.WebGateway/Arslan.Vms.WebGateway.csproj"


COPY "NuGet.Config" "NuGet.Config"

RUN dotnet restore "Arslan.Vms.Migration.sln"
COPY . .

WORKDIR "/src/_migration/src/Arslan.Vms.DbIntegration"
RUN dotnet publish -c Release -o /app 
# RUN dotnet publish --no-restore -c Release -o /app 

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Arslan.Vms.DbIntegration.dll"]