import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [react()],
    build: {
        outDir: '../Scripts/react',  // on g�n�re dans Scripts/
        emptyOutDir: true,
    },
    base: '/Scripts/react/', // n�cessaire pour que les chemins fonctionnent
})
