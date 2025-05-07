<template>
  <el-drawer v-model="state.visible" direction="ltr" destroy-on-close size="100%" @closed="onClosed">
    <template #header="{ titleId, titleClass }">
      <div class="my-flex my-flex-between mr10">
        <span :id="titleId" :class="titleClass">{{ title }}</span>
        <div>
          <el-button ref="saveRef" size="small" type="primary" round :loading="state.saveLoading">
            <template #icon>
              <el-icon>
                <my-icon name="save" color="var(--color)"></my-icon>
              </el-icon>
            </template>
            保存
          </el-button>
          <el-popover ref="popoverSaveRef" placement="bottom-end" :virtual-ref="saveRef" trigger="click" virtual-triggering :width="230" title="提示">
            <p class="my-flex my-flex-items-center">
              <SvgIcon name="ele-Warning" size="16" color="var(--el-color-warning)" class="mr5" />
              确定要保存设计模板吗？
            </p>
            <div class="mt10" style="text-align: right; margin: 0">
              <el-button size="small" text @click="onSaveCancel">取消</el-button>
              <el-button size="small" type="primary" @click="onSave"> 保存并关闭 </el-button>
              <el-button size="small" type="primary" @click="onSave(false)"> 保存 </el-button>
            </div>
          </el-popover>

          <el-tooltip content="刷新" placement="bottom">
            <el-button ref="refreshRef" link>
              <template #icon>
                <el-icon size="18px">
                  <ele-Refresh></ele-Refresh>
                </el-icon>
              </template>
            </el-button>
          </el-tooltip>
          <el-popover ref="popoverRefreshRef" placement="bottom" :virtual-ref="refreshRef" trigger="click" virtual-triggering :width="230">
            <p class="my-flex my-flex-items-center">确定要刷新设计模板吗？</p>
            <div class="mt10" style="text-align: right; margin: 0">
              <el-button size="small" text @click="onRefreshCancel">取消</el-button>
              <el-button size="small" type="primary" @click="onRefresh">确定</el-button>
            </div>
          </el-popover>
        </div>
      </div>
    </template>
    <Design ref="designRef" :title="title" v-model:printData="state.printTemplate.printData"></Design>
  </el-drawer>
</template>

<script lang="ts" setup name="admin/print-template-deisgn">
import { reactive, ref, getCurrentInstance, nextTick } from 'vue'
import Design from './design.vue'
import { PrintTemplateGetPageOutput } from '/@/api/admin/data-contracts'
import eventBus from '/@/utils/mitt'
import { PrintTemplateApi } from '/@/api/admin/PrintTemplate'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const designRef = ref()
const saveRef = ref()
const popoverSaveRef = ref()
const refreshRef = ref()
const popoverRefreshRef = ref()

const state = reactive({
  visible: false,
  saveLoading: false,
  refreshLoading: false,
  printTemplate: {
    id: 0,
    version: 0,
    printData: '{}',
  },
})

const loadData = async () => {
  if (state.printTemplate.id > 0) {
    state.refreshLoading = true
    const res = await new PrintTemplateApi().getUpdateTemplate({ id: state.printTemplate.id }).catch(() => {
      state.refreshLoading = false
    })
    state.refreshLoading = false
    if (res?.success) {
      const printTemplate = res.data
      state.printTemplate.id = printTemplate?.id as number
      state.printTemplate.version = printTemplate?.version as number
      state.printTemplate.printData = printTemplate?.printData || ('{}' as string)

      nextTick(() => {
        designRef.value?.hiprintTemplate.clear()
        const template = JSON.parse(printTemplate?.template || '{}')
        designRef.value?.hiprintTemplate.update(template)
        designRef.value?.setPaper(template)
      })
    }
  }
}

const onSaveCancel = () => {
  popoverSaveRef.value?.hide?.()
}

const onRefreshCancel = () => {
  popoverRefreshRef.value?.hide?.()
}

const onRefresh = () => {
  onRefreshCancel()
  loadData()
}

//保存
const onSave = async (close = true) => {
  try {
    if (designRef.value?.hiprintTemplate) {
      if (state.printTemplate.id != undefined && state.printTemplate.id > 0) {
        state.saveLoading = true
        const res = await new PrintTemplateApi()
          .updateTemplate(
            {
              id: state.printTemplate.id,
              version: state.printTemplate.version,
              template: JSON.stringify(designRef.value?.hiprintTemplate.getJson() || {}),
              printData: state.printTemplate.printData as string,
            },
            { showSuccessMessage: true }
          )
          .catch(() => {
            state.saveLoading = false
          })

        state.printTemplate.version = state.printTemplate.version + 1
        state.saveLoading = false
        onSaveCancel()
        if (res?.success) {
          eventBus.emit('refreshPrintTemplate')
          if (close) state.visible = false
        }
      } else {
        proxy.$modal.msgWarning('请选择打印模板')
      }
    }
  } catch (error) {}
}

// 打开对话框
const open = async (row: PrintTemplateGetPageOutput = {}) => {
  state.visible = true

  state.printTemplate.id = row.id as number
  await loadData()
}

// 关闭
const onClosed = () => {
  state.visible = false
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
:deep() {
  .left-box {
    .el-tabs__header {
      margin-bottom: 0px;
    }
    .el-tab-pane {
      height: 100%;
    }
  }

  .rect-printElement-types .hiprint-printElement-type a.ep-draggable-item {
    height: auto;
    color: #666;
    box-shadow: none !important;
    text-overflow: ellipsis;
  }

  .hiprint-printElement-type a.ep-draggable-item:before {
    display: block;
    width: 30px;
    height: 30px;
    content: '';
    background-repeat: no-repeat;
    background-size: contain;
    margin: 0 auto;
  }

  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.hline']:before {
    background-image: url('/@/assets/svgs/hiprint/hline.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.vline']:before {
    background-image: url('/@/assets/svgs/hiprint/vline.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.rect']:before {
    background-image: url('/@/assets/svgs/hiprint/rect.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.oval']:before {
    background-image: url('/@/assets/svgs/hiprint/oval.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.barcode']:before {
    background-image: url('/@/assets/svgs/hiprint/barcode.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.qrcode']:before {
    background-image: url('/@/assets/svgs/hiprint/qrcode.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.text']:before {
    background-image: url('/@/assets/svgs/hiprint/text.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.longText']:before {
    background-image: url('/@/assets/svgs/hiprint/longText.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.table']:before {
    background-image: url('/@/assets/svgs/hiprint/table.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.emptyTable']:before {
    background-image: url('/@/assets/svgs/hiprint/emptyTable.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.html']:before {
    background-image: url('/@/assets/svgs/hiprint/html.svg');
  }
  .hiprint-printElement-type a.ep-draggable-item[tid='comModule.image']:before {
    background-image: url('/@/assets/svgs/hiprint/image.svg');
  }
}
</style>
