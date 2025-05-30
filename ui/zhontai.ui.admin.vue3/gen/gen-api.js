import axios from 'axios'
import ejs from 'ejs'
import fs from 'node:fs'
import path from 'node:path'
import { generateApi } from 'swagger-typescript-api'

const projectPath = process.cwd()
const apiUrl = 'http://localhost:16010'

const apis = [
  {
    output: path.resolve(projectPath, './src/api/admin'),
    url: `${apiUrl}/doc/admin/swagger/admin/swagger.json`,
    enumUrl: `${apiUrl}/api/admin/get-enums`,
  },
  // {
  //   output: path.resolve(projectPath, './src/api/app'),
  //   url: `${apiUrl}/doc/app/swagger/app/swagger.json`,
  //   // enumUrl: `${apiUrl}/api/app/get-enums`,
  // },
]

const genEnums = async (api) => {
  console.log(`✨   try to get enums by URL "${api.enumUrl}"`)
  console.log(`⭐   start generating your typescript api`)
  const res = await axios.get(api.enumUrl).catch((error) => {
    console.error(error)
  })

  if (res?.data?.data?.length > 0) {
    ejs.renderFile(path.resolve(projectPath, './gen/templates/enum-contracts.ejs'), res.data, {}, function (err, content) {
      fs.writeFile(path.resolve(api.output + '/enum-contracts.ts'), content, (err) => {})
      console.log(`✅   api file "enum-contracts.ts" created in ${api.output}\n`)
    })
  }
}

apis?.forEach(async (api) => {
  if (api.enumUrl) {
    await genEnums(api)
  }

  await generateApi({
    output: api.output,
    templates: path.resolve(projectPath, './gen/templates'),
    url: api.url,
    httpClientType: 'axios',
    modular: true,
    cleanOutput: false,
    moduleNameIndex: 2, // 0 api, 1 api htt-client data-contracts, 2 apis htt-client data-contracts
    moduleNameFirstTag: true, //apis htt-client data-contracts
    unwrapResponseData: true,
    generateUnionEnums: true,
    defaultResponseType: 'AxiosResponse',
  }).catch((error) => console.error(error))
})
