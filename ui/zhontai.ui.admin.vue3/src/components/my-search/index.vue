<template>
  <div class="table-search-container" v-if="props.searchItems.length > 0">
    <el-form ref="tableSearchRef" :model="state.form" size="default" label-width="100px" class="table-form">
      <el-row>
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20" v-for="(val, key) in visibleItems" :key="key">
          <template v-if="val.type !== ''">
            <el-form-item
              :label="val.label"
              :prop="val.prop"
              :rules="[{ required: val.required, message: `${val.label}不能为空`, trigger: val.type === 'input' ? 'blur' : 'change' }]"
            >
              <el-input v-if="val.type === 'input'" v-model="state.form[val.prop]" :placeholder="val.placeholder" clearable style="width: 100%" />
              <el-date-picker
                v-else-if="val.type === 'date'"
                v-model="state.form[val.prop]"
                type="date"
                :placeholder="val.placeholder"
                style="width: 100%"
              />
              <el-select v-else-if="val.type === 'select'" v-model="state.form[val.prop]" :placeholder="val.placeholder" style="width: 100%">
                <el-option v-for="item in val.options" :key="item.value" :label="item.label" :value="item.value"> </el-option>
              </el-select>
            </el-form-item>
          </template>
        </el-col>
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20">
          <el-form-item class="table-form-btn" label-width="0px">
            <div class="my-flex my-fill my-flex-end">
              <template v-if="showToggle">
                <div class="table-form-btn-toggle mr10" @click="onToggleExpanded">
                  <span>{{ isExpanded ? '收起' : '展开' }}</span>
                  <SvgIcon :name="isExpanded ? 'ele-ArrowUp' : 'ele-ArrowDown'" />
                </div>
              </template>
              <el-button size="default" type="primary" @click="onSearch(tableSearchRef)">查询</el-button>
              <el-button icon="ele-RefreshLeft" text bg class="ml10" @click="onReset(tableSearchRef)">重置</el-button>
            </div>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </div>
</template>

<script setup lang="ts" name="example/makeTableDemoSearch">
import { reactive, ref, onMounted, computed } from 'vue'
import type { FormInstance } from 'element-plus'

// 定义父组件传过来的值
const props = defineProps({
  // 搜索表单
  searchItems: {
    type: Array<TableSearchType>,
    default: () => [],
  },
  visibleCount: {
    type: Number,
    default: () => 3,
  },
})

const isExpanded = ref(false)

// 默认显示数量
const DEFAULT_VISIBLE_COUNT = 3

// 处理可见数量配置
const visibleCount = computed(() => {
  // 验证配置有效性
  const count = props.visibleCount ?? DEFAULT_VISIBLE_COUNT
  return Math.max(1, Math.min(count, props.searchItems.length))
})

// 动态计算显示项
const visibleItems = computed(() => props.searchItems.filter((_, index) => index < visibleCount.value || isExpanded.value))

// 计算剩余项数量
const remainingCount = computed(() => props.searchItems.length - visibleCount.value)

// 是否需要显示展开按钮
const showToggle = computed(() => props.searchItems.length > visibleCount.value && remainingCount.value > 0)

// 定义子组件向父组件传值/事件
const emit = defineEmits(['search'])

// 定义变量内容
const tableSearchRef = ref<FormInstance>()
const state = reactive({
  form: {} as EmptyObjectType,
})

// 查询
const onSearch = (formEl: FormInstance | undefined) => {
  if (!formEl) return
  formEl.validate((valid: boolean) => {
    if (!valid) {
      return
    }

    emit('search', state.form)
  })
}
// 重置
const onReset = (formEl: FormInstance | undefined) => {
  if (!formEl) return
  formEl.resetFields()
  emit('search', state.form)
}
// 切换展开状态
const onToggleExpanded = () => {
  isExpanded.value = !isExpanded.value
}
// 初始化 form 字段，取自父组件 search.prop
const initFormField = () => {
  if (props.searchItems.length <= 0) return false
  props.searchItems.forEach((v) => (state.form[v.prop] = ''))
}
// 页面加载时
onMounted(() => {
  initFormField()
})
</script>

<style scoped lang="scss">
.table-search-container {
  display: flex;
  .table-form {
    flex: 1;
    .table-form-btn-toggle {
      white-space: nowrap;
      user-select: none;
      display: flex;
      align-items: center;
      color: var(--el-color-primary);
      cursor: pointer;
    }
  }
}
</style>
