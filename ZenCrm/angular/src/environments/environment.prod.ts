import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44349/',
  redirectUri: baseUrl,
  clientId: 'ZenCrm_App',
  responseType: 'code',
  scope: 'offline_access ZenCrm',
  requireHttps: true,
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'ZenCrm',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44349',
      rootNamespace: 'ZenCrm',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;
