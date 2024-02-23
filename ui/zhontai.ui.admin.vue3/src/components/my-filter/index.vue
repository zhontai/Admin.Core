<template>
  <div>
    <el-tree
      :data="[state.dataTree]"
      :props="state.defaultProps"
      :expand-on-click-node="false"
      :default-expand-all="true"
      :indent="16"
      class="my-search-filter"
    >
      <template #default="{ node, data }">
        <template v-if="data.logic && !data.field">
          <div class="my-flex">
            <el-radio-group v-model="data.logic">
              <el-radio label="And">并且</el-radio>
              <el-radio label="Or">或者</el-radio>
            </el-radio-group>
            <el-button type="text" icon="ele-Plus" @click="onAddGroup(data)" class="ml16">分组</el-button>
            <el-button type="text" icon="ele-Plus" @click="onAddCondition(data)">条件</el-button>
            <el-button v-if="!data.root" icon="ele-Minus" class="ml8" @click="onDelete(node, data)" />
          </div>
        </template>
        <template v-else>
          <div>
            <el-select v-model="data.field" style="width: 120px; margin-right: 5px" @change="onChangeField(data)">
              <el-option v-for="(o, index) in props.fields" :key="index" :label="o.description" :value="o.field" />
            </el-select>
            <el-select v-model="data.operator" style="width: 120px; margin-right: 5px" @change="onChangeOperator(data)">
              <el-option v-for="(op, index) in getOperators(data.type)" :key="index" :label="op.label" :value="op.value" />
            </el-select>

            <template v-if="data.type === 'date'">
              <component
                :is="data.componentName"
                v-model="data.value"
                :type="data.config.type"
                unlink-panels
                start-placeholder="开始时间"
                end-placeholder="结束时间"
                :format="data.config.format"
                :value-format="data.config.valueFormat"
                :disabled-date="disabledDate"
                :style="{ width: data.config.type === 'date' ? '160px' : '220px', 'margin-right': '5px' }"
              ></component>
            </template>
            <el-input-number
              v-else-if="data.type === 'number'"
              v-model="data.value"
              controls-position="right"
              style="width: 160px; margin-right: 5px"
            />
            <el-switch v-else-if="data.type === 'bool'" v-model="data.value" style="margin-right: 5px" />
            <el-input v-else v-model="data.value" style="width: 160px; margin-right: 5px" />

            <el-button icon="ele-Minus" style="margin-right: 5px" @click="onDelete(node, data)" />
          </div>
        </template>
      </template>
    </el-tree>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'
import { cloneDeep } from 'lodash-es'

const props = defineProps({
  // {field: '', operator: 'Contains', description: '', value: '', componentName: 'el-input', config: {type: '', format:'', valueFormat:''}}
  // 默认字段 default: true
  // 字段类型 type: 'string:字符串 | date:日期 | number:数字 | bool:布尔'
  // 日期操作符 operator: 'datetimerange'
  // 日期控件配置 config: {type: 'datetimerange', format:''yyyy-MM-dd HH:mm'', valueFormat:''yyyy-MM-dd HH:mm'', defaultTime : ['00:00:00', '00:00:00']}
  fields: {
    type: Array as any,
    default() {
      return []
    },
  },
})

//const emits = defineEmits(['sure'])

const operators = {
  equal: { label: '等于', value: 'Equal' },
  notEqual: { label: '不等于', value: 'NotEqual' },
  contains: { label: '包含', value: 'Contains' },
  notContains: { label: '不包含', value: 'NotContains' },
  startsWith: { label: '开始以', value: 'StartsWith' },
  notStartsWith: { label: '开始不是以', value: 'NotStartsWith' },
  endsWith: { label: '结束以', value: 'EndsWith' },
  notEndsWith: { label: '结束不是以', value: 'NotEndsWith' },
  lessThan: { label: '小于', value: 'LessThan' },
  lessThanOrEqual: { label: '小于等于', value: 'LessThanOrEqual' },
  greaterThan: { label: '大于', value: 'GreaterThan' },
  greaterThanOrEqual: { label: '大于等于', value: 'GreaterThanOrEqual' },
  dateRange: { label: '时间段', value: 'dateRange' },
  any: { label: '在列表', value: 'Any' },
  notAny: { label: '不在列表', value: 'NotAny' },
}

const operatorGroups = {
  string: [
    operators.equal,
    operators.notEqual,
    operators.contains,
    operators.notContains,
    operators.startsWith,
    operators.notStartsWith,
    operators.endsWith,
    operators.notEndsWith,
  ],
  date: [
    operators.equal,
    operators.notEqual,
    operators.lessThan,
    operators.lessThanOrEqual,
    operators.greaterThan,
    operators.greaterThanOrEqual,
    operators.dateRange,
  ],
  number: [operators.equal, operators.notEqual, operators.lessThan, operators.lessThanOrEqual, operators.greaterThan, operators.greaterThanOrEqual],
  bool: [operators.equal, operators.notEqual],
}

let firstField = {
  field: '',
  value: '',
  operator: '',
  description: '',
  componentName: '',
  type: '',
  config: {},
} as any

if (props.fields && props.fields.length > 0) {
  const field = props.fields.find((a: any) => a.defaultSelect === true)
  if (field) {
    firstField = field
  } else {
    firstField = props.fields[0]
  }
}

const state = reactive({
  showDialog: false,
  dataTree: {
    root: true,
    logic: 'And',
    filters: [],
  },
  defaultProps: {
    label: '',
    children: ['filters'],
  },
  operatorGroups: operatorGroups as any,
  firstField: firstField,
})

const disabledDate = (time: any) => {
  return time.getTime() > Date.now()
}

const getOperators = (type: any) => {
  const ops = state.operatorGroups[type || 'string']
  return ops && ops.length > 0 ? ops : ops['string']
}

const onChangeField = (data: any) => {
  const field = props.fields.find((a: any) => a.field === data.field) as any
  data.value = ''
  data.componentName = field.componentName
  data.description = field.description
  data.type = field.type ? field.type : ''
  const operators = getOperators(data.type)
  let defaultOprator = ''
  if (field.operator) {
    const operatorIndex = operators.findIndex((a: any) => a.value === field.operator)
    defaultOprator = operatorIndex >= 0 ? field.operator : ''
  }
  if (!defaultOprator) {
    defaultOprator = operators[0].value
  }
  data.operator = defaultOprator
  data.config = { ...field.config }
  if (data.type === 'date') {
    let dateType = 'date'
    if (data.operator === 'dateRange') {
      dateType = dateType + 'range'
    }
    data.config.type = data.config.type ? data.config.type : dateType
    if (data.config.type.indexOf('range') >= 0) {
      data.operator = 'dateRange'
    }
    data.config.format = data.config.format ? data.config.format : 'YYYY-MM-DD'
    data.config.valueFormat = data.config.valueFormat ? data.config.valueFormat : 'YYYY-MM-DD'
    data.config.defaultTime = data.config.defaultTime ? data.config.defaultTime : ['00:00:00', '00:00:00']
  }
}

const onChangeOperator = (data: any) => {
  data.value = ''
  if (data.type === 'date') {
    if (data.operator === 'dateRange') {
      data.config.type = data.config.type + 'range'
    } else {
      if (data.config.type.indexOf('range') >= 0) {
        data.config.type = data.config.type.replace(/range$/, '')
      }
    }
  }
}

const onAddGroup = (data: any) => {
  const newFilter = {
    logic: 'And',
  }
  if (!data.filters) {
    data.filters = []
  }
  data.filters.push(newFilter)
}

const onAddCondition = (data: any) => {
  const firstOprator = getOperators(data.type)[0]
  const newFilter = {
    field: state.firstField.value,
    description: state.firstField.description,
    operator: firstOprator.operator,
    value: state.firstField.value || '',
    type: state.firstField.type,
    componentName: state.firstField.componentName,
    config: { ...state.firstField.config },
  }
  if (!data.filters) {
    data.filters = []
  }
  const index = data.filters.findIndex((a: any) => a.logic && !a.field)
  if (index >= 0) {
    data.filters.splice(index, 0, newFilter)
  } else {
    data.filters.push(newFilter)
  }
}

const onDelete = (node: any, data: any) => {
  const parent = node.parent
  const filters = parent.data.filters || parent.data
  const index = filters.findIndex((d: any) => d === data)
  filters.splice(index, 1)
}

const reset = () => {
  state.dataTree.filters = []
}

const getDynamicFilter = () => {
  return cloneDeep(state.dataTree)
}

defineExpose({ getDynamicFilter, reset })
</script>

<style lang="scss" scoped>
:deep() {
  .el-radio {
    margin-right: 20px;
    &:last-child {
      margin-right: 0px;
    }
  }
  .el-tree-node {
    white-space: unset;
  }
  .el-tree-node__content {
    height: auto;
    line-height: 40px;
    min-height: 40px;
    &:hover {
      background-color: unset;
      cursor: unset;
    }
  }
  .el-tree-node:focus > .el-tree-node__content {
    background-color: unset;
  }
}
</style>
