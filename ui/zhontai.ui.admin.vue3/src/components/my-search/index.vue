<template>
  <div v-if="props.searchItems.length > 0">
    <el-form ref="formRef" :model="formState" :inline="true" label-width="auto" size="default">
      <el-row>
        <!-- 动态渲染表单项 -->
        <el-col
          v-for="(item, index) in searchItems"
          v-show="index < visibleCount || isExpanded"
          :key="index"
          :xs="24"
          :sm="12"
          :md="8"
          :lg="6"
          :xl="4"
          class="my-flex"
        >
          <el-form-item :label="item.label" :prop="item.field" :rules="item.rules" class="my-flex-fill">
            <component :is="item.componentName" v-model="formState[item.field]" clearable v-bind="item.attrs" class="w100" />
          </el-form-item>
        </el-col>

        <!-- 操作按钮区域 -->
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="my-flex">
          <el-form-item label-width="0px" class="my-flex-fill">
            <div class="my-flex my-flex-fill">
              <el-button v-if="showToggle" type="text" @click="onToggleExpanded">
                {{ isExpanded ? '收起' : '展开' }}
                <SvgIcon :name="isExpanded ? 'ele-ArrowUp' : 'ele-ArrowDown'" />
              </el-button>
              <el-button size="default" type="primary" @click="onSearch">查询</el-button>
              <el-button icon="ele-RefreshLeft" text bg @click="onReset">重置</el-button>
            </div>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </div>
</template>

<script setup lang="ts" name="coms/my-search">
import { reactive, ref, computed, watchEffect } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
import { cloneDeep } from 'lodash-es'

interface SearchItem {
  field: string
  operator: OperatorEnum
  label: string
  rules?: FormRules
  componentName: string
  attrs?: EmptyObjectType
  defaultValue?: any
}

// 定义父组件传过来的值
const props = defineProps({
  // 查询条件配置项
  searchItems: {
    type: Array<SearchItem>,
    default: () => [],
  },
  // 显示查询条件数量
  visibleCount: {
    type: Number,
    default: () => 3,
  },
  // 是否过滤空值
  isFilterEmptyValue: {
    type: Boolean,
    default: () => true,
  },
  // 是否重置查询
  isResetSearch: {
    type: Boolean,
    default: () => true,
  },
})

// 定义子组件向父组件传值/事件
const emit = defineEmits(['search'])

const isExpanded = ref(false)
const formRef = ref<FormInstance>()

// 表单初始值
const formState = reactive<EmptyObjectType>(
  props.searchItems.reduce((acc: EmptyObjectType, item) => {
    acc[item.field] = item.defaultValue ?? ''
    return acc
  }, {})
)

// 默认显示数量
const DEFAULT_VISIBLE_COUNT = 3

// 处理可见数量配置
const visibleCount = computed(() => {
  // 验证配置有效性
  const count = props.visibleCount ?? DEFAULT_VISIBLE_COUNT
  return Math.max(1, Math.min(count, props.searchItems.length))
})

// 计算剩余项数量
const remainingCount = computed(() => props.searchItems.length - visibleCount.value)

// 是否需要显示展开按钮
const showToggle = computed(() => props.searchItems.length > visibleCount.value && remainingCount.value > 0)

// 空值判断函数
const isEmptyValue = (value: any) => {
  return value === '' || value === null || value === undefined
}

// 查询
const onSearch = async () => {
  try {
    await formRef.value?.validate()
    const filters = Object.entries(formState)
      // 根据配置过滤空值
      .filter(([_, value]) => !props.isFilterEmptyValue || !isEmptyValue(value))
      .map(([field, value]) => ({
        field,
        operator: props.searchItems.find((item) => item.field === field)?.operator || 'Equal',
        value,
      }))

    emit('search', cloneDeep(formState), { logic: 'And', filters })
  } catch (error) {
    console.error('表单验证失败:', error)
  }
}

// 重置
const onReset = () => {
  formRef.value?.resetFields()
  props.searchItems.forEach((item) => {
    formState[item.field] = item.defaultValue ?? ''
  })
  if (props.isResetSearch) onSearch()
}

// 切换展开状态
const onToggleExpanded = () => {
  isExpanded.value = !isExpanded.value
}
</script>

<style scoped lang="scss"></style>
