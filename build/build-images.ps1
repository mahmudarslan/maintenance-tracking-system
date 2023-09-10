param ($version='1.0.0')

$currentFolder = $PSScriptRoot
$slnFolder = Join-Path $currentFolder "../"
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

$total = 9

Write-Host "===== BUILDING APPLICATIONS =====" -ForegroundColor Yellow

### IDENTITY-SERVICE
Write-Host "**************** BUILDING IDENTITY-SERVICE 1/$total ****************" -ForegroundColor Green
Set-Location $identityServiceFolder
docker build -f "$identityServiceFolder/Dockerfile" -t arslan.vms/service-identity:$version .

### ADMINISTRATION-SERVICE
Write-Host "**************** BUILDING ADMINISTRATION-SERVICE 2/$total ****************" -ForegroundColor Green
Set-Location $slnFolder
docker build -f "$administrationServiceFolder/Dockerfile" -t arslan.vms/service-administration:$version .

### INVENTORY-SERVICE
Write-Host "**************** BUILDING INVENTORY-SERVICE 3/$total ****************" -ForegroundColor Green
Set-Location $inventoryServiceFolder
docker build -f "$inventoryServiceFolder/Dockerfile" -t arslan.vms/service-basket:$version .

### ORDER-SERVICE
Write-Host "**************** BUILDING ORDER-SERVICE 4/$total ****************" -ForegroundColor Green
Set-Location $orderServiceFolder
docker build -f "$orderServiceFolder/Dockerfile" -t arslan.vms/service-catalog:$version .

### PAYMENT-SERVICE
Write-Host "**************** BUILDING PAYMENT-SERVICE 5/$total ****************" -ForegroundColor Green
Set-Location $paymentServiceFolder
docker build -f "$paymentServiceFolder/Dockerfile" -t arslan.vms/service-payment:$version .

### PLANNER-SERVICE
Write-Host "**************** BUILDING PLANNER-SERVICE 6/$total ****************" -ForegroundColor Green
Set-Location $plannerServiceFolder
docker build -f "$plannerServiceFolder/Dockerfile" -t arslan.vms/service-ordering:$version .

### PRODUCT-SERVICE
Write-Host "**************** BUILDING PRODUCT-SERVICE 7/$total ****************" -ForegroundColor Green
Set-Location $productServiceFolder
docker build -f "$productServiceFolder/Dockerfile" -t arslan.vms/service-cmskit:$version .

### VEHICLE-SERVICE
Write-Host "**************** BUILDING VEHICLE-SERVICE 8/$total ****************" -ForegroundColor Green
Set-Location $vehicleServiceFolder
docker build -f "$vehicleServiceFolder/Dockerfile" -t arslan.vms/service-cmskit:$version .

### WEB-GATEWAY
Write-Host "**************** BUILDING WEB-GATEWAY 9/$total ****************" -ForegroundColor Green
Set-Location $webGatewayFolder
docker build -f "$webGatewayFolder/Dockerfile" -t arslan.vms/gateway-web:$version .

### ALL COMPLETED
Write-Host "ALL COMPLETED" -ForegroundColor Green
Set-Location $currentFolder