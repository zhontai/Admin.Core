<template>
  <div>
    <div v-if="state.dataTree.length === 0" class="empty-filter">
      <el-empty :image-size="60">
        <template #description>
          <el-button type="primary" link @click="onAddGroup('')">新增查询条件</el-button>
        </template>
      </el-empty>
    </div>
    <el-tree
      v-else
      :data="state.dataTree"
      :props="state.defaultProps"
      :expand-on-click-node="false"
      :default-expand-all="true"
      :indent="16"
      class="my-search-filter"
    >
      <template #default="{ node, data }">
        <template v-if="data.logic && !data.field">
          <div class="my-flex">
            <el-segmented
              v-model="data.logic"
              size="small"
              :options="[
                {
                  label: '并且',
                  value: 'And',
                },
                {
                  label: '或者',
                  value: 'Or',
                },
              ]"
            />
            <el-button type="primary" link icon="ele-Plus" @click="onAddGroup(data)" class="ml16">分组</el-button>
            <el-button type="primary" link icon="ele-Plus" @click="onAddCondition(data)">条件</el-button>
            <el-button type="danger" link icon="ele-Minus" class="ml8" @click="onDelete(node, data)" />
          </div>
        </template>
        <template v-else>
          <div class="my-flex my-flex-wrap ml8 w100">
            <el-select placeholder="请选择字段" v-model="data.field" style="width: 130px; margin-right: 5px" @change="onChangeField(data)">
              <el-option v-for="(o, index) in props.fields" :key="index" :label="o.label" :value="o.field" />
            </el-select>
            <el-select placeholder="请选择操作符" v-model="data.operator" style="width: 130px; margin-right: 5px" @change="onChangeOperator(data)">
              <el-option v-for="(op, index) in getOperators(data.type)" :key="index" :label="op.label" :value="op.value" />
            </el-select>
            <component :is="data.componentName" v-model="data.value" clearable v-bind="data.attrs" class="my-flex-fill" />
            <el-button type="danger" link icon="ele-Minus" class="ml5" @click="onDelete(node, data)" />
          </div>
        </template>
      </template>
    </el-tree>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'
import { cloneDeep, mergeWith } from 'lodash-es'
import { Operator } from '/@/api/admin.extend/enum-contracts'

const props = defineProps({
  fields: {
    type: Array as any,
    default() {
      return []
    },
  },
})

const operatorGroups = {
  string: [
    Operator.equal,
    Operator.notEqual,
    Operator.contains,
    Operator.notContains,
    Operator.startsWith,
    Operator.notStartsWith,
    Operator.endsWith,
    Operator.notEndsWith,
  ],
  date: [
    Operator.equal,
    Operator.notEqual,
    Operator.lessThan,
    Operator.lessThanOrEqual,
    Operator.greaterThan,
    Operator.greaterThanOrEqual,
    Operator.dateRange,
  ],
  number: [Operator.equal, Operator.notEqual, Operator.lessThan, Operator.lessThanOrEqual, Operator.greaterThan, Operator.greaterThanOrEqual],
  bool: [Operator.equal, Operator.notEqual],
  select: [Operator.equal, Operator.notEqual],
}

let firstField = {} as any

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
  dataTree: [
    {
      root: true,
      logic: 'And',
      filters: [
        {
          field: '',
          operator: '',
          value: '',
          type: '',
          componentName: 'el-input',
          attrs: {
            placeholder: '请输入字段值',
          },
        },
      ],
    },
  ],
  defaultProps: {
    label: '',
    children: 'filters',
  },
  operatorGroups: operatorGroups as any,
  firstField: firstField,
})

// 获得操作符列表
const getOperators = (type: any) => {
  const ops = state.operatorGroups[type || 'string']
  return ops && ops.length > 0 ? ops : ops['string']
}

// 更改字段
const onChangeField = (data: any) => {
  data.value = ''
  data.type = ''

  const field = props.fields.find((a: any) => a.field === data.field) as any
  mergeWith(data, field)

  data.componentName = data.componentName || 'el-input'

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

  if (data.type === 'date') {
    let dateType = 'date'
    if (data.operator === Operator.dateRange.value) {
      dateType = dateType + 'range'
    }
    data.attrs.type = data.attrs.type ? data.attrs.type : dateType
    if (data.attrs.type.indexOf('range') >= 0) {
      data.operator = Operator.dateRange.value
    }
  }
}

// 更改操作符
const onChangeOperator = (data: any) => {
  data.value = ''
  if (data.type === 'date') {
    if (data.operator === Operator.dateRange.value) {
      data.attrs.type = data.attrs.type + 'range'
    } else {
      if (data.attrs.type.indexOf('range') >= 0) {
        data.attrs.type = data.attrs.type.replace(/range$/, '')
      }
    }
  }
}

// 添加分组
const onAddGroup = (data: any) => {
  if (data) {
    if (!data.filters) {
      data.filters = []
    }
    data.filters.push({
      logic: 'And',
      filters: [
        {
          field: '',
          operator: '',
          value: '',
          type: '',
          componentName: 'el-input',
          attrs: {
            placeholder: '请输入字段值',
          },
        },
      ],
    })
  } else {
    state.dataTree = [
      {
        root: true,
        logic: 'And',
        filters: [
          {
            field: '',
            operator: '',
            value: '',
            type: '',
            componentName: 'el-input',
            attrs: {
              placeholder: '请输入字段值',
            },
          },
        ],
      },
    ]
  }
}

// 添加条件
const onAddCondition = (data: any) => {
  const newFilter = {
    field: '',
    operator: '',
    value: '',
    type: '',
    componentName: 'el-input',
    attrs: {
      placeholder: '请输入字段值',
    },
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

  return newFilter
}

// 删除分组或条件
const onDelete = (node: any, data: any) => {
  const parent = node.parent
  const filters = parent.data.filters || parent.data
  const index = filters.findIndex((d: any) => d === data)
  filters.splice(index, 1)

  // 如果删除后父节点没有任何条件，则删除父节点
  if (filters.length === 0 && parent.parent) {
    const grandParent = parent.parent
    const parentFilters = grandParent.data.filters || grandParent.data
    const parentIndex = parentFilters.findIndex((d: any) => d === parent.data)
    parentFilters.splice(parentIndex, 1)
  }
}

// 重置
const reset = () => {
  state.dataTree = []
}

// 获取动态过滤条件
const getDynamicFilter = () => {
  return state.dataTree.length > 0 ? cloneDeep(state.dataTree[0]) : null
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
  .el-tree-node__expand-icon.is-leaf {
    display: none;
  }
}
</style>
