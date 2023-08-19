param ($version='1.0.0')

$currentFolder = $PSScriptRoot
$slnFolder = Join-Path $currentFolder "../"
# Apps
$webAppFolder = Join-Path $slnFolder "apps/angular"
$authserverFolder = Join-Path $slnFolder "apps/auth-server/src/Arslan.Vms.AuthServer"
$publicWebFolder = Join-Path $slnFolder "apps/public-web/src/Arslan.Vms.PublicWeb"
# Gateways
$webGatewayFolder = Join-Path $slnFolder "gateways/web/src/Arslan.Vms.WebGateway"
$webPublicGatewayFolder = Join-Path $slnFolder "gateways/web-public/src/Arslan.Vms.WebPublicGateway"
# Microservices
$identityServiceFolder = Join-Path $slnFolder "services/identity/src/Arslan.Vms.IdentityService.HttpApi.Host"
$administrationServiceFolder = Join-Path $slnFolder "services/administration/src/Arslan.Vms.AdministrationService.HttpApi.Host"
$basketServiceFolder = Join-Path $slnFolder "services/basket/src/Arslan.Vms.BasketService"
$catalogServiceFolder = Join-Path $slnFolder "services/catalog/src/Arslan.Vms.CatalogService.HttpApi.Host"
$paymentServiceFolder = Join-Path $slnFolder "services/payment/src/Arslan.Vms.PaymentService.HttpApi.Host"
$orderingServiceFolder = Join-Path $slnFolder "services/ordering/src/Arslan.Vms.OrderingService.HttpApi.Host"
$cmskitServiceFolder = Join-Path $slnFolder "services/cmskit/src/Arslan.Vms.CmskitService.HttpApi.Host"

$total = 12

### Angular WEB App(WWW)
Write-Host "*** BUILDING WEB (WWW) 1/$total ****************" -ForegroundColor Green
Set-Location $webAppFolder
yarn
# ng build --prod
npm run build:prod


### AUTH-SERVER
Write-Host "*** BUILDING AUTH-SERVER 2/$total ****************" -ForegroundColor Green
Set-Location $authserverFolder
dotnet publish -c Release


### PUBLIC-WEB
Write-Host "*** BUILDING WEB-PUBLIC 3/$total ****************" -ForegroundColor Green
Set-Location $publicWebFolder
dotnet publish -c Release

### WEB-GATEWAY
Write-Host "*** BUILDING WEB-GATEWAY 4/$total ****************" -ForegroundColor Green
Set-Location $webGatewayFolder
dotnet publish -c Release


### PUBLICWEB-GATEWAY
Write-Host "*** BUILDING WEB-PUBLIC-GATEWAY 5/$total ****************" -ForegroundColor Green
Set-Location $webPublicGatewayFolder
dotnet publish -c Release


### IDENTITY-SERVICE
Write-Host "*** BUILDING IDENTITY-SERVICE 6/$total ****************" -ForegroundColor Green
Set-Location $identityServiceFolder
dotnet publish -c Release


### ADMINISTRATION-SERVICE
Write-Host "*** BUILDING ADMINISTRATION-SERVICE 7/$total ****************" -ForegroundColor Green
Set-Location $administrationServiceFolder
dotnet publish -c Release


### BASKET-SERVICE
Write-Host "**************** BUILDING BASKET-SERVICE 8/$total ****************" -ForegroundColor Green
Set-Location $basketServiceFolder
dotnet publish -c Release


### CATALOG-SERVICE
Write-Host "**************** BUILDING CATALOG-SERVICE 9/$total ****************" -ForegroundColor Green
Set-Location $catalogServiceFolder
dotnet publish -c Release


### PAYMENT-SERVICE
Write-Host "**************** BUILDING PAYMENT-SERVICE 10/$total ****************" -ForegroundColor Green
Set-Location $paymentServiceFolder
dotnet publish -c Release


### ORDERING-SERVICE
Write-Host "**************** BUILDING ORDERING-SERVICE 11/$total ****************" -ForegroundColor Green
Set-Location $orderingServiceFolder
dotnet publish -c Release

### CMSKIT-SERVICE
Write-Host "**************** BUILDING CMSKIT-SERVICE 12/$total ****************" -ForegroundColor Green
Set-Location $cmskitServiceFolder
dotnet publish -c Release


### ALL COMPLETED
Write-Host "ALL COMPLETED" -ForegroundColor Green
Set-Location $currentFolder