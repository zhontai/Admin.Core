<template>
  <el-form>
    <el-form-item>
      <el-radio v-model="radioValue" :label="1"> 日，允许的通配符[, - * ? / L W] </el-radio>
    </el-form-item>

    <el-form-item>
      <el-radio v-model="radioValue" :label="2"> 不指定 </el-radio>
    </el-form-item>

    <el-form-item>
      <el-radio v-model="radioValue" :label="3">
        周期从
        <el-input-number v-model="cycle01" :min="1" :max="30" /> - <el-input-number v-model="cycle02" :min="cycle01 + 1" :max="31" /> 日
      </el-radio>
    </el-form-item>

    <el-form-item>
      <el-radio v-model="radioValue" :label="4">
        从
        <el-input-number v-model="average01" :min="1" :max="30" /> 号开始，每
        <el-input-number v-model="average02" :min="1" :max="31 - average01" /> 日执行一次
      </el-radio>
    </el-form-item>

    <el-form-item>
      <el-radio v-model="radioValue" :label="5">
        每月
        <el-input-number v-model="workday" :min="1" :max="31" /> 号最近的那个工作日
      </el-radio>
    </el-form-item>

    <el-form-item>
      <el-radio v-model="radioValue" :label="6"> 本月最后一天 </el-radio>
    </el-form-item>

    <el-form-item>
      <el-radio v-model="radioValue" :label="7"> 指定 </el-radio>
      <el-select clearable v-model="checkboxList" placeholder="可多选" multiple :multiple-limit="10">
        <el-option v-for="item in 31" :key="item" :label="item" :value="item" />
      </el-select>
    </el-form-item>
  </el-form>
</template>
<script lang="ts" setup>
import { ref, computed, watch, nextTick } from 'vue'
const emit = defineEmits(['update'])
const props = defineProps({
  cron: {
    type: Object,
    default: () => {
      return {
        second: '*',
        min: '*',
        hour: '*',
        day: '*',
        month: '*',
        week: '?',
        year: '',
      }
    },
  },
  check: {
    type: Function,
    default: () => {},
  },
})
const radioValue = ref(1)
const cycle01 = ref(1)
const cycle02 = ref(2)
const average01 = ref(1)
const average02 = ref(1)
const workday = ref(1)
const checkboxList = ref([])
const checkCopy = ref([1])
const cycleTotal = computed(() => {
  return cycle01.value + '-' + cycle02.value
})
const averageTotal = computed(() => {
  return average01.value + '/' + average02.value
})
const workdayTotal = computed(() => {
  return workday.value + 'W'
})
const checkboxString = computed(() => {
  return checkboxList.value.join(',')
})
watch([cycle01, cycle02], () => {
  cycle01.value = props.check(cycle01.value, 1, 30)
  cycle02.value = props.check(cycle02.value, cycle01.value + 1, 31)
})
watch([average01, average02], () => {
  average01.value = props.check(average01.value, 1, 30)
  average02.value = props.check(average02.value, 1, 31 - average01.value)
})
watch([workday], () => {
  workday.value = props.check(workday.value, 1, 31)
})
watch(
  () => props.cron.day,
  (value) => {
    nextTick(() => {
      changeRadioValue(value)
    })
  },
  {
    deep: true,
    immediate: true,
  }
)
watch([radioValue, cycleTotal, averageTotal, workdayTotal, checkboxString], () => onRadioChange())
const changeRadioValue = (value: any) => {
  if (value === '*') {
    radioValue.value = 1
  } else if (value === '?') {
    radioValue.value = 2
  } else if (value.indexOf('-') > -1) {
    const indexArr = value.split('-')
    cycle01.value = Number(indexArr[0])
    cycle02.value = Number(indexArr[1])
    radioValue.value = 3
  } else if (value.indexOf('/') > -1) {
    const indexArr = value.split('/')
    average01.value = Number(indexArr[0])
    average02.value = Number(indexArr[1])
    radioValue.value = 4
  } else if (value.indexOf('W') > -1) {
    const indexArr = value.split('W')
    workday.value = Number(indexArr[0])
    radioValue.value = 5
  } else if (value === 'L') {
    radioValue.value = 6
  } else {
    checkboxList.value = [...new Set(value.split(',').map((item: any) => Number(item)))] as never[]
    radioValue.value = 7
  }
}
// 单选按钮值变化时
const onRadioChange = () => {
  if (radioValue.value === 2 && props.cron.week === '?') {
    emit('update', 'week', '*', 'day')
  }
  if (radioValue.value !== 2 && props.cron.week !== '?') {
    emit('update', 'week', '?', 'day')
  }
  switch (radioValue.value) {
    case 1:
      emit('update', 'day', '*', 'day')
      break
    case 2:
      emit('update', 'day', '?', 'day')
      break
    case 3:
      emit('update', 'day', cycleTotal.value, 'day')
      break
    case 4:
      emit('update', 'day', averageTotal.value, 'day')
      break
    case 5:
      emit('update', 'day', workdayTotal.value, 'day')
      break
    case 6:
      emit('update', 'day', 'L', 'day')
      break
    case 7:
      if (checkboxList.value.length === 0) {
        checkboxList.value.push(checkCopy.value[0] as never)
      } else {
        checkCopy.value = checkboxList.value
      }
      emit('update', 'day', checkboxString.value, 'day')
      break
  }
}
</script>

<style lang="scss" scoped>
.el-input-number--small,
.el-select,
.el-select--small {
  margin: 0 0.2rem;
}
.el-select,
.el-select--small {
  width: 18.8rem;
}
.el-radio {
  margin-right: 5px;
}
</style>
