import vue from '@vitejs/plugin-vue'
import { resolve } from 'path'
import { defineConfig, ConfigEnv, UserConfig } from 'vite'
import compression from 'vite-plugin-compression'
import vueSetupExtend from 'vite-plugin-vue-setup-extend'
import { loadEnv } from '/@/utils/vite'
import { createSvgIconsPlugin } from 'vite-plugin-svg-icons'
import AutoImport from 'unplugin-auto-import/vite'
import fs from 'node:fs'
import path from 'node:path'

const pathResolve = (dir: string): any => {
  return resolve(__dirname, '.', dir)
}

const alias: Record<string, string> = {
  '/@': pathResolve('./src/'),
  'vue-i18n': 'vue-i18n/dist/vue-i18n.cjs.js',
}

const viteConfig = defineConfig(({ mode, command }: ConfigEnv) => {
  const env = loadEnv(mode)

  // 更新 env.config.js 中的 VITE_API_URL
  try {
    const envConfigPath = path.resolve('./public/env.config.js')
    let existingConfig = { VITE_API_URL: '' }

    // 尝试读取现有配置
    if (fs.existsSync(envConfigPath)) {
      const fileContent = fs.readFileSync(envConfigPath, 'utf-8')
      const configMatch = fileContent.match(/window\.__ENV_CONFIG__\s*=\s*(\{[\s\S]*?\})/)
      if (configMatch && configMatch[1]) {
        try {
          // 将字符串转换为对象
          const configObj = Function('return ' + configMatch[1])()
          if (typeof configObj === 'object') {
            existingConfig = configObj
          }
        } catch (e) {
          console.warn('解析现有配置失败，将保留部分配置', e)
        }
      }
    }

    // 只更新 VITE_API_URL
    existingConfig.VITE_API_URL = env.VITE_API_URL

    // 写入更新后的配置
    fs.writeFileSync(
      envConfigPath,
      `window.__ENV_CONFIG__ = ${JSON.stringify(existingConfig, null, 2)}
`
    )
  } catch (e) {
    console.error('更新 env.config.js 失败', e)
  }

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
      AutoImport({
        imports: ['vue', 'vue-router', 'pinia'],
        vueTemplate: true,
        dts: './src/auto-imports.d.ts',
        exclude: ['**/node_modules/**', '**/dist/**', '**/src/auto-imports.d.ts'],
      }),
      {
        name: 'html-version',
        transformIndexHtml(html) {
          return html.replace(/<script src="(\/env\.config\.js)"><\/script>/, `<script src="$1?v=${process.env.npm_package_version}"></script>`)
        },
      },
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
    //https://sass-lang.com/documentation/breaking-changes/legacy-js-api
    css: { preprocessorOptions: { css: { charset: false }, scss: { api: 'modern' } } },
    define: {
      __VUE_I18N_LEGACY_API__: JSON.stringify(false),
      __VUE_I18N_FULL_INSTALL__: JSON.stringify(false),
      __INTLIFY_PROD_DEVTOOLS__: JSON.stringify(false),
      __NEXT_VERSION__: JSON.stringify(process.env.npm_package_version),
      __NEXT_NAME__: JSON.stringify(process.env.npm_package_name),
    },
  } as UserConfig
})

export default viteConfig
