export version="1.0.5"

az acr login --name volocr

docker tag Arslan.Vms/app-web:"${version}" volocr.azurecr.io/Arslan.Vms/app-web:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/app-web:"${version}"

docker tag Arslan.Vms/app-authserver:"${version}" volocr.azurecr.io/Arslan.Vms/app-authserver:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/app-authserver:"${version}"

docker tag Arslan.Vms/app-publicweb:"${version}" volocr.azurecr.io/Arslan.Vms/app-publicweb:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/app-publicweb:"${version}"

docker tag Arslan.Vms/gateway-web:"${version}" volocr.azurecr.io/Arslan.Vms/gateway-web:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/gateway-web:"${version}"

docker tag Arslan.Vms/gateway-web-public:"${version}" volocr.azurecr.io/Arslan.Vms/gateway-web-public:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/gateway-web-public:"${version}"

docker tag Arslan.Vms/service-identity:"${version}" volocr.azurecr.io/Arslan.Vms/service-identity:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/service-identity:"${version}"

docker tag Arslan.Vms/service-administration:"${version}" volocr.azurecr.io/Arslan.Vms/service-administration:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/service-administration:"${version}"

docker tag Arslan.Vms/service-basket:"${version}" volocr.azurecr.io/Arslan.Vms/service-basket:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/service-basket:"${version}"

docker tag Arslan.Vms/service-catalog:"${version}" volocr.azurecr.io/Arslan.Vms/service-catalog:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/service-catalog:"${version}"

docker tag Arslan.Vms/service-ordering:"${version}" volocr.azurecr.io/Arslan.Vms/service-ordering:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/service-ordering:"${version}"

docker tag Arslan.Vms/service-cmskit:"${version}" volocr.azurecr.io/Arslan.Vms/service-cmskit:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/service-cmskit:"${version}"

docker tag Arslan.Vms/service-payment:"${version}" volocr.azurecr.io/Arslan.Vms/service-payment:"${version}"
docker push volocr.azurecr.io/Arslan.Vms/service-payment:"${version}"