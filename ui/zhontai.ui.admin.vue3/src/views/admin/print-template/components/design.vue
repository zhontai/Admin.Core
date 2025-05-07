<template>
  <div class="h100 w100 my-design" style="position: relative">
    <div class="my-flex w100 h100">
      <div style="width: 220px; min-width: 220px; border-right: 1px solid var(--el-border-color)">
        <el-tabs tab-position="top" stretch class="h100 left-box">
          <el-tab-pane label="组件">
            <el-scrollbar height="100%" max-height="100%" :always="false" wrap-style="padding:10px">
              <!-- 拖拽组件 -->
              <div ref="epContainerRef" class="rect-printElement-types hiprintEpContainer"></div>
            </el-scrollbar>
          </el-tab-pane>
          <el-tab-pane label="数据源">
            <MyJsonEditor
              v-model="printData"
              :options="{
                mode: 'text',
                mainMenuBar: true,
                statusBar: false,
                showErrorTable: false,
                modes: [],
              }"
            ></MyJsonEditor>
          </el-tab-pane>
        </el-tabs>
      </div>
      <div class="my-fill" style="overflow: hidden; min-width: 355px">
        <!-- 操作栏 -->
        <div style="padding: 10px 10px 0px 10px; border-bottom: 1px solid var(--el-border-color)">
          <div class="my-flex my-flex-wrap">
            <div class="my-flex my-flex-wrap">
              <!-- 纸张 -->
              <el-select v-model="state.curPaper.type" size="small" placeholder="纸张" class="mr2 mb10" style="width: 60px" @change="onSetPaper">
                <el-option v-for="item in state.paperTypes" :key="item.type" :label="item.type" :value="item.type" />
              </el-select>
              <!-- 自定义纸张 -->
              <el-tooltip content="自定义纸张" placement="top">
                <el-button ref="paperRef" :type="state.curPaper.type === '' ? 'primary' : ''" size="small" class="mr10 mb10">
                  <el-icon>
                    <my-icon name="customSize" color="var(--color)"></my-icon>
                  </el-icon>
                </el-button>
              </el-tooltip>
              <el-popover ref="popoverRef" :virtual-ref="paperRef" trigger="click" virtual-triggering :width="300" title="设置纸张宽高(mm)">
                <p class="my-flex my-flex-items-center my-flex-between">
                  <el-input-number
                    v-model="state.customPaper.width"
                    placeholder="宽"
                    :precision="1"
                    :step="1"
                    min="0"
                    controls-position="right"
                    style="width: 110px"
                  >
                  </el-input-number>
                  ~
                  <el-input-number
                    v-model="state.customPaper.height"
                    placeholder="高"
                    :precision="1"
                    :step="1"
                    min="0"
                    controls-position="right"
                    style="width: 110px"
                  >
                  </el-input-number>
                </p>
                <div class="mt10">
                  <el-button size="small" type="primary" class="w100" @click="onCustomPaper"> 确定 </el-button>
                </div>
              </el-popover>

              <!-- 缩放 -->
              <el-input-number
                v-model="state.scaleValue"
                size="small"
                :precision="1"
                :step="0.1"
                min="0.5"
                max="5"
                class="mr10 mb10"
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
            </div>
            <div class="my-fill my-flex my-flex-wrap my-flex-between">
              <!-- 排版 -->
              <el-button-group size="small" class="my-flex mr10 mb10">
                <el-tooltip content="左对齐" placement="top">
                  <el-button @click="onSetElsAlign('left')">
                    <el-icon>
                      <my-icon name="left" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
                <el-tooltip content="居中" placement="top">
                  <el-button @click="onSetElsAlign('vertical')">
                    <el-icon>
                      <my-icon name="vertical" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
                <el-tooltip content="右对齐" placement="top">
                  <el-button @click="onSetElsAlign('right')">
                    <el-icon>
                      <my-icon name="right" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
                <el-tooltip content="顶对齐" placement="top">
                  <el-button @click="onSetElsAlign('top')">
                    <el-icon>
                      <my-icon name="top" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
                <el-tooltip content="垂直居中" placement="top">
                  <el-button @click="onSetElsAlign('horizontal')">
                    <el-icon>
                      <my-icon name="horizontal" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
                <el-tooltip content="底对齐" placement="top">
                  <el-button @click="onSetElsAlign('bottom')">
                    <el-icon>
                      <my-icon name="bottom" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
                <el-tooltip content="横向分散" placement="top">
                  <el-button @click="onSetElsAlign('distributeHor')">
                    <el-icon>
                      <my-icon name="distributeHor" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
                <el-tooltip content="纵向分散" placement="top">
                  <el-button @click="onSetElsAlign('distributeVer')">
                    <el-icon>
                      <my-icon name="distributeVer" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
                <el-tooltip content="旋转" placement="top">
                  <el-button @click="onRotatePaper">
                    <el-icon>
                      <my-icon name="rotate" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
              </el-button-group>

              <!-- 操作 -->
              <el-button-group size="small" class="my-flex mb10">
                <el-tooltip content="预览" placement="top">
                  <el-button icon="ele-View" @click="onPreView"></el-button>
                </el-tooltip>
                <el-tooltip content="清空" placement="top">
                  <el-button icon="ele-Delete" @click="onClearPaper"></el-button>
                </el-tooltip>
                <el-tooltip content="打印" placement="top">
                  <el-button icon="ele-Printer" @click="onPrint"> </el-button>
                </el-tooltip>
                <el-tooltip content="查看模板JSON" placement="top">
                  <el-button @click="onViewJson">
                    <el-icon>
                      <my-icon name="json" color="var(--color)"></my-icon>
                    </el-icon>
                  </el-button>
                </el-tooltip>
              </el-button-group>
            </div>
          </div>
        </div>
        <el-scrollbar
          ref="printTemplateScrollbarRef"
          height="100%"
          max-height="100%"
          :always="false"
          wrap-style="background-color: #fff;color: #333;"
        >
          <!-- 画布 -->
          <div ref="designRef" class="hiprint-printTemplate"></div>
        </el-scrollbar>
      </div>
      <div style="width: 350px; min-width: 350px; border-left: 1px solid var(--el-border-color)">
        <el-scrollbar height="100%" max-height="100%" :always="false" wrap-style="padding:0px">
          <!-- 配置 -->
          <div id="hiprint-printElementOptionSetting"></div>
        </el-scrollbar>
      </div>
    </div>
  </div>

  <PrintPreview ref="previewRef"></PrintPreview>
  <ViewJson ref="previewJsonDialogRef" title="查看模板JSON"></ViewJson>
</template>

<script lang="ts" setup name="admin/print-template-deisgn">
import { reactive, ref, onMounted, nextTick, getCurrentInstance } from 'vue'
import { hiprint } from 'vue-plugin-hiprint'
import providers from './providers'
import PrintPreview from './preview.vue'
import ViewJson from './view-json.vue'
import MyJsonEditor from '/@/components/my-json-editor/index.vue'

const title = defineModel('title', { type: String })
const printData = defineModel('printData', { type: String })

interface IPaperType {
  type: string
  width: number
  height: number
}

const state = reactive({
  visible: false,
  sureLoading: false,
  scaleValue: 1,
  // 当前纸张
  curPaper: {
    type: 'A4',
    width: 210,
    height: 296.6,
  } as IPaperType,
  // 自定义纸张
  customPaper: {
    type: '',
    width: 220,
    height: 80,
  } as IPaperType,
  // 纸张类型
  paperTypes: [
    {
      type: 'A3',
      width: 420,
      height: 296.6,
    },
    {
      type: 'A4',
      width: 210,
      height: 296.6,
    },
    {
      type: 'A5',
      width: 210,
      height: 147.6,
    },
    {
      type: 'B3',
      width: 500,
      height: 352.6,
    },
    {
      type: 'B4',
      width: 250,
      height: 352.6,
    },
    {
      type: 'B5',
      width: 250,
      height: 175.6,
    },
  ] as IPaperType[],
  showSaveDialog: false,
})

const { proxy } = getCurrentInstance() as any

let hiprintTemplate = ref()
const paperRef = ref()
const popoverRef = ref()
const printTemplateScrollbarRef = ref()
const previewRef = ref()
const previewJsonDialogRef = ref()
const epContainerRef = ref()
const designRef = ref()
const epDraggableItemRef = ref()

onMounted(() => {
  buildProvider()
  buildDesigner()
})

// 构建拖拽组件
const buildProvider = () => {
  let provider = providers[0]
  hiprint.init({ providers: [provider.f] })

  if (epContainerRef.value) {
    epContainerRef.value.innerHTML = ''
  }

  hiprint.PrintElementTypeManager.build(epContainerRef.value, provider.value)
}

// 构建设计器
const buildDesigner = (template = {} as any) => {
  if (designRef.value) {
    designRef.value.innerHTML = ''
  }
  hiprintTemplate.value = new hiprint.PrintTemplate({
    template: template,
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

  hiprintTemplate.value.design(designRef.value)

  designRef.value.querySelector('.hiprint-printPaper')?.firstChild.click()
}

const setPaper = (template = {} as any) => {
  if (template?.panels?.length > 0) {
    const width = template.panels[0].width
    const height = template.panels[0].height
    const paperType = state.paperTypes.find((a) => a.width == width && a.height == height)
    state.curPaper = { type: paperType?.type || '', width: width, height: height }
    if (state.curPaper.type === '') {
      state.customPaper.width = width
      state.customPaper.height = height
    }
  } else {
    onSetPaper(state.curPaper.type)
  }
}

/**
 * 设置纸张大小
 * @param type [A3, A4, A5, B3, B4, B5, '']
 * @param value {width,height} mm
 */
const onSetPaper = (type: string, value?: { width: number; height: number }) => {
  try {
    const paperType = state.paperTypes.find((x) => x.type == type)
    if (paperType) {
      state.curPaper = { type: type, width: paperType.width, height: paperType.height }
      hiprintTemplate.value.setPaper(paperType.width, paperType.height)
    } else {
      state.curPaper = { type: '', width: value?.width as number, height: value?.height as number }
      hiprintTemplate.value.setPaper(value?.width, value?.height)
    }
    nextTick(() => {
      printTemplateScrollbarRef.value.update()
    })
  } catch (error) {
    proxy.$modal.msgError(`操作失败: ${error}`)
  }
}

//自定义纸张
const onCustomPaper = () => {
  popoverRef.value?.hide?.()
  onSetPaper('', { width: state.customPaper.width, height: state.customPaper.height })
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

// 旋转
const onRotatePaper = () => {
  if (hiprintTemplate.value) {
    hiprintTemplate.value.rotatePaper()
  }
}

//预览
const onPreView = () => {
  if (hiprintTemplate.value) {
    previewRef.value.open(hiprintTemplate.value.getJson() || {}, JSON.parse(printData.value || '{}'), title.value)
  }
}

// 清空
const onClearPaper = () => {
  proxy.$modal
    .confirm(`确定要清空设计模板吗？`)
    .then(async () => {
      try {
        if (hiprintTemplate.value) {
          hiprintTemplate.value.clear()
        }
      } catch (error) {}
    })
    .catch(() => {})
}

//打印
const onPrint = async () => {
  if (hiprintTemplate.value) {
    hiprintTemplate.value.print(JSON.parse(printData.value || '{}'))
  }
}

//查看Json
const onViewJson = () => {
  if (hiprintTemplate.value) {
    let templateJson = JSON.stringify(hiprintTemplate.value.getJson() || {})
    previewJsonDialogRef.value.open(templateJson)
  }
}

defineExpose({
  hiprintTemplate,
  setPaper,
})
</script>

<style lang="scss">
.hiprint-printElement {
  color: #000;
}
</style>

<style scoped lang="scss">
.hiprint-printTemplate {
  padding: 15px 10px 10px 15px;
}
:deep() {
  .jsoneditor {
    border-width: 0px;
  }
  .jsoneditor-menu {
    color: var(--el-text-color-primary);
    background-color: var(--el-color-primary);
    border-bottom: 1px solid var(--el-border-color);
  }

  textarea.jsoneditor-text {
    background-color: var(--el-bg-color-overlay);
    color: var(--el-text-color-primary);
  }

  .hiprint-option-items {
    padding: 10px;
  }
  .prop-tabs {
    background-color: var(--el-bg-color);
    border-style: none;
    box-shadow: none;
    border-color: var(--el-border-color);
  }
  .prop-tabs .prop-tab-items {
    display: flex;
    height: 40px;
    padding: 0px;
    border-bottom-color: var(--el-border-color);
  }
  .prop-tabs .prop-tab-items .prop-tab-item {
    background-color: var(--el-bg-color);
    flex: 1;
    height: 40px;
    line-height: 40px;
    text-align: center;
  }
  .prop-tabs .prop-tab-items li.active,
  .prop-tabs .prop-tab-items li:hover {
    border-bottom: 2px solid var(--el-color-primary);
    color: var(--el-color-primary);
  }
  .prop-tabs .prop-tab-items .prop-tab-item .tab-title {
    font-weight: normal;
  }
  .prop-tabs .hiprint-option-items {
    background-color: var(--el-bg-color);
    padding: 10px;
  }
  .hiprint-option-item-settingBtn {
    width: 100%;
    height: 30px;
    line-height: 30px;
    background-color: var(--el-color-primary);
  }
  .prop-tabs .hiprint-option-item-settingBtn {
    width: 45%;
    margin-left: 10px;
  }
  .hiprint-option-item-submitBtn {
    background-color: var(--el-color-primary);
    margin-bottom: 20px;
  }
  .hiprint-option-item-deleteBtn {
    background-color: var(--el-color-danger);
    margin-left: 10px;
    margin-bottom: 20px;
  }
  .hiprint-option-items .hiprint-option-item-field input,
  .hiprint-option-items .hiprint-option-item-field select,
  .hiprint-option-items .hiprint-option-item-field textarea {
    border: none;
    background-color: var(--el-bg-color);
    border-radius: var(--el-input-border-radius, var(--el-border-radius-base));
    box-shadow: 0 0 0 1px var(--el-input-border-color, var(--el-border-color)) inset;
    padding: 1px 11px;
    height: calc(var(--el-component-size, 32px));
    line-height: calc(var(--el-component-size, 32px) - 2px);
    &:hover {
      box-shadow: 0 0 0 1px var(--el-border-color-hover) inset;
    }
    &:focus {
      box-shadow: 0 0 0 1px var(--el-color-primary) inset;
    }
  }

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
