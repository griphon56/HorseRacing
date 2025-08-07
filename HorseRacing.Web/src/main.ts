import './assets/main.css'
import 'bootstrap/dist/css/bootstrap.min.css';

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import { router } from './router'
import naive from 'naive-ui'
import { signalRService } from '~/core/signalr/signalr-service';
import { SignalRHubName } from '~/core/signalr/constants';
import { useAuthStore } from './stores/auth-store';

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(naive);

router.beforeEach(async (to, from, next) => {
    const authStore = useAuthStore()

    if (authStore.tokens?.AccessToken) {
        const state = signalRService.getConnectionState()
        if (state !== 'Connected') {
        try {
            await signalRService.connectToHub(SignalRHubName)
        } catch (e) {
            console.error('SignalR connection failed in guard', e)
        }
        }
    }

    next()
})

app.mount('#app')
