import vue from '@vitejs/plugin-vue'
import { resolve } from 'path'
import { defineConfig, ConfigEnv } from 'vite'
import compression from 'vite-plugin-compression'
import vueSetupExtend from 'vite-plugin-vue-setup-extend-plus'
import { loadEnv } from '/@/utils/vite'
import { createSvgIconsPlugin } from 'vite-plugin-svg-icons'

const pathResolve = (dir: string): any => {
  return resolve(__dirname, '.', dir)
}

const alias: Record<string, string> = {
  '/@': pathResolve('./src/'),
  'vue-i18n': 'vue-i18n/dist/vue-i18n.cjs.js',
}

const viteConfig = defineConfig(({ mode, command }: ConfigEnv) => {
  const env = loadEnv(mode)
  return {
    plugins: [
      vue(),
      vueSetupExtend(),
      compression({
        threshold: 5121,
        disable: !env.VITE_COMPRESSION,
        deleteOriginFile: false,
      }),
      createSvgIconsPlugin({
        iconDirs: [pathResolve('src/assets/icons')],
        symbolId: 'icon-[dir]-[name]',
        inject: 'body-last',
        customDomId: '__svg__icons__dom__',
      }),
    ],
    root: process.cwd(),
    resolve: { alias },
    base: command === 'serve' ? './' : env.VITE_PUBLIC_PATH,
    hmr: true,
    optimizeDeps: { exclude: ['vue-demi'] },
    server: {
      host: '0.0.0.0',
      port: env.VITE_PORT,
      open: env.VITE_OPEN,
      proxy: {
        '/gitee': {
          target: 'https://gitee.com',
          ws: true,
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/gitee/, ''),
        },
      },
    },
    build: {
      outDir: 'dist',
      chunkSizeWarningLimit: 1500,
      sourcemap: false,
      rollupOptions: {
        output: {
          chunkFileNames: 'assets/js/[name]-[hash].js',
          entryFileNames: 'assets/js/[name]-[hash].js',
          assetFileNames: 'assets/[ext]/[name]-[hash].[ext]',
          manualChunks(id) {
            if (id.includes('node_modules')) {
              return id.toString().match(/\/node_modules\/(?!.pnpm)(?<moduleName>[^\/]*)\//)?.groups!.moduleName ?? 'vender'
            }
          },
        },
      },
    },
    css: { preprocessorOptions: { css: { charset: false } } },
    define: {
      __VUE_I18N_LEGACY_API__: JSON.stringify(false),
      __VUE_I18N_FULL_INSTALL__: JSON.stringify(false),
      __INTLIFY_PROD_DEVTOOLS__: JSON.stringify(false),
      __NEXT_VERSION__: JSON.stringify(process.env.npm_package_version),
      __NEXT_NAME__: JSON.stringify(process.env.npm_package_name),
    },
  }
})

export default viteConfig
