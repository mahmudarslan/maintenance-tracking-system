import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Vms',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44342/',
    redirectUri: baseUrl,
    clientId: 'vms_app',
    responseType: 'code',
    scope: 'offline_access Vms',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44342',
      rootNamespace: 'Arslan.Vms',
    }
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;
