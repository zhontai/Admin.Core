<template>
  <el-drawer v-model="state.visible" direction="ltr" destroy-on-close size="100%" @closed="onClosed">
    <template #header="{ titleId, titleClass }">
      <div class="my-flex my-flex-between mr10">
        <span :id="titleId" :class="titleClass">{{ title }}</span>
        <div>
          <el-tooltip content="刷新" placement="bottom">
            <el-button link @click="onRefresh">
              <template #icon>
                <el-icon size="18px">
                  <ele-Refresh></ele-Refresh>
                </el-icon>
              </template>
            </el-button>
          </el-tooltip>
        </div>
      </div>
    </template>
    <Design ref="designRef" :title="title" :id="state.id"></Design>
  </el-drawer>
</template>

<script lang="ts" setup name="admin/print-template-deisgn">
import { reactive, ref } from 'vue'
import Design from './design.vue'
import { PrintTemplateGetPageOutput } from '/@/api/admin/data-contracts'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const designRef = ref()

const state = reactive({
  visible: false,
  id: 0,
})

//刷新
const onRefresh = async () => {
  designRef.value.refresh()
}

// 打开对话框
const open = async (row: PrintTemplateGetPageOutput = {}) => {
  state.id = row.id as number
  state.visible = true
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
