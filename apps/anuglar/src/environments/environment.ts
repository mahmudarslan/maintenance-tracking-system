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
    clientId: 'Web',
    responseType: 'code',
    scope: 'offline_access openid profile email phone roles AdministrationService IdentityService BasketService CatalogService OrderingService PaymentService CmskitService',
    requireHttps: false
  },
  apis: {
    default: {
      url: 'https://localhost:7500',
      rootNamespace: 'Arslan.Vms',
    },
    inventory: {
      url: 'https://localhost:7006',
      rootNamespace: 'Arslan.Vms.InventoryService',
    },
  },
  // remoteEnv: {
  //   url: '/assets/appsettings.json',
  //   mergeStrategy: 'deepmerge',
  //   method: 'GET'
  // }
} as Environment;
