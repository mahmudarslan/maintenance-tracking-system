#!/bin/bash

export IMAGE_TAG="1.0.0"
export total=12
cd ../
export currentFolder=`pwd`
cd build/

echo "*** BUILDING ADMINISTRATION-SERVICE 1/$total ****************"
cd ${currentFolder}/services/administration/src/Arslan.Vms.AdministrationService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local  --force-rm -t "Arslan.Vms/service-administrator:${IMAGE_TAG}" .

echo "*** BUILDING IDENTITY-SERVICE 2/$total ****************"
cd ${currentFolder}/services/identity/src/Arslan.Vms.IdentityService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local  --force-rm -t "Arslan.Vms/service-identity:${IMAGE_TAG}" .

echo "**************** BUILDING INVENTORY-SERVICE 3/$total ****************"
cd ${currentFolder}/services/inventory/src/Arslan.Vms.InventoryService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local  --force-rm -t "Arslan.Vms/service-inventory:${IMAGE_TAG}" .

echo "**************** BUILDING ORDER-SERVICE 4/$total ****************"
cd ${currentFolder}/services/order/src/Arslan.Vms.OrderService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local  --force-rm -t "Arslan.Vms/service-order:${IMAGE_TAG}" .

echo "**************** BUILDING PAYMENT-SERVICE 5/$total ****************"
cd ${currentFolder}/services/payment/src/Arslan.Vms.PaymentService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local  --force-rm -t "Arslan.Vms/service-payment:${IMAGE_TAG}" .

echo "**************** BUILDING PLANNER-SERVICE 6/$total ****************"
cd ${currentFolder}/services/planner/src/Arslan.Vms.PlannerService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local  --force-rm -t "Arslan.Vms/service-planner:${IMAGE_TAG}" .

echo "**************** BUILDING PRODUCT-SERVICE 7/$total ****************"
cd ${currentFolder}/services/product/src/Arslan.Vms.ProductService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local  --force-rm -t "Arslan.Vms/service-product:${IMAGE_TAG}" .

echo "**************** BUILDING VEHICLE-SERVICE 8/$total ****************"
cd ${currentFolder}/services/vehicle/src/Arslan.Vms.VehicleService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local  --force-rm -t "Arslan.Vms/service-vehicle:${IMAGE_TAG}" .

echo "*** BUILDING WEB-GATEWAY 9/$total ****************"
cd ${currentFolder}/gateways/web/src/Arslan.Vms.WebGateway
dotnet publish -c Release
docker build  -f Dockerfile.local --force-rm -t "Arslan.Vms/gateway-web:${IMAGE_TAG}" .

echo "ALL COMPLETED"