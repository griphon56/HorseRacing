import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueJsx from '@vitejs/plugin-vue-jsx';
import { Agent } from 'https'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueJsx()
  ],
  resolve: {
    alias: {
      '~': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'https://localhost:7101',
        changeOrigin: true,
        secure: false,
        ws: true,
        agent: new Agent({
          maxSockets: 100,
          keepAlive: true,
          maxFreeSockets: 10,
          keepAliveMsecs: 10000,
          timeout: 60000
        }),
        auth: 'LOGIN:PASS',
        configure: (proxy, options) => {
          proxy.on('proxyRes', (proxyRes, req, _res) => {
            const key: string = 'www-authenticate'
            proxyRes.headers[key] =
              proxyRes.headers[key] && proxyRes.headers[key]?.toString().split(',')
          })
        }
      },
    }
  }
});