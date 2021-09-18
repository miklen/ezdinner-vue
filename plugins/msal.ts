import * as msal from '@azure/msal-browser'
import Vue, { PluginObject, VueConstructor } from 'vue'

// https://dev.to/425show/secure-your-vue-js-apis-with-azure-ad-b2c-42j6
// https://github.com/AspNetMonsters/vue-azure-b2c-sample
declare module 'vue/types/vue' {
  interface Vue {
    $msal: MsalPlugin
  }
}

export interface MsalPluginOptions {
  clientId: string
  loginAuthority: string
  passwordAuthority: string
  knownAuthority: string
  redirectUri: string
}

let msalInstance: msal.PublicClientApplication

export class MsalPlugin implements PluginObject<MsalPluginOptions> {
  private pluginOptions: MsalPluginOptions = {
    clientId: '',
    loginAuthority: '',
    passwordAuthority: '',
    knownAuthority: '',
    redirectUri: '',
  }

  private isAuthenticated = false
  private isAuthenticatedPromise: Promise<boolean> = Promise.resolve(false)

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  public install(vue: VueConstructor<Vue>, options?: MsalPluginOptions): void {
    if (!options) {
      throw new Error('MsalPluginOptions must be specified')
    }
    this.pluginOptions = options
    this.initialize(options)
  }

  private initialize(options: MsalPluginOptions) {
    const msalConfig: msal.Configuration = {
      auth: {
        redirectUri: options.redirectUri,
        clientId: options.clientId,
        authority: options.loginAuthority,
        knownAuthorities: [options.knownAuthority],
      },
      cache: {
        cacheLocation: 'localStorage',
      },
      system: {
        loggerOptions: {
          loggerCallback: (
            level: msal.LogLevel,
            message: string,
            containsPii: boolean,
          ): void => {
            if (containsPii) {
              return
            }
            switch (level) {
              case msal.LogLevel.Error:
                console.error(message)
                return
              case msal.LogLevel.Info:
                console.info(message)
                return
              case msal.LogLevel.Verbose:
                console.debug(message)
                return
              case msal.LogLevel.Warning:
                console.warn(message)
            }
          },
          piiLoggingEnabled: false,
          logLevel: msal.LogLevel.Verbose,
        },
      },
    }
    msalInstance = new msal.PublicClientApplication(msalConfig)
    this.isAuthenticatedPromise = this.setupLoginRedirectHandler()
  }

  public async setupLoginRedirectHandler() {
    const response = await msalInstance.handleRedirectPromise()
    if (response !== null) {
      this.isAuthenticated = !!response?.account
    } else {
      // need to call getAccount here?
      const currentAccounts = msalInstance.getAllAccounts()
      if (!currentAccounts || currentAccounts.length < 1) {
        this.isAuthenticated = false
      } else if (currentAccounts.length > 1) {
        // Add choose account code here
      } else if (currentAccounts.length === 1) {
        this.isAuthenticated = !!currentAccounts[0]
      }
    }

    return this.isAuthenticated
  }

  public async signIn() {
    try {
      const loginRequest: msal.RedirectRequest = {
        scopes: [
          'openid',
          'profile',
          'offline_access',
          'https://ezlifehacks.onmicrosoft.com/98d97e1a-7c16-4f2f-89e3-ca839335a122/backendapi',
        ],
      }
      return await msalInstance.loginRedirect(loginRequest)
    } catch (err) {
      // handle error
      if (err.errorMessage && err.errorMessage.includes('AADB2C90118')) {
        try {
          const passwordResetResponse: msal.AuthenticationResult = await msalInstance.loginPopup(
            {
              scopes: [
                'openid',
                'profile',
                'offline_access',
                'https://ezlifehacks.onmicrosoft.com/98d97e1a-7c16-4f2f-89e3-ca839335a122/backendapi',
              ],
              authority: this.pluginOptions.passwordAuthority,
            },
          )
          this.isAuthenticated = !!passwordResetResponse.account
        } catch (passwordResetError) {
          console.error(passwordResetError)
        }
      } else {
        this.isAuthenticated = false
      }
    }
  }

  public async signOut() {
    await msalInstance.logoutRedirect()
    this.isAuthenticated = false
  }

  public async acquireToken() {
    const request = {
      account: msalInstance.getAllAccounts()[0],
      scopes: [
        'https://ezlifehacks.onmicrosoft.com/98d97e1a-7c16-4f2f-89e3-ca839335a122/backendapi',
      ],
    }
    try {
      const response = await msalInstance.acquireTokenSilent(request)
      return response.accessToken
    } catch (error) {
      if (error instanceof msal.InteractionRequiredAuthError) {
        return msalInstance.acquireTokenPopup(request).catch((popupError) => {
          console.error(popupError)
        })
      }
      return false
    }
  }

  public getFirstName(): string | undefined {
    const account = this.getAccount()
    const claims: any = account?.idTokenClaims
    return claims?.given_name
  }

  public getLastName(): string | undefined {
    const account = this.getAccount()
    const claims: any = account?.idTokenClaims
    return claims?.family_name
  }

  private getAccount(): msal.AccountInfo | undefined {
    if (this.isAuthenticated) return msalInstance.getAllAccounts()[0]
    return undefined
  }

  public async getIsAuthenticated(): Promise<boolean> {
    return await this.isAuthenticatedPromise
  }
}
