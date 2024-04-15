<template>
  <el-date-picker
    v-model="state.dateRange"
    :value-format="timeFormat"
    format="YYYY-MM-DD"
    type="daterange"
    start-placeholder="开始时间"
    end-placeholder="结束时间"
    :shortcuts="state.shortcuts"
    @change="change"
  ></el-date-picker>
</template>

<script lang="ts" setup>
import dayjs from 'dayjs'
import { reactive, watch, defineEmits, ref } from 'vue'

const emit = defineEmits(['update:startDate', 'update:endDate'])

const props = defineProps({
  startDate: {
    type: String,
    default: '',
  },
  endDate: {
    type: String,
    default: '',
  },
})

const timeFormat = ref('YYYY-MM-DD').value

const state = reactive({
  dateRange: [props.startDate, props.endDate],
  shortcuts: [
    {
      text: '最近一年',
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(1, 'years').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近半年',
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(6, 'months').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近三月',
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(3, 'months').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近一月',
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(1, 'months').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近七天',
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(7, 'days').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近三天',
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(3, 'days').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '今天',
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().startOf('day').format(timeFormat)
        return [start, end]
      },
    },
  ],
})

const change = (value: any) => {
  let start = ''
  let end = ''

  if (value) {
    end = dayjs(value[1]).endOf('day').format(timeFormat)
    start = dayjs(value[0]).startOf('day').format(timeFormat)

    state.dateRange[0] = start
    state.dateRange[1] = end
  }

  emit('update:startDate', start)
  emit('update:endDate', end)
}

watch(
  () => props.startDate,
  async (newValue) => {
    if (newValue == '' || newValue == null) {
      state.dateRange = ['', '']
      emit('update:startDate', '')
      emit('update:endDate', '')
    } else state.dateRange = [newValue, state.dateRange[1]]
  }
)

watch(
  () => props.endDate,
  async (newValue) => {
    if (newValue == '' || newValue == null) {
      state.dateRange = ['', '']
      emit('update:startDate', '')
      emit('update:endDate', '')
    } else state.dateRange = [state.dateRange[0], newValue]
  }
)
</script>
