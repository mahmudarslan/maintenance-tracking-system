import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Vms',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'http://localhost:8080/realms/arslan',
    redirectUri: baseUrl,
    clientId: 'vms',
    responseType: 'code',
    scope: 'offline_access',
    requireHttps: false,
    cacheLocation: 'localstorage' // <-- add this config
  },
  apis: {
    default: {
      url: 'http://localhost:44300',
      rootNamespace: 'Arslan.Vms',
    },
  },
} as Environment;
