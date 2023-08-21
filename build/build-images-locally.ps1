param ($version='1.0.0')

$currentFolder = $PSScriptRoot
$slnFolder = Join-Path $currentFolder "../"
# Apps
$webAppFolder = Join-Path $slnFolder "apps/angular"
$authserverFolder = Join-Path $slnFolder "apps/auth-server/src/Arslan.Vms.AuthServer"
$publicWebFolder = Join-Path $slnFolder "apps/public-web/src/Arslan.Vms.PublicWeb"
# Gateways
$webGatewayFolder = Join-Path $slnFolder "gateways/web/src/Arslan.Vms.WebGateway"
# Microservices
$administrationServiceFolder = Join-Path $slnFolder "services/administration/src/Arslan.Vms.AdministrationService.HttpApi.Host"
$identityServiceFolder = Join-Path $slnFolder "services/identity/src/Arslan.Vms.IdentityService.HttpApi.Host"
$inventoryServiceFolder = Join-Path $slnFolder "services/inventory/src/Arslan.Vms.InventoryService.HttpApi.Host"
$orderServiceFolder = Join-Path $slnFolder "services/order/src/Arslan.Vms.OrderService.HttpApi.Host"
$paymentServiceFolder = Join-Path $slnFolder "services/payment/src/Arslan.Vms.PaymentService.HttpApi.Host"
$plannerServiceFolder = Join-Path $slnFolder "services/planner/src/Arslan.Vms.PlannerService.HttpApi.Host"
$productServiceFolder = Join-Path $slnFolder "services/product/src/Arslan.Vms.ProductService.HttpApi.Host"
$vehicleServiceFolder = Join-Path $slnFolder "services/vehicle/src/Arslan.Vms.VehicleService.HttpApi.Host"

$total = 12

### Angular WEB App(WWW)
Write-Host "*** BUILDING WEB (WWW) 1/$total ****************" -ForegroundColor Green
Set-Location $webAppFolder
yarn
# ng build --prod
npm run build:prod
docker build -f Dockerfile.local -t Arslan.Vms/app-web:$version .

### AUTH-SERVER
Write-Host "*** BUILDING AUTH-SERVER 2/$total ****************" -ForegroundColor Green
Set-Location $authserverFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/app-authserver:$version .

### PUBLIC-WEB
Write-Host "*** BUILDING WEB-PUBLIC 3/$total ****************" -ForegroundColor Green
Set-Location $publicWebFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/app-publicweb:$version .

### WEB-GATEWAY
Write-Host "*** BUILDING WEB-GATEWAY 4/$total ****************" -ForegroundColor Green
Set-Location $webGatewayFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/gateway-web:$version .

### PUBLICWEB-GATEWAY
Write-Host "*** BUILDING WEB-PUBLIC-GATEWAY 5/$total ****************" -ForegroundColor Green
Set-Location $webPublicGatewayFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/gateway-web-public:$version .

### IDENTITY-SERVICE
Write-Host "*** BUILDING IDENTITY-SERVICE 6/$total ****************" -ForegroundColor Green
Set-Location $identityServiceFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/service-identity:$version .

### ADMINISTRATION-SERVICE
Write-Host "*** BUILDING ADMINISTRATION-SERVICE 7/$total ****************" -ForegroundColor Green
Set-Location $administrationServiceFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/service-administration:$version .

### BASKET-SERVICE
Write-Host "**************** BUILDING BASKET-SERVICE 8/$total ****************" -ForegroundColor Green
Set-Location $basketServiceFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/service-basket:$version .

### CATALOG-SERVICE
Write-Host "**************** BUILDING CATALOG-SERVICE 9/$total ****************" -ForegroundColor Green
Set-Location $catalogServiceFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/service-catalog:$version .

### PAYMENT-SERVICE
Write-Host "**************** BUILDING PAYMENT-SERVICE 10/$total ****************" -ForegroundColor Green
Set-Location $paymentServiceFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/service-payment:$version .

### ORDERING-SERVICE
Write-Host "**************** BUILDING ORDERING-SERVICE 11/$total ****************" -ForegroundColor Green
Set-Location $orderingServiceFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/service-ordering:$version .

### CMSKIT-SERVICE
Write-Host "**************** BUILDING CMSKIT-SERVICE 12/$total ****************" -ForegroundColor Green
Set-Location $cmskitServiceFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t Arslan.Vms/service-cmskit:$version .

### ALL COMPLETED
Write-Host "ALL COMPLETED" -ForegroundColor Green
Set-Location $currentFolder