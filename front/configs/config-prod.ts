interface Config {
  ApiUrl: string
  AppVersion: string
}

const defaultConfig: Config = {
  ApiUrl: '',
  AppVersion: '1.0.1',
}

const useLocalEndpoints = false

const config: Config = {
  ...defaultConfig,
  ...(useLocalEndpoints
    ? {
        ApiUrl: 'http://localhost:5455'
      }
    : {
        ApiUrl: 'https://upgrader-back.tlabs.cc'
    }),
}

export default config
