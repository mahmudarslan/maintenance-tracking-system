#!/bin/bash

export IMAGE_TAG="1.0.0"
export total=12
cd ../
export currentFolder=`pwd`
cd build/


echo "*** BUILDING WEB (WWW) 1/${total} ****************"
cd ${currentFolder}/apps/angular
yarn
# ng build --prod
npm run build:prod
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/app-web:${IMAGE_TAG}" .


echo "*** BUILDING AUTH-SERVER 2/$total ****************"
cd ${currentFolder}/apps/auth-server/src/Arslan.Vms.AuthServer
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/app-authserver:${IMAGE_TAG}" .


echo "*** BUILDING WEB-PUBLIC 3/$total ****************"
cd ${currentFolder}/apps/public-web/src/Arslan.Vms.PublicWeb
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/app-publicweb:${IMAGE_TAG}" .


echo "*** BUILDING WEB-GATEWAY 4/$total ****************"
cd ${currentFolder}/gateways/web/src/Arslan.Vms.WebGateway
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/gateway-web:${IMAGE_TAG}" .


echo "*** BUILDING WEB-PUBLIC-GATEWAY 5/$total ****************"
cd ${currentFolder}/gateways/web-public/src/Arslan.Vms.WebPublicGateway
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/gateway-web-public:${IMAGE_TAG}" .


echo "*** BUILDING IDENTITY-SERVICE 6/$total ****************"
cd ${currentFolder}/services/identity/src/Arslan.Vms.IdentityService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/service-identity:${IMAGE_TAG}" .


echo "*** BUILDING ADMINISTRATION-SERVICE 7/$total ****************"
cd ${currentFolder}/services/administration/src/Arslan.Vms.AdministrationService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/service-administration:${IMAGE_TAG}" .


echo "**************** BUILDING BASKET-SERVICE 8/$total ****************"
cd ${currentFolder}/services/basket/src/Arslan.Vms.BasketService
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/service-basket:${IMAGE_TAG}" .


echo "**************** BUILDING CATALOG-SERVICE 9/$total ****************"
cd ${currentFolder}/services/catalog/src/Arslan.Vms.CatalogService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/service-catalog:${IMAGE_TAG}" .


echo "**************** BUILDING PAYMENT-SERVICE 10/$total ****************"
cd ${currentFolder}/services/payment/src/Arslan.Vms.PaymentService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/service-payment:${IMAGE_TAG}" .


echo "**************** BUILDING ORDERING-SERVICE 11/$total ****************"
cd ${currentFolder}/services/ordering/src/Arslan.Vms.OrderingService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/service-ordering:${IMAGE_TAG}" .

echo "**************** BUILDING CMSKIT-SERVICE 12/$total ****************"
cd ${currentFolder}/services/cmskit/src/Arslan.Vms.CmskitService.HttpApi.Host
dotnet publish -c Release
docker build -f Dockerfile.local --force-rm -t "Arslan.Vms/service-cmskit:${IMAGE_TAG}" .

echo "ALL COMPLETED"