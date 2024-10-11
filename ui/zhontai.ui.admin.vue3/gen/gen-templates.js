import path from 'node:path'
import { generateTemplates } from 'swagger-typescript-api'

//导出swagger-typescript-api内置模板
generateTemplates({
  cleanOutput: false,
  output: path.resolve(process.cwd(), './gen/templates'),
  httpClientType: 'axios',
  modular: true,
  silent: false,
  rewrite: false,
})
