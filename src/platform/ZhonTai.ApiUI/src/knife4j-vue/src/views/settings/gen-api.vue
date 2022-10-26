<template>
  <a-layout-content :class="['knife4j-body-content', copyright?'':'knife4j-body-content--no-fotter']">
    <a v-for="(api, index) in apis" :key="index" @click="downloadApi(api)">{{api.description||api.name}}</a>
  </a-layout-content>
</template>
<script>
import Constants from "@/store/constants";
let localStore = null;
let instance = null;

export default {
  props: {
    data: {
    }
  },
  computed:{
    apis(){
      return this.data.instance.tags
    },
    copyright() {
      const servers = this.$store.state.globals.swaggerCurrentInstance
        ?.swaggerData?.servers
      if (servers && servers.length > 0) {
        return this.$store.state.globals.swaggerCurrentInstance.swaggerData
          .servers[0].extensions?.copyright
      } else {
        return ''
      }
    }
  },
  data() {
    return {

    };
  },
  created() {
  },
  methods: {
    downloadApi(api){
      const urls = api.childrens.length > 0 ? api.childrens[0].url.split('/'):[]
      let scope = urls.length > 2 ? urls[2] :'admin'
      scope = scope.replace(/^(\w)/,function($,$1){
        return $1.toLowerCase()
      })
      let apiName = api.name
      apiName = apiName.replace(/^(\w)/,function($,$1){
        return $1.toLowerCase()
      })
      let a = document.createElement("a");
      let content =
`/**
 *  ${api.description||apiName}
 *  @module @/api/${scope}/${apiName}
 */

import request from '@/utils/request'
import scope from './scope'
const apiPrefix = \`\${process.env.VUE_APP_BASE_API}/\${scope}/${apiName}/\`
`
      let functionNames = []
      for (const children of api.childrens) {
        const paths = children.url.split('/')
        let methodName = paths[paths.length-1]
        let reg = /[-\.\_](\w)/g
        let functionName = methodName.replace(reg, function($,$1){
          return $1.toUpperCase()
        })
        functionName = functionName.replace(/^(\w)/,function($,$1){
          return $1.toLowerCase()
        })
        functionName = functionName === 'delete' ? 'deleteAsync' : functionName
        functionNames[functionNames.length] = functionName
        const methodType = children.methodType ? children.methodType.toLowerCase() : 'get'
        content = content +
`
/**
 * ${children.summary||functionName}
 */
export const ${functionName} = (params, config = {}) => {
  return request.${methodType}(apiPrefix + '${methodName}', ${methodType=='post'||methodType=='put'||methodType=='patch'?'params, config':'{ params: params, ...config }'})
}
`
      }
      content = content +
`
export default {
`
      functionNames.forEach((exportFunctionName, index) => {
        content = content +
`  ${exportFunctionName}${index==functionNames.length-1?'':','}
`
      })

      content = content +
`}
`
      let option = {};
      const fileName = apiName + ".js";
      const url = window.URL.createObjectURL(
        new Blob([content], {
          type: (option.type || "text/plain") + ";charset=" + (option.encoding || "utf-8")
        })
      );
      a.href = url;
      a.download = fileName || "file";
      a.click();
      window.URL.revokeObjectURL(url);
    }
  }
};
</script>

<style lang="less" scoped>
  .knife4j-body-content{
    margin-top:10px;
    a{
      margin-right: 20px;
      line-height: 2;
      white-space: nowrap;
    }
  }
</style>
