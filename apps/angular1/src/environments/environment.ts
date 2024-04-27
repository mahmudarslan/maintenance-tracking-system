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
    issuer: 'http://localhost:8080/',
    redirectUri: baseUrl,
    clientId: 'vms_app',
    responseType: 'code',
    scope: 'offline_access Vms',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'http://localhost:44300',
      rootNamespace: 'Arslan.Vms',
    },
  },
} as Environment;
