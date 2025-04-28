import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [react()],
    build: {
        outDir: '../Scripts/react',  // on génère dans Scripts/
        emptyOutDir: true,
    },
    base: '/Scripts/react/', // nécessaire pour que les chemins fonctionnent
})
