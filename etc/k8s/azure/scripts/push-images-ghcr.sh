export version="1.0.0"

az acr login --name volocr

docker tag Arslan.Vms/app-web:"${version}" ghcr.io/volosoft/Arslan.Vms/app-web:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/app-web:"${version}"

docker tag Arslan.Vms/app-authserver:"${version}" ghcr.io/volosoft/Arslan.Vms/app-authserver:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/app-authserver:"${version}"

docker tag Arslan.Vms/app-publicweb:"${version}" ghcr.io/volosoft/Arslan.Vms/app-publicweb:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/app-publicweb:"${version}"

docker tag Arslan.Vms/gateway-web:"${version}" ghcr.io/volosoft/Arslan.Vms/gateway-web:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/gateway-web:"${version}"

docker tag Arslan.Vms/gateway-web-public:"${version}" ghcr.io/volosoft/Arslan.Vms/gateway-web-public:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/gateway-web-public:"${version}"

docker tag Arslan.Vms/service-identity:"${version}" ghcr.io/volosoft/Arslan.Vms/service-identity:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/service-identity:"${version}"

docker tag Arslan.Vms/service-administration:"${version}" ghcr.io/volosoft/Arslan.Vms/service-administration:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/service-administration:"${version}"

docker tag Arslan.Vms/service-basket:"${version}" ghcr.io/volosoft/Arslan.Vms/service-basket:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/service-basket:"${version}"

docker tag Arslan.Vms/service-catalog:"${version}" ghcr.io/volosoft/Arslan.Vms/service-catalog:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/service-catalog:"${version}"

docker tag Arslan.Vms/service-ordering:"${version}" ghcr.io/volosoft/Arslan.Vms/service-ordering:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/service-ordering:"1.0.1"

docker tag Arslan.Vms/service-cmskit:"${version}" ghcr.io/volosoft/Arslan.Vms/service-cmskit:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/service-cmskit:"${version}"

docker tag Arslan.Vms/service-payment:"${version}" ghcr.io/volosoft/Arslan.Vms/service-payment:"${version}"
docker push ghcr.io/volosoft/Arslan.Vms/service-payment:"${version}"