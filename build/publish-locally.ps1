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

### ADMINISTRATION-SERVICE
Write-Host "*** BUILDING ADMINISTRATION-SERVICE 1/$total ****************" -ForegroundColor Green
Set-Location $administrationServiceFolder
dotnet publish -c Release

### IDENTITY-SERVICE
Write-Host "*** BUILDING IDENTITY-SERVICE 2/$total ****************" -ForegroundColor Green
Set-Location $identityServiceFolder
dotnet publish -c Release

### INVENTORY-SERVICE
Write-Host "**************** BUILDING INVENTORY-SERVICE 3/$total ****************" -ForegroundColor Green
Set-Location $inventoryServiceFolder
dotnet publish -c Release

### ORDER-SERVICE
Write-Host "**************** BUILDING ORDER-SERVICE 4/$total ****************" -ForegroundColor Green
Set-Location $orderServiceFolder
dotnet publish -c Release

### PAYMENT-SERVICE
Write-Host "**************** BUILDING PAYMENT-SERVICE 5/$total ****************" -ForegroundColor Green
Set-Location $paymentServiceFolder
dotnet publish -c Release

### PLANNER-SERVICE
Write-Host "**************** BUILDING PLANNER-SERVICE 6/$total ****************" -ForegroundColor Green
Set-Location $plannerServiceFolder
dotnet publish -c Release 

### PRODUCT-SERVICE
Write-Host "**************** BUILDING PRODUCT-SERVICE 7/$total ****************" -ForegroundColor Green
Set-Location $productServiceFolder
dotnet publish -c Release

### VEHICLE-SERVICE
Write-Host "**************** BUILDING VEHICLE-SERVICE 8/$total ****************" -ForegroundColor Green
Set-Location $vehicleServiceFolder
dotnet publish -c Release

### WEB-GATEWAY
Write-Host "*** BUILDING WEB-GATEWAY 9/$total ****************" -ForegroundColor Green
Set-Location $webGatewayFolder
dotnet publish -c Release

### ALL COMPLETED
Write-Host "ALL COMPLETED" -ForegroundColor Green
Set-Location $currentFolder