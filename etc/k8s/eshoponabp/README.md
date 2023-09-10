# abp-charts

# Arslan.Vms
This project is a reference for who want to build microservice solutions with the ABP Framework.

## Pre-requirement

* [Helm](https://helm.sh) must be installed to use the charts.
Please refer to Helm's [documentation](https://helm.sh/docs/) to get started.
* Install [NGINX ingress](https://kubernetes.github.io/ingress-nginx/deploy/) for k8s or Install NGINX ingress using helm
  ```powershell
  helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
  helm repo update

  helm upgrade --install --version=4.0.19 ingress-nginx ingress-nginx/ingress-nginx
  ```

Once Helm is set up properly, add the repo as follows:

```console
helm repo add abp-charts https://abpframework.github.io/abp-charts/Arslan.Vms
```

Initial authentication data (redirectURIs etc) are seeded based on **vms-st** name and **vms** namespace for the deployment.

## Configuring HTTPS

You can also run the staging solution on your local Kubernetes Cluster with HTTPS. There are various ways to create self-signed certificate. 

### Installing mkcert
This guide will use mkcert to create self-signed certificates.

Follow the [installation guide](https://github.com/FiloSottile/mkcert#installation) to install mkcert.

### Creating mkcert Root CA
Use the command to create root (local) certificate authority for your certificates:
```powershell
mkcert -install
```

**Note:** all the certificates created by mkcert certificate creation will be trusted by local machine

### Run mkcert

Create certificate for the Arslan.Vms domains using the mkcert command below:
```powershell
mkcert "vms-st-web" "vms-st-public-web" "vms-st-authserver" "vms-st-identity" "vms-st-administration" "vms-st-basket" "vms-st-catalog" "vms-st-ordering" "vms-st-cmskit" "vms-st-payment" "vms-st-gateway-web" "vms-st-gateway-web-public"
```

At the end of the output you will see something like

The certificate is at "./vms-st-web+10.pem" and the key at "./vms-st-web+10-key.pem"

Copy the cert name and key name below to create tls secret

```powershell
kubectl create namespace vms
kubectl create secret tls -n vms vms-wildcard-tls --cert=./vms-st-web+10.pem --key=./vms-st-web+10-key.pem
```

## How to run?

* Add entries to the hosts file (in Windows: `C:\Windows\System32\drivers\etc\hosts`, in linux and macos: `/etc/hosts` ):

  ````powershell
  127.0.0.1 vms-st-web
  127.0.0.1 vms-st-public-web
  127.0.0.1 vms-st-authserver
  127.0.0.1 vms-st-identity
  127.0.0.1 vms-st-administration
  127.0.0.1 vms-st-basket
  127.0.0.1 vms-st-catalog
  127.0.0.1 vms-st-ordering
  127.0.0.1 vms-st-cmskit
  127.0.0.1 vms-st-payment
  127.0.0.1 vms-st-gateway-web
  127.0.0.1 vms-st-gateway-web-public
  ````

* Run `helm upgrade --install vms-st abp-charts/Arslan.Vms --namespace vms --create-namespace`
* *You may wait ~30 seconds on first run for preparing the database*.
* Browse https://vms-st-public-web for public and https://vms-st-web for web application
* Username: `admin`, password: `1q2w3E*`.
