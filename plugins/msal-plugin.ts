import Vue from 'vue'
import { MsalPlugin, MsalPluginOptions } from './msal';

export default ({ }, inject: any) => {

    const options: MsalPluginOptions = {
        clientId: '654aa80d-4783-43db-8ed9-4e160bb1d765',
        loginAuthority: 'https://ezlifehacks.b2clogin.com/ezlifehacks.onmicrosoft.com/B2C_1_signupsignin/',
        passwordAuthority: 'https://ezlifehacks.b2clogin.com/ezlifehacks.onmicrosoft.com/B2C_1_passwordreset/',
        knownAuthority: 'https://ezlifehacks.b2clogin.com',
        // this is required for the signin to work from any subroute
        redirectUri: 'http://localhost:3000'
    };

    const msal = new MsalPlugin()

    Vue.use(msal, options)

    // Vue.observable is required for isAuthenticated to be reactive in components
    inject('msal', Vue.observable(msal))
}


