<template>
  <el-drawer v-model="state.visible" direction="ltr" destroy-on-close size="100%" @closed="onClosed">
    <template #header="{ titleId, titleClass }">
      <div class="my-flex my-flex-between mr20">
        <span :id="titleId" :class="titleClass">{{ title }}</span>
      </div>
    </template>
    <div class="h100 w100 my-design" style="position: relative">
      <div class="my-flex w100 h100">
        <div style="width: 210px; min-width: 210px; border-right: 1px solid var(--el-border-color)">
          <el-scrollbar height="100%" max-height="100%" :always="false" wrap-style="padding:10px">
            <!-- 拖拽组件 -->
            <div id="hiprint-printEpContainer" class="rect-printElement-types hiprintEpContainer"></div>
          </el-scrollbar>
        </div>
        <div class="my-fill">
          <div style="padding: 10px; border-bottom: 1px solid var(--el-border-color)">
            <el-input-number
              v-model="state.scaleValue"
              size="small"
              :precision="1"
              :step="0.1"
              min="0.5"
              max="5"
              @change="onChangeScale"
              style="width: 90px"
            >
              <template #decrease-icon>
                <SvgIcon name="ele-ZoomOut" />
              </template>
              <template #increase-icon>
                <SvgIcon name="ele-ZoomIn" />
              </template>
            </el-input-number>

            <el-button-group size="small" class="ml10">
              <el-tooltip content="左对齐" placement="bottom">
                <el-button @click="onSetElsAlign('left')">
                  <el-icon>
                    <my-icon name="left" color="var(--color)"></my-icon>
                  </el-icon>
                </el-button>
              </el-tooltip>
              <el-tooltip content="居中" placement="bottom">
                <el-button @click="onSetElsAlign('vertical')">
                  <el-icon>
                    <my-icon name="vertical" color="var(--color)"></my-icon>
                  </el-icon>
                </el-button>
              </el-tooltip>
              <el-tooltip content="右对齐" placement="bottom">
                <el-button @click="onSetElsAlign('right')">
                  <el-icon>
                    <my-icon name="right" color="var(--color)"></my-icon>
                  </el-icon>
                </el-button>
              </el-tooltip>
              <el-tooltip content="顶对齐" placement="bottom">
                <el-button @click="onSetElsAlign('top')">
                  <el-icon>
                    <my-icon name="top" color="var(--color)"></my-icon>
                  </el-icon>
                </el-button>
              </el-tooltip>
              <el-tooltip content="垂直居中" placement="bottom">
                <el-button @click="onSetElsAlign('horizontal')">
                  <el-icon>
                    <my-icon name="horizontal" color="var(--color)"></my-icon>
                  </el-icon>
                </el-button>
              </el-tooltip>
              <el-tooltip content="底对齐" placement="bottom">
                <el-button @click="onSetElsAlign('bottom')">
                  <el-icon>
                    <my-icon name="bottom" color="var(--color)"></my-icon>
                  </el-icon>
                </el-button>
              </el-tooltip>
              <el-tooltip content="横向分散" placement="bottom">
                <el-button @click="onSetElsAlign('distributeHor')">
                  <el-icon>
                    <my-icon name="distributeHor" color="var(--color)"></my-icon>
                  </el-icon>
                </el-button>
              </el-tooltip>
              <el-tooltip content="纵向分散" placement="bottom">
                <el-button @click="onSetElsAlign('distributeVer')">
                  <el-icon>
                    <my-icon name="distributeVer" color="var(--color)"></my-icon>
                  </el-icon>
                </el-button>
              </el-tooltip>
            </el-button-group>
          </div>
          <el-scrollbar height="100%" max-height="100%" :always="false" wrap-style="padding:25px 0px 0px 25px;">
            <!-- 设计器 -->
            <div id="hiprint-printTemplate" class="hiprint-printTemplate"></div>
          </el-scrollbar>
        </div>
        <div style="width: 370px; min-width: 370px; border-left: 1px solid var(--el-border-color)">
          <el-scrollbar height="100%" max-height="100%" :always="false" wrap-style="padding:10px">
            <!-- 配置参数 -->
            <div id="hiprint-printElementOptionSetting"></div>
          </el-scrollbar>
        </div>
      </div>
    </div>
  </el-drawer>
</template>

<script lang="ts" setup name="admin/print-template-deisgn">
import { reactive, ref, onMounted, nextTick } from 'vue'
import { hiprint } from 'vue-plugin-hiprint'
import providers from './providers'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const state = reactive({
  visible: false,
  sureLoading: false,
  scaleValue: 1,
})

let hiprintTemplate = ref()

onMounted(() => {})

// 构建拖拽组件
const buildProvider = () => {
  let provider = providers[0]
  hiprint.init({ providers: [provider.f] })

  const printEpContainerEl = document.getElementById('hiprint-printEpContainer')
  if (printEpContainerEl) {
    printEpContainerEl.innerHTML = ''
  }
  hiprint.PrintElementTypeManager.build(printEpContainerEl, provider.value)
}

// 构建设计器
const buildDesigner = () => {
  const printTemplateEl = document.getElementById('hiprint-printTemplate')
  if (printTemplateEl) {
    printTemplateEl.innerHTML = ''
  }
  hiprintTemplate.value = new hiprint.PrintTemplate({
    template: {},
    settingContainer: '#hiprint-printElementOptionSetting',
    paginationContainer: '.hiprint-printPagination',
    fontList: [
      { title: '微软雅黑', value: 'Microsoft YaHei' },
      { title: '黑体', value: 'STHeitiSC-Light' },
      { title: '思源黑体', value: 'SourceHanSansCN-Normal' },
      { title: '王羲之书法体', value: '王羲之书法体' },
      { title: '宋体', value: 'SimSun' },
      { title: '华为楷体', value: 'STKaiti' },
      { title: 'cursive', value: 'cursive' },
    ],
    history: true,
  })

  hiprintTemplate.value.design('#hiprint-printTemplate')
}

//排版
const onSetElsAlign = (e: any) => {
  hiprintTemplate.value.setElsAlign(e)
}

// 缩放
const onChangeScale = () => {
  if (hiprintTemplate.value) {
    // scaleVal: 放大缩小值, false: 不保存(不传也一样), 如果传 true, 打印时也会放大
    hiprintTemplate.value.zoom(state.scaleValue)
  }
}

// 打开对话框
const open = async (row: any = {}) => {
  state.visible = true
  nextTick(() => {
    buildProvider()
    buildDesigner()
  })
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
}
</style>
