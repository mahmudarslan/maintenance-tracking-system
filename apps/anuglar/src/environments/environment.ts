import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Vms [Localhost',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'http://localhost:8080/realms/arslan',
    redirectUri: baseUrl,
    clientId: 'vms-web',
    responseType: 'code',
    scope: 'offline_access openid profile email phone roles',
    requireHttps: false
  },
  apis: {
    default: {
      url: 'http://localhost:4300',
      rootNamespace: 'Arslan.Vms',
    }
  },
  // remoteEnv: {
  //   url: '/assets/appsettings.json',
  //   mergeStrategy: 'deepmerge',
  //   method: 'GET'
  // }
} as Environment;
