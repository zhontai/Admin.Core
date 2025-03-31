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
            <el-tab-pane label="打印数据">
              <MyJsonEditor
                v-model="state.printTemplate.printData"
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
        <div class="my-fill" style="overflow: hidden; min-width: 355px" v-loading="state.refreshLoading">
          <!-- 操作栏 -->
          <div style="padding: 10px 10px 0px 10px; border-bottom: 1px solid var(--el-border-color)">
            <div class="my-flex my-flex-wrap my-flex-items-end">
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
              </div>
              <div class="my-fill my-flex my-flex-between">
                <!-- 操作 -->
                <el-button-group size="small" class="my-flex mr10 mb10">
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
                <el-button ref="saveRef" size="small" type="primary" plain :loading="state.saveLoading">
                  <template #icon>
                    <el-icon>
                      <my-icon name="save" color="var(--color)"></my-icon>
                    </el-icon>
                  </template>
                  保存
                </el-button>
                <el-popover
                  ref="popoverSaveRef"
                  placement="bottom-end"
                  :virtual-ref="saveRef"
                  trigger="click"
                  virtual-triggering
                  :width="230"
                  title="提示"
                >
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
              </div>
            </div>
          </div>
          <el-scrollbar ref="printTemplateScrollbarRef" height="100%" max-height="100%" :always="false" wrap-style="padding:25px 0px 0px 25px;">
            <!-- 画布 -->
            <div ref="designRef" class="hiprint-printTemplate"></div>
          </el-scrollbar>
        </div>
        <div style="width: 350px; min-width: 350px; border-left: 1px solid var(--el-border-color)">
          <el-scrollbar height="100%" max-height="100%" :always="false" wrap-style="padding:10px">
            <!-- 配置 -->
            <div id="hiprint-printElementOptionSetting"></div>
          </el-scrollbar>
        </div>
      </div>
    </div>

    <PrintPreview ref="previewRef" title="预览"></PrintPreview>
    <ViewJson ref="previewJsonDialogRef" title="查看模板JSON"></ViewJson>
  </el-drawer>
</template>

<script lang="ts" setup name="admin/print-template-deisgn">
import { reactive, ref, onMounted, nextTick, getCurrentInstance } from 'vue'
import { hiprint } from 'vue-plugin-hiprint'
import providers from './providers'
import PrintPreview from './preview.vue'
import ViewJson from './view-json.vue'
import { PrintTemplateGetPageOutput } from '/@/api/admin/data-contracts'
import { PrintTemplateApi } from '/@/api/admin/PrintTemplate'
import eventBus from '/@/utils/mitt'
import MyJsonEditor from '/@/components/my-json-editor/index.vue'

interface IPaperType {
  type: string
  width: number
  height: number
}

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
  refreshLoading: false,
  saveLoading: false,
  printTemplate: {
    id: 0,
    version: 0,
    printData: '{}',
  },
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
const saveRef = ref()
const popoverSaveRef = ref()

onMounted(() => {})

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
  if (template?.panels?.length > 0) {
    const width = template.panels[0].width
    const height = template.panels[0].height
    const paperType = state.paperTypes.find((a) => a.width == width && a.height == height)
    state.curPaper = { type: paperType?.type || '', width: width, height: height }
  }

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
    previewRef.value.open(hiprintTemplate.value.getJson() || {}, JSON.parse(state.printTemplate.printData || '{}'))
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
    hiprintTemplate.value.print(JSON.parse(state.printTemplate.printData || '{}'))
  }
}

//查看Json
const onViewJson = () => {
  if (hiprintTemplate.value) {
    let templateJson = JSON.stringify(hiprintTemplate.value.getJson() || {})
    previewJsonDialogRef.value.open(templateJson)
  }
}

//刷新
const onRefresh = async () => {
  proxy.$modal
    .confirm(`确定要刷新设计模板吗？`)
    .then(async () => {
      try {
        if (state.printTemplate.id > 0) {
          state.refreshLoading = true
          const res = await new PrintTemplateApi().getUpdateTemplate({ id: state.printTemplate.id }).catch(() => {
            state.refreshLoading = false
          })
          state.refreshLoading = false
          if (res?.success) {
            const template = res.data
            state.printTemplate.id = template?.id as number
            state.printTemplate.version = template?.version as number

            buildDesigner(JSON.parse(template?.template || '{}'))
          }
        }
      } catch (error) {}
    })
    .catch(() => {})
}

const onSaveCancel = () => {
  popoverSaveRef.value?.hide?.()
}

//保存
const onSave = async (close = true) => {
  try {
    if (hiprintTemplate.value) {
      if (state.printTemplate.id != undefined && state.printTemplate.id > 0) {
        state.saveLoading = true
        const res = await new PrintTemplateApi()
          .updateTemplate(
            {
              id: state.printTemplate.id,
              version: state.printTemplate.version,
              template: JSON.stringify(hiprintTemplate.value.getJson() || {}),
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
  if (row.id && row.id > 0) {
    const res = await new PrintTemplateApi().getUpdateTemplate({ id: row.id }, { loading: true })
    if (res?.success) {
      const template = res.data
      state.printTemplate.id = template?.id as number
      state.printTemplate.version = template?.version as number
      state.printTemplate.printData = template?.printData || ('{}' as string)

      state.visible = true
      nextTick(() => {
        buildProvider()
        buildDesigner(JSON.parse(template?.template || '{}'))
      })
    }
  }
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
