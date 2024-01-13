<template>
  <el-date-picker
    v-model="dateRange"
    format="YYYY-MM-DD"
    type="daterange"
    start-placeholder="开始时间"
    end-placeholder="结束时间"
    :clearable="false"
    :shortcuts="shortcuts"
    @change="change"
  ></el-date-picker>
</template>

<script lang="ts">
import dayjs from 'dayjs'
import { defineComponent, reactive, toRefs } from 'vue'

export default defineComponent({
  name: 'MyDateRange',
  setup() {
    const state = reactive({
      dateRange: [dayjs().subtract(1, 'months').startOf('day'), dayjs().endOf('day')],
      shortcuts: [
        {
          text: '最近一年',
          value: () => {
            const end = dayjs().endOf('day')
            const start = dayjs().subtract(1, 'years').startOf('day')
            return [start, end]
          },
        },
        {
          text: '最近半年',
          value: () => {
            const end = dayjs().endOf('day')
            const start = dayjs().subtract(6, 'months').startOf('day')
            return [start, end]
          },
        },
        {
          text: '最近三月',
          value: () => {
            const end = dayjs().endOf('day')
            const start = dayjs().subtract(3, 'months').startOf('day')
            return [start, end]
          },
        },
        {
          text: '最近一月',
          value: () => {
            const end = dayjs().endOf('day')
            const start = dayjs().subtract(1, 'months').startOf('day')
            return [start, end]
          },
        },
        {
          text: '最近七天',
          value: () => {
            const end = dayjs().endOf('day')
            const start = dayjs().subtract(7, 'days').startOf('day')
            return [start, end]
          },
        },
        {
          text: '最近三天',
          value: () => {
            const end = dayjs().endOf('day')
            const start = dayjs().subtract(3, 'days').startOf('day')
            return [start, end]
          },
        },
        {
          text: '今天',
          value: () => {
            const end = dayjs().endOf('day')
            const start = dayjs().startOf('day')
            return [start, end]
          },
        },
      ],
    })

    const change = (value: any) => {
      const end = dayjs(value[1]).endOf('day')
      const start = dayjs(value[0]).startOf('day')
      state.dateRange[0] = start
      state.dateRange[1] = end
    }

    return {
      change,
      ...toRefs(state),
    }
  },
})
</script>
