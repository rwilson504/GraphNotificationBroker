
param appName string
param graphChangeTrackingSpId string
param userId string
param apiClientId string
@secure()
param apiClientSecret string
param corsUrls array
param authUrl string
param graphUrl string

module signalRUpstream 'signalrUpstream.bicep' = {
  name: 'dp${appName}-signalRUpstream'
  params: {
    name: appName
    apiClientId: apiClientId
    allowedOrigins: corsUrls
  }
}
