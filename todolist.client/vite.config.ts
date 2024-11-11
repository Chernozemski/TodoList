import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import { env } from 'process';

const backendHttpUrl = env.BACKEND_HTTP_URL ?? 'http://localhost:5002/';

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [plugin()],
	server: {
		proxy: {
			'^/todos': {
				target: backendHttpUrl,
				secure: false,
				configure: (proxy, _options) => {
			proxy.on('error', (err, _req, _res) => {
			  console.log('proxy error', err);
			});
			proxy.on('proxyReq', (proxyReq, req, _res) => {
			  console.log('Sending Request to the Target:', req.method, req.url);
			});
			proxy.on('proxyRes', (proxyRes, req, _res) => {
			  console.log('Received Response from the Target:', proxyRes.statusCode, req.url);
			});
		  },
			}
		},
		host: true,
		strictPort: true,
		port: Number(env.VITE_PORT)
	}
})