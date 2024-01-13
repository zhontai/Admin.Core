<template>
  <el-dialog v-model="state.showDialog" destroy-on-close :title="title" append-to-body draggable width="780px">
    <div style="padding: 0px 0px 8px 8px; background-color: var(--ba-bg-color)">
      <div>
        <el-input v-model="state.fontIconSearch" placeholder="筛选图标" clearable />
      </div>
      <div class="icon-selector-popper">
        <div class="icon-selector-warp">
          <div class="icon-selector-warp-title flex">
            <div class="flex-auto">{{ title }}</div>
            <div class="icon-selector-warp-title-tab" v-if="type === 'all'">
              <span :class="{ 'span-active': state.fontIconType === 'ali' }" @click="onIconChange('ali')" class="ml10" title="iconfont 图标">
                ali
              </span>
              <span :class="{ 'span-active': state.fontIconType === 'ele' }" @click="onIconChange('ele')" class="ml10" title="elementPlus 图标">
                ele
              </span>
              <span :class="{ 'span-active': state.fontIconType === 'awe' }" @click="onIconChange('awe')" class="ml10" title="fontawesome 图标">
                awe
              </span>
            </div>
          </div>
          <div class="icon-selector-warp-row">
            <el-row v-if="fontIconSheetsFilterList.length > 0">
              <el-col :xs="6" :sm="4" :md="4" :lg="4" :xl="4" @click="onColClick(v)" v-for="(v, k) in fontIconSheetsFilterList" :key="k">
                <div class="icon-selector-warp-item" :class="{ 'icon-selector-active': state.fontIconPrefix === v }" @dblclick="onSure">
                  <div class="flex-margin">
                    <div class="icon-selector-warp-item-value my-flex-column my-flex-items-center">
                      <SvgIcon :name="v" />
                      <div class="icon-name my-line-1" :title="v">{{ v }}</div>
                    </div>
                  </div>
                </div>
              </el-col>
            </el-row>
            <el-empty :image-size="100" v-if="fontIconSheetsFilterList.length <= 0" :description="emptyDescription"></el-empty>
          </div>
        </div>
      </div>
    </div>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="onCancel" size="default">取 消</el-button>
        <el-button type="primary" @click="onSure" size="default" :loading="sureLoading">确 定</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup>
import { reactive, onMounted, computed, PropType } from 'vue'
import initIconfont from '/@/utils/getStyleSheets'

// 定义父组件传过来的值
const props = defineProps({
  title: {
    type: String,
    default: '',
  },
  sureLoading: {
    type: Boolean,
    default: false,
  },
  // icon 图标类型
  type: {
    type: String,
    default: () => 'all',
  },
  // 自定义空状态描述文字
  emptyDescription: {
    type: String,
    default: () => '无相关图标',
  },
  modelValue: {
    type: String as PropType<string | undefined | null>,
    default: () => '',
  },
})

// 定义子组件向父组件传值/事件
const emits = defineEmits(['update:modelValue', 'get', 'clear', 'sure'])

const state = reactive({
  showDialog: false,
  loading: false,
  fontIconPrefix: props.modelValue,
  fontIconSearch: '',
  fontIconSheetsList: [],
  fontIconType: 'ele',
})

// 处理 icon type 类型为 all 时，类型 ali、ele、awe 回显问题
const initFontIconTypeEcho = () => {
  if (props.modelValue!.indexOf('iconfont') > -1) onIconChange('ali')
  else if (props.modelValue!.indexOf('ele-') > -1) onIconChange('ele')
  else if (props.modelValue!.indexOf('fa') > -1) onIconChange('awe')
  else onIconChange('ele')
}

// 图标搜索及图标数据显示
const fontIconSheetsFilterList = computed(() => {
  if (!state.fontIconSearch) return state.fontIconSheetsList
  let search = state.fontIconSearch.trim().toLowerCase()
  return state.fontIconSheetsList.filter((item: string) => {
    if (item.toLowerCase()?.indexOf(search) !== -1) return item
  })
})

// 初始化数据
const initFontIconData = async (type: string) => {
  state.fontIconSheetsList = []
  if (type === 'ali') {
    await initIconfont.ali().then((res: any) => {
      // 阿里字体图标使用 `iconfont xxx`
      state.fontIconSheetsList = res.map((i: string) => `iconfont ${i}`)
    })
  } else if (type === 'ele') {
    await initIconfont.ele().then((res: any) => {
      state.fontIconSheetsList = res
    })
  } else if (type === 'awe') {
    await initIconfont.awe().then((res: any) => {
      // fontawesome字体图标使用 `fa xxx`
      state.fontIconSheetsList = res.map((i: string) => `fa ${i}`)
    })
  }
}

// 图标点击切换
const onIconChange = (type: string) => {
  state.fontIconType = type
  initFontIconData(type)
}

// 获取当前点击的 icon 图标
const onColClick = (v: string) => {
  state.fontIconPrefix = v
  // emits('get', state.fontIconPrefix)
  // emits('update:modelValue', state.fontIconPrefix)
}

// 页面加载时
onMounted(() => {})

// 打开对话框
const open = async () => {
  await initFontIconTypeEcho()
  state.showDialog = true
}

// 关闭对话框
const close = () => {
  state.showDialog = false
}
// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  emits('sure', state.fontIconPrefix)
}

defineExpose({
  open,
  close,
})
</script>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'my-icon-select',
})
</script>

<style scoped lang="scss">
:deep(.el-dialog__body) {
  padding: 5px 10px;
}
.icon-selector-popper {
  padding: 0 !important;
  .icon-selector-warp {
    .icon-selector-warp-title {
      height: 40px;
      line-height: 40px;
      padding: 0px 5px;
      .icon-selector-warp-title-tab {
        span {
          cursor: pointer;
          &:hover {
            color: var(--el-color-primary);
            text-decoration: underline;
          }
        }
        .span-active {
          color: var(--el-color-primary);
          text-decoration: underline;
        }
      }
    }
    .icon-selector-warp-row {
      border: 1px solid var(--el-border-color);
      border-right-width: 0px;
      border-bottom-width: 0px;
      border-top: 1px solid var(--el-border-color);
      .el-scrollbar__bar.is-horizontal {
        display: none;
      }
      .icon-selector-warp-item {
        display: flex;
        border: 1px solid var(--el-border-color);
        height: 90px;
        border-left-width: 0px;
        border-top-width: 0px;
        .icon-selector-warp-item-value {
          i {
            font-size: 20px !important;
            color: var(--el-text-color-regular);
          }
          .icon-name {
            margin-top: 8px;
            padding: 0px 5px;
          }
        }
        &:hover {
          cursor: pointer;
          background-color: var(--el-color-primary-light-9);
          .icon-selector-warp-item-value {
            i,
            .icon-name {
              color: var(--el-color-primary);
            }
          }
        }
      }
      .icon-selector-active {
        background-color: var(--el-color-primary-light-9);
        .icon-selector-warp-item-value {
          i,
          .icon-name {
            color: var(--el-color-primary);
          }
        }
      }
    }
  }
}
</style>
