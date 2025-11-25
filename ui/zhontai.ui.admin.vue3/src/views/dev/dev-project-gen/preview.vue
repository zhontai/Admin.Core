<template>
  <MySplitter>
    <el-splitter-panel size="20%" min="200" max="40%">
      <div class="my-flex-column w100 h100">
        <group-template-menu
          :groupIds="state.filter.groupIds"
          :projectId="state.filter.projectId"
          @node-click="onNodeClick"
          select-first-node
          v-model="state.filter.templateIds"
        ></group-template-menu>
      </div>
    </el-splitter-panel>
    <el-splitter-panel>
      <div class="my-flex-column w100 h100">
        <el-card v-loading="state.loading" class="mt8 my-fill" shadow="never" :body-style="{ paddingBottom: '0' }">
          <el-form label-position="top">
            <el-form-item label="生成路径">
              <el-input type="text" v-model="state.previewData.path"></el-input>
            </el-form-item>
            <el-form-item label="生成内容">
              <el-input type="textarea" v-model="state.previewData.content" autosize></el-input>
            </el-form-item>
          </el-form>
        </el-card>
      </div>
    </el-splitter-panel>
  </MySplitter>
</template>

<script lang="ts" setup name="dev/dev-project-gen/preview">
import { DevProjectGenApi } from '/@/api/dev/DevProjectGen'

const MySplitter = defineAsyncComponent(() => import('/@/components/my-layout/splitter.vue'))
const GroupTemplateMenu = defineAsyncComponent(() => import('./components/dev-group-template-menu.vue'))

const route = useRoute()

const state = reactive({
  loading: false,
  filter: {
    projectId: null,
    groupIds: null,
    templateIds: null,
    isPreview: true,
  } as any,
  previewData: {
    path: null as string | null | undefined,
    content: null as string | null | undefined,
  },
})

onMounted(() => {
  state.filter.projectId = route.query.projectId
  if (typeof route.query.groupIds == 'string') state.filter.groupIds = route.query.groupIds?.split(',')
})

onBeforeMount(() => {})

const onQuery = async () => {
  state.loading = true
  const res = await new DevProjectGenApi().generate(state.filter, { showErrorMessage: false }).catch(() => {
    state.loading = false
  })
  if (res?.data && res.data.length > 0) {
    state.previewData.path = res.data[0].path
    state.previewData.content = res.data[0].content
  } else {
    state.previewData.path = null
    state.previewData.content = res?.msg
  }
  state.loading = false
}

const onNodeClick = (node: any) => {
  if (state.filter && node != null && !node.isGroup) {
    state.filter.templateIds = [node?.id]
    onQuery()
  }
}
</script>
