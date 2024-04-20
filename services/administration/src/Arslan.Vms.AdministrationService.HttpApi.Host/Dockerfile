FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "Arslan.Vms.sln" "Arslan.Vms.sln"

COPY ["devexpress-packages", "devexpress-packages/"]
 
# Shared
COPY "shared/Arslan.Vms.Shared.Hosting/Arslan.Vms.Shared.Hosting.csproj" "shared/Arslan.Vms.Shared.Hosting/Arslan.Vms.Shared.Hosting.csproj"
COPY "shared/Arslan.Vms.Shared.Hosting.AspNetCore/Arslan.Vms.Shared.Hosting.AspNetCore.csproj" "shared/Arslan.Vms.Shared.Hosting.AspNetCore/Arslan.Vms.Shared.Hosting.AspNetCore.csproj"
COPY "shared/Arslan.Vms.Shared.Hosting.Gateways/Arslan.Vms.Shared.Hosting.Gateways.csproj" "shared/Arslan.Vms.Shared.Hosting.Gateways/Arslan.Vms.Shared.Hosting.Gateways.csproj"
COPY "shared/Arslan.Vms.Shared.Hosting.Microservices/Arslan.Vms.Shared.Hosting.Microservices.csproj" "shared/Arslan.Vms.Shared.Hosting.Microservices/Arslan.Vms.Shared.Hosting.Microservices.csproj"
COPY "shared/Arslan.Vms.Shared.Localization/Arslan.Vms.Shared.Localization.csproj" "shared/Arslan.Vms.Shared.Localization/Arslan.Vms.Shared.Localization.csproj"

# Microservices Administration Service
COPY "services/administration/src/Arslan.Vms.AdministrationService.HttpApi.Host/Arslan.Vms.AdministrationService.HttpApi.Host.csproj" "services/administration/src/Arslan.Vms.AdministrationService.HttpApi.Host/Arslan.Vms.AdministrationService.HttpApi.Host.csproj"
COPY "services/administration/src/Arslan.Vms.AdministrationService.HttpApi/Arslan.Vms.AdministrationService.HttpApi.csproj" "services/administration/src/Arslan.Vms.AdministrationService.HttpApi/Arslan.Vms.AdministrationService.HttpApi.csproj"
COPY "services/administration/src/Arslan.Vms.AdministrationService.HttpApi.Client/Arslan.Vms.AdministrationService.HttpApi.Client.csproj" "services/administration/src/Arslan.Vms.AdministrationService.HttpApi.Client/Arslan.Vms.AdministrationService.HttpApi.Client.csproj"
COPY "services/administration/src/Arslan.Vms.AdministrationService.Application/Arslan.Vms.AdministrationService.Application.csproj" "services/administration/src/Arslan.Vms.AdministrationService.Application/Arslan.Vms.AdministrationService.Application.csproj"
COPY "services/administration/src/Arslan.Vms.AdministrationService.Application.Contracts/Arslan.Vms.AdministrationService.Application.Contracts.csproj" "services/administration/src/Arslan.Vms.AdministrationService.Application.Contracts/Arslan.Vms.AdministrationService.Application.Contracts.csproj"
COPY "services/administration/src/Arslan.Vms.AdministrationService.Domain/Arslan.Vms.AdministrationService.Domain.csproj" "services/administration/src/Arslan.Vms.AdministrationService.Domain/Arslan.Vms.AdministrationService.Domain.csproj"
COPY "services/administration/src/Arslan.Vms.AdministrationService.Domain.Shared/Arslan.Vms.AdministrationService.Domain.Shared.csproj" "services/administration/src/Arslan.Vms.AdministrationService.Domain.Shared/Arslan.Vms.AdministrationService.Domain.Shared.csproj"
COPY "services/administration/src/Arslan.Vms.AdministrationService.EntityFrameworkCore/Arslan.Vms.AdministrationService.EntityFrameworkCore.csproj" "services/administration/src/Arslan.Vms.AdministrationService.EntityFrameworkCore/Arslan.Vms.AdministrationService.EntityFrameworkCore.csproj"
# Microservices Identity Service
COPY "services/identity/src/Arslan.Vms.IdentityService.HttpApi.Host/Arslan.Vms.IdentityService.HttpApi.Host.csproj" "services/identity/src/Arslan.Vms.IdentityService.HttpApi.Host/Arslan.Vms.IdentityService.HttpApi.Host.csproj"
COPY "services/identity/src/Arslan.Vms.IdentityService.HttpApi/Arslan.Vms.IdentityService.HttpApi.csproj" "services/identity/src/Arslan.Vms.IdentityService.HttpApi/Arslan.Vms.IdentityService.HttpApi.csproj"
COPY "services/identity/src/Arslan.Vms.IdentityService.HttpApi.Client/Arslan.Vms.IdentityService.HttpApi.Client.csproj" "services/identity/src/Arslan.Vms.IdentityService.HttpApi.Client/Arslan.Vms.IdentityService.HttpApi.Client.csproj"
COPY "services/identity/src/Arslan.Vms.IdentityService.Application/Arslan.Vms.IdentityService.Application.csproj" "services/identity/src/Arslan.Vms.IdentityService.Application/Arslan.Vms.IdentityService.Application.csproj"
COPY "services/identity/src/Arslan.Vms.IdentityService.Application.Contracts/Arslan.Vms.IdentityService.Application.Contracts.csproj" "services/identity/src/Arslan.Vms.IdentityService.Application.Contracts/Arslan.Vms.IdentityService.Application.Contracts.csproj"
COPY "services/identity/src/Arslan.Vms.IdentityService.Domain/Arslan.Vms.IdentityService.Domain.csproj" "services/identity/src/Arslan.Vms.IdentityService.Domain/Arslan.Vms.IdentityService.Domain.csproj"
COPY "services/identity/src/Arslan.Vms.IdentityService.Domain.Shared/Arslan.Vms.IdentityService.Domain.Shared.csproj" "services/identity/src/Arslan.Vms.IdentityService.Domain.Shared/Arslan.Vms.IdentityService.Domain.Shared.csproj"
COPY "services/identity/src/Arslan.Vms.IdentityService.EntityFrameworkCore/Arslan.Vms.IdentityService.EntityFrameworkCore.csproj" "services/identity/src/Arslan.Vms.IdentityService.EntityFrameworkCore/Arslan.Vms.IdentityService.EntityFrameworkCore.csproj"
# Microservices Vehicle Service
COPY "services/vehicle/src/Arslan.Vms.VehicleService.HttpApi.Host/Arslan.Vms.VehicleService.HttpApi.Host.csproj" "services/vehicle/src/Arslan.Vms.VehicleService.HttpApi.Host/Arslan.Vms.VehicleService.HttpApi.Host.csproj"
COPY "services/vehicle/src/Arslan.Vms.VehicleService.HttpApi/Arslan.Vms.VehicleService.HttpApi.csproj" "services/vehicle/src/Arslan.Vms.VehicleService.HttpApi/Arslan.Vms.VehicleService.HttpApi.csproj"
COPY "services/vehicle/src/Arslan.Vms.VehicleService.HttpApi.Client/Arslan.Vms.VehicleService.HttpApi.Client.csproj" "services/vehicle/src/Arslan.Vms.VehicleService.HttpApi.Client/Arslan.Vms.VehicleService.HttpApi.Client.csproj"
COPY "services/vehicle/src/Arslan.Vms.VehicleService.Application/Arslan.Vms.VehicleService.Application.csproj" "services/vehicle/src/Arslan.Vms.VehicleService.Application/Arslan.Vms.VehicleService.Application.csproj"
COPY "services/vehicle/src/Arslan.Vms.VehicleService.Application.Contracts/Arslan.Vms.VehicleService.Application.Contracts.csproj" "services/vehicle/src/Arslan.Vms.VehicleService.Application.Contracts/Arslan.Vms.VehicleService.Application.Contracts.csproj"
COPY "services/vehicle/src/Arslan.Vms.VehicleService.Domain/Arslan.Vms.VehicleService.Domain.csproj" "services/vehicle/src/Arslan.Vms.VehicleService.Domain/Arslan.Vms.VehicleService.Domain.csproj"
COPY "services/vehicle/src/Arslan.Vms.VehicleService.Domain.Shared/Arslan.Vms.VehicleService.Domain.Shared.csproj" "services/vehicle/src/Arslan.Vms.VehicleService.Domain.Shared/Arslan.Vms.VehicleService.Domain.Shared.csproj"
COPY "services/vehicle/src/Arslan.Vms.VehicleService.EntityFrameworkCore/Arslan.Vms.VehicleService.EntityFrameworkCore.csproj" "services/vehicle/src/Arslan.Vms.VehicleService.EntityFrameworkCore/Arslan.Vms.VehicleService.EntityFrameworkCore.csproj"
# Microservices Product Service
COPY "services/product/src/Arslan.Vms.ProductService.HttpApi.Host/Arslan.Vms.ProductService.HttpApi.Host.csproj" "services/product/src/Arslan.Vms.ProductService.HttpApi.Host/Arslan.Vms.ProductService.HttpApi.Host.csproj"
COPY "services/product/src/Arslan.Vms.ProductService.HttpApi/Arslan.Vms.ProductService.HttpApi.csproj" "services/product/src/Arslan.Vms.ProductService.HttpApi/Arslan.Vms.ProductService.HttpApi.csproj"
COPY "services/product/src/Arslan.Vms.ProductService.HttpApi.Client/Arslan.Vms.ProductService.HttpApi.Client.csproj" "services/product/src/Arslan.Vms.ProductService.HttpApi.Client/Arslan.Vms.ProductService.HttpApi.Client.csproj"
COPY "services/product/src/Arslan.Vms.ProductService.Application/Arslan.Vms.ProductService.Application.csproj" "services/product/src/Arslan.Vms.ProductService.Application/Arslan.Vms.ProductService.Application.csproj"
COPY "services/product/src/Arslan.Vms.ProductService.Application.Contracts/Arslan.Vms.ProductService.Application.Contracts.csproj" "services/product/src/Arslan.Vms.ProductService.Application.Contracts/Arslan.Vms.ProductService.Application.Contracts.csproj"
COPY "services/product/src/Arslan.Vms.ProductService.Domain/Arslan.Vms.ProductService.Domain.csproj" "services/product/src/Arslan.Vms.ProductService.Domain/Arslan.Vms.ProductService.Domain.csproj"
COPY "services/product/src/Arslan.Vms.ProductService.Domain.Shared/Arslan.Vms.ProductService.Domain.Shared.csproj" "services/product/src/Arslan.Vms.ProductService.Domain.Shared/Arslan.Vms.ProductService.Domain.Shared.csproj"
COPY "services/product/src/Arslan.Vms.ProductService.EntityFrameworkCore/Arslan.Vms.ProductService.EntityFrameworkCore.csproj" "services/product/src/Arslan.Vms.ProductService.EntityFrameworkCore/Arslan.Vms.ProductService.EntityFrameworkCore.csproj"
# Microservices Order Service
COPY "services/order/src/Arslan.Vms.OrderService.HttpApi.Host/Arslan.Vms.OrderService.HttpApi.Host.csproj" "services/order/src/Arslan.Vms.OrderService.HttpApi.Host/Arslan.Vms.OrderService.HttpApi.Host.csproj"
COPY "services/order/src/Arslan.Vms.OrderService.HttpApi/Arslan.Vms.OrderService.HttpApi.csproj" "services/order/src/Arslan.Vms.OrderService.HttpApi/Arslan.Vms.OrderService.HttpApi.csproj"
COPY "services/order/src/Arslan.Vms.OrderService.HttpApi.Client/Arslan.Vms.OrderService.HttpApi.Client.csproj" "services/order/src/Arslan.Vms.OrderService.HttpApi.Client/Arslan.Vms.OrderService.HttpApi.Client.csproj"
COPY "services/order/src/Arslan.Vms.OrderService.Application/Arslan.Vms.OrderService.Application.csproj" "services/order/src/Arslan.Vms.OrderService.Application/Arslan.Vms.OrderService.Application.csproj"
COPY "services/order/src/Arslan.Vms.OrderService.Application.Contracts/Arslan.Vms.OrderService.Application.Contracts.csproj" "services/order/src/Arslan.Vms.OrderService.Application.Contracts/Arslan.Vms.OrderService.Application.Contracts.csproj"
COPY "services/order/src/Arslan.Vms.OrderService.Domain/Arslan.Vms.OrderService.Domain.csproj" "services/order/src/Arslan.Vms.OrderService.Domain/Arslan.Vms.OrderService.Domain.csproj"
COPY "services/order/src/Arslan.Vms.OrderService.Domain.Shared/Arslan.Vms.OrderService.Domain.Shared.csproj" "services/order/src/Arslan.Vms.OrderService.Domain.Shared/Arslan.Vms.OrderService.Domain.Shared.csproj"
COPY "services/order/src/Arslan.Vms.OrderService.EntityFrameworkCore/Arslan.Vms.OrderService.EntityFrameworkCore.csproj" "services/order/src/Arslan.Vms.OrderService.EntityFrameworkCore/Arslan.Vms.OrderService.EntityFrameworkCore.csproj"
# Microservices Inventory Service
COPY "services/inventory/src/Arslan.Vms.InventoryService.HttpApi.Host/Arslan.Vms.InventoryService.HttpApi.Host.csproj" "services/inventory/src/Arslan.Vms.InventoryService.HttpApi.Host/Arslan.Vms.InventoryService.HttpApi.Host.csproj"
COPY "services/inventory/src/Arslan.Vms.InventoryService.HttpApi/Arslan.Vms.InventoryService.HttpApi.csproj" "services/inventory/src/Arslan.Vms.InventoryService.HttpApi/Arslan.Vms.InventoryService.HttpApi.csproj"
COPY "services/inventory/src/Arslan.Vms.InventoryService.HttpApi.Client/Arslan.Vms.InventoryService.HttpApi.Client.csproj" "services/inventory/src/Arslan.Vms.InventoryService.HttpApi.Client/Arslan.Vms.InventoryService.HttpApi.Client.csproj"
COPY "services/inventory/src/Arslan.Vms.InventoryService.Application/Arslan.Vms.InventoryService.Application.csproj" "services/inventory/src/Arslan.Vms.InventoryService.Application/Arslan.Vms.InventoryService.Application.csproj"
COPY "services/inventory/src/Arslan.Vms.InventoryService.Application.Contracts/Arslan.Vms.InventoryService.Application.Contracts.csproj" "services/inventory/src/Arslan.Vms.InventoryService.Application.Contracts/Arslan.Vms.InventoryService.Application.Contracts.csproj"
COPY "services/inventory/src/Arslan.Vms.InventoryService.Domain/Arslan.Vms.InventoryService.Domain.csproj" "services/inventory/src/Arslan.Vms.InventoryService.Domain/Arslan.Vms.InventoryService.Domain.csproj"
COPY "services/inventory/src/Arslan.Vms.InventoryService.Domain.Shared/Arslan.Vms.InventoryService.Domain.Shared.csproj" "services/inventory/src/Arslan.Vms.InventoryService.Domain.Shared/Arslan.Vms.InventoryService.Domain.Shared.csproj"
COPY "services/inventory/src/Arslan.Vms.InventoryService.EntityFrameworkCore/Arslan.Vms.InventoryService.EntityFrameworkCore.csproj" "services/inventory/src/Arslan.Vms.InventoryService.EntityFrameworkCore/Arslan.Vms.InventoryService.EntityFrameworkCore.csproj"
# Microservices Payment Service
COPY "services/payment/src/Arslan.Vms.PaymentService.HttpApi.Host/Arslan.Vms.PaymentService.HttpApi.Host.csproj" "services/payment/src/Arslan.Vms.PaymentService.HttpApi.Host/Arslan.Vms.PaymentService.HttpApi.Host.csproj"
COPY "services/payment/src/Arslan.Vms.PaymentService.HttpApi/Arslan.Vms.PaymentService.HttpApi.csproj" "services/payment/src/Arslan.Vms.PaymentService.HttpApi/Arslan.Vms.PaymentService.HttpApi.csproj"
COPY "services/payment/src/Arslan.Vms.PaymentService.HttpApi.Client/Arslan.Vms.PaymentService.HttpApi.Client.csproj" "services/payment/src/Arslan.Vms.PaymentService.HttpApi.Client/Arslan.Vms.PaymentService.HttpApi.Client.csproj"
COPY "services/payment/src/Arslan.Vms.PaymentService.Application/Arslan.Vms.PaymentService.Application.csproj" "services/payment/src/Arslan.Vms.PaymentService.Application/Arslan.Vms.PaymentService.Application.csproj"
COPY "services/payment/src/Arslan.Vms.PaymentService.Application.Contracts/Arslan.Vms.PaymentService.Application.Contracts.csproj" "services/payment/src/Arslan.Vms.PaymentService.Application.Contracts/Arslan.Vms.PaymentService.Application.Contracts.csproj"
COPY "services/payment/src/Arslan.Vms.PaymentService.Domain/Arslan.Vms.PaymentService.Domain.csproj" "services/payment/src/Arslan.Vms.PaymentService.Domain/Arslan.Vms.PaymentService.Domain.csproj"
COPY "services/payment/src/Arslan.Vms.PaymentService.Domain.Shared/Arslan.Vms.PaymentService.Domain.Shared.csproj" "services/payment/src/Arslan.Vms.PaymentService.Domain.Shared/Arslan.Vms.PaymentService.Domain.Shared.csproj"
COPY "services/payment/src/Arslan.Vms.PaymentService.EntityFrameworkCore/Arslan.Vms.PaymentService.EntityFrameworkCore.csproj" "services/payment/src/Arslan.Vms.PaymentService.EntityFrameworkCore/Arslan.Vms.PaymentService.EntityFrameworkCore.csproj"
# Microservices Planner Service
COPY "services/planner/src/Arslan.Vms.PlannerService.HttpApi.Host/Arslan.Vms.PlannerService.HttpApi.Host.csproj" "services/planner/src/Arslan.Vms.PlannerService.HttpApi.Host/Arslan.Vms.PlannerService.HttpApi.Host.csproj"
COPY "services/planner/src/Arslan.Vms.PlannerService.HttpApi/Arslan.Vms.PlannerService.HttpApi.csproj" "services/planner/src/Arslan.Vms.PlannerService.HttpApi/Arslan.Vms.PlannerService.HttpApi.csproj"
COPY "services/planner/src/Arslan.Vms.PlannerService.HttpApi.Client/Arslan.Vms.PlannerService.HttpApi.Client.csproj" "services/planner/src/Arslan.Vms.PlannerService.HttpApi.Client/Arslan.Vms.PlannerService.HttpApi.Client.csproj"
COPY "services/planner/src/Arslan.Vms.PlannerService.Application/Arslan.Vms.PlannerService.Application.csproj" "services/planner/src/Arslan.Vms.PlannerService.Application/Arslan.Vms.PlannerService.Application.csproj"
COPY "services/planner/src/Arslan.Vms.PlannerService.Application.Contracts/Arslan.Vms.PlannerService.Application.Contracts.csproj" "services/planner/src/Arslan.Vms.PlannerService.Application.Contracts/Arslan.Vms.PlannerService.Application.Contracts.csproj"
COPY "services/planner/src/Arslan.Vms.PlannerService.Domain/Arslan.Vms.PlannerService.Domain.csproj" "services/planner/src/Arslan.Vms.PlannerService.Domain/Arslan.Vms.PlannerService.Domain.csproj"
COPY "services/planner/src/Arslan.Vms.PlannerService.Domain.Shared/Arslan.Vms.PlannerService.Domain.Shared.csproj" "services/planner/src/Arslan.Vms.PlannerService.Domain.Shared/Arslan.Vms.PlannerService.Domain.Shared.csproj"
COPY "services/planner/src/Arslan.Vms.PlannerService.EntityFrameworkCore/Arslan.Vms.PlannerService.EntityFrameworkCore.csproj" "services/planner/src/Arslan.Vms.PlannerService.EntityFrameworkCore/Arslan.Vms.PlannerService.EntityFrameworkCore.csproj"

# Gateways
COPY "gateways/web/src/Arslan.Vms.WebGateway/Arslan.Vms.WebGateway.csproj" "gateways/web/src/Arslan.Vms.WebGateway/Arslan.Vms.WebGateway.csproj"

# Modules Keycloak
#COPY "modules/keycloak/src/Arslan.Keycloak.Application/Arslan.Keycloak.Application.csproj" "modules/keycloak/src/Arslan.Keycloak.Application/Arslan.Keycloak.Application.csproj"
#COPY "modules/keycloak/src/Arslan.Keycloak.Application.Contracts/Arslan.Keycloak.Application.Contracts.csproj" "modules/keycloak/src/Arslan.Keycloak.Application.Contracts/Arslan.Keycloak.Application.Contracts.csproj"

COPY "NuGet.Config" "NuGet.Config"

RUN dotnet restore "Arslan.Vms.sln"
COPY . .

WORKDIR "/src/services/administration/src/Arslan.Vms.AdministrationService.HttpApi.Host"
RUN dotnet publish -c Release -o /app 
# RUN dotnet publish --no-restore -c Release -o /app 

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Arslan.Vms.AdministrationService.HttpApi.Host.dll"]