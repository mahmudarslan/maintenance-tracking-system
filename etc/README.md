 # Pre-requirements

* Docker Desktop with Kubernetes enabled
* Install [NGINX ingress](https://kubernetes.github.io/ingress-nginx/deploy/) for k8s

    OR

    Install NGINX ingress using helm
```powershell
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

helm upgrade --install --version=4.0.19 ingress-nginx ingress-nginx/ingress-nginx
```
* Install [Helm](https://helm.sh/docs/intro/install/) for running helm charts


# How to run?

* Add entries to the hosts file (in Windows: `C:\Windows\System32\drivers\etc\hosts`, in linux and macos: `/etc/hosts` ):

````powershell
127.0.0.1 vms-web
127.0.0.1 vms-authserver
127.0.0.1 vms-administration
127.0.0.1 vms-identity
127.0.0.1 vms-inventory
127.0.0.1 vms-order
127.0.0.1 vms-payment
127.0.0.1 vms-planner
127.0.0.1 vms-product
127.0.0.1 vms-vehicle
127.0.0.1 vms-gateway-web
````

* Run `build-images.ps1` or `build-images.sh` in the `build` directory.
* Run `deploy-staging.ps1` or `deploy-staging.sh` in the `helm-chart` directory. It is deployed with the `vms` namespace.
* *You may wait ~30 seconds on first run for preparing the database*.
* Browse https://vms-st-public-web for public and https://vms-st-web for web application
* Username: `admin`, password: `1q2w3E*`.

# Running on HTTPS

You can also run the staging solution on your local kubernetes kluster with https. There are various ways to create self-signed certificate. 

## Installing mkcert
This guide will use mkcert to create self-signed certificates.

Follow the [installation guide](https://github.com/FiloSottile/mkcert#installation) to install mkcert.

## Creating mkcert Root CA
Use the command to create root (local) certificate authority for your certificates:
```powershell
mkcert -install
```

**Note:** all the certificates created by mkcert certificate creation will be trusted by local machine

## Run mkcert

Create certificate for the Arslan.Vms domains using the mkcert command below:
```powershell
mkcert "vms-st-web" "vms-st-public-web" "vms-st-authserver" "vms-st-identity" "vms-st-administration" "vms-st-basket" "vms-st-catalog" "vms-st-ordering" "vms-st-cmskit" "vms-st-payment" "vms-st-gateway-web" "vms-st-gateway-web-public"
```

At the end of the output you will see something like

The certificate is at "./vms-st-web+10.pem" and the key at "./vms-st-web+10-key.pem"

Copy the cert name and key name below to create tls secret

```powershell
kubectl create namespace vms
kubectl create secret tls -n vms vms-wildcard-tls --cert=./vms-st-web+10.pem  --key=./vms-st-web+10-key.pem
```
