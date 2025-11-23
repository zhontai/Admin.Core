<template>
  <el-card shadow="never" style="margin-top: 8px" body-style="padding:0px;" class="my-fill">
    <template #header>
      <div class="my-flex my-flex-items-center">
        <el-input v-model="state.filterText" placeholder="筛选模板" clearable />
        <el-icon class="cursor-pointer ml10" :title="state.templateStatus ? '显示所有模板' : '隐藏未启用模板'" @click.stop="displayTemplateStatus">
          <ele-Hide v-if="state.templateStatus == null" />
          <ele-View v-else />
        </el-icon>
      </div>
    </template>
    <el-scrollbar v-loading="state.loading" height="100%" max-height="100%" :always="false" wrap-style="padding:10px">
      <el-tree
        ref="menuRef"
        :data="state.treeData"
        node-key="id"
        :props="{ children: 'children', label: 'name' }"
        :filter-node-method="onFilterNode"
        highlight-current
        check-strictly
        default-expand-all
        render-after-expand
        :expand-on-click-node="false"
        v-bind="$attrs"
        @node-click="onNodeClick"
        @check-change="onCheckChange"
      >
        <template #default="{ node, data }">
          <div class="my-flex my-flex-between my-flex-items-center w100 pr5">
            <div class="my-flex my-flex-items-center" itle="点击图标可编辑模板">
              <el-icon class="color-primary mr5" v-if="data.isEnable">
                <ele-CircleCheck />
              </el-icon>
              {{ node.label }}
            </div>
            <template v-if="!data.isGroup">
              <el-icon @click.stop="editTemplate(node, data)" title="编辑模板">
                <ele-Edit />
              </el-icon>
            </template>
            <div class="my-flex my-flex-items-center" v-if="data.isGroup">
              <el-icon @click.stop="addTemplate(node, data)" title="添加模板" class="mr5">
                <ele-Plus />
              </el-icon>
              <el-icon @click.stop="editGroup(node, data)" title="编辑分组">
                <ele-Edit />
              </el-icon>
            </div>
          </div>
        </template>
      </el-tree>
    </el-scrollbar>
    <dev-template-form ref="devTemplateFormRef"></dev-template-form>
    <dev-group-form ref="devGroupFormRef"></dev-group-form>
  </el-card>
</template>

<script lang="ts" setup name="dev/project/gen/grouptemplatemenu">
import { DevProjectGenApi } from '/@/api/dev/DevProjectGen'
import { DevProjectGenPreviewMenuOutput } from '/@/api/dev/data-contracts'
import eventBus from '/@/utils/mitt'
import { ElTree } from 'element-plus'
// 引入组件
const DevTemplateForm = defineAsyncComponent(() => import('../../dev-template/components/dev-template-form.vue'))
const DevGroupForm = defineAsyncComponent(() => import('../../dev-group/components/dev-group-form.vue'))

interface Props {
  modelValue: number[] | null | undefined
  selectFirstNode: boolean
  projectId: number
  groupIds: number[]
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: () => [],
  selectFirstNode: false,
  projectId: 0,
  groupIds: () => [],
})

const devTemplateFormRef = ref()
const devGroupFormRef = ref()
const menuRef = ref<InstanceType<typeof ElTree>>()
const state = reactive({
  loading: false,
  filterText: '',
  treeData: [] as Array<DevProjectGenPreviewMenuOutput>,
  lastKey: 0,
  templateStatus: null as boolean | null,
})

watch(
  () => state.filterText,
  (val) => {
    menuRef.value?.filter(val)
  }
)

onMounted(() => {
  initData()
  eventBus.off('refreshDevTemplate')
  eventBus.on('refreshDevTemplate', async () => {
    initData()
    if (state.lastKey) {
      menuRef.value?.setCurrentKey(state.lastKey)
      let node = menuRef.value?.getNode(state.lastKey)
      emits('node-click', node?.data as DevProjectGenPreviewMenuOutput)
    }
  })
  eventBus.off('refreshDevGroup')
  eventBus.on('refreshDevGroup', async () => {
    initData()
  })
})

const emits = defineEmits<{
  (e: 'node-click', node: DevProjectGenPreviewMenuOutput | null): void
  (e: 'update:modelValue', node: any[] | undefined | null): void
}>()

const onFilterNode = (value: string, data: any) => {
  if (!value) return true
  return data.name?.indexOf(value) !== -1
}

const onNodeClick = (node: any) => {
  if (state.lastKey === node.id) {
    state.lastKey = 0
    menuRef.value?.setCurrentKey(undefined)
    emits('node-click', null)
  } else {
    state.lastKey = node.id as number
    emits('node-click', node)
  }
}

const onCheckChange = () => {
  emits('update:modelValue', menuRef.value?.getCheckedKeys())
}

const initData = async () => {
  state.loading = true
  const res = await new DevProjectGenApi()
    .getPreviewMenu({ projectId: props.projectId, groupIds: props.groupIds, templateStatus: state.templateStatus })
    .catch(() => {
      state.loading = false
    })
  state.loading = false
  if (res?.success && res.data && res.data.length > 0) {
    state.treeData = res.data.map((s: DevProjectGenPreviewMenuOutput) => {
      return {
        id: s.groupId,
        name: s.groupName,
        isGroup: true,
        children: s.templateList?.map((s2) => {
          return {
            id: s2.templateId,
            name: s2.templateName,
            parentId: s2.groupId,
            isEnable: s2.isEnable,
          }
        }),
      } as DevProjectGenPreviewMenuOutput
    })
    if (state.treeData.length > 0 && props.selectFirstNode) {
      nextTick(() => {
        // const firstNode = state.treeData[0]
        // menuRef.value?.setCurrentKey(firstNode.id)
        // emits('node-click', firstNode)
      })
    }
  } else {
    state.treeData = []
  }
}
const editTemplate = (node: any, data: any) => {
  devTemplateFormRef.value.open(
    {
      id: data.id,
    },
    {
      title: '编辑模板:' + data.name,
    }
  )
}
//添加模板
const addTemplate = (node: any, data: any) => {
  devTemplateFormRef.value.open(
    {},
    {
      title: '添加模板',
      groupId: data.id,
    }
  )
}
//编辑分组
const editGroup = (node: any, data: any) => {
  devGroupFormRef.value.open(
    {
      id: data.id,
    },
    {
      title: '编辑分组:' + data.name,
    }
  )
}
//模板状态切换
const displayTemplateStatus = () => {
  state.templateStatus = state.templateStatus == null ? true : null
  initData()
}

defineExpose({
  menuRef,
})
</script>
