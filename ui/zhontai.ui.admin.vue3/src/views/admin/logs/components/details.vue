<template>
  <el-drawer
    v-model="state.showDialog"
    title="操作日志详细信息"
    direction="rtl"
    destroy-on-close
    size="600"
    :append-to-body="true"
    :lock-scroll="false"
    resizable
  >
    <div class="my-fill h100" style="padding: 12px">
      <el-descriptions class="margin-top" :column="1" border>
        <el-descriptions-item label="操作名称" label-class-name="label">{{ state.details.apiLabel }}</el-descriptions-item>
        <el-descriptions-item label="操作接口" label-class-name="label">{{ state.details.apiPath }}</el-descriptions-item>
        <el-descriptions-item label="操作状态" label-class-name="label"
          ><el-tag :type="state.details.status ? 'success' : 'danger'" disable-transitions>{{
            state.details.status ? '成功' : '失败'
          }}</el-tag></el-descriptions-item
        >
        <el-descriptions-item label="请求方法" label-class-name="label">{{ state.details.apiMethod }}</el-descriptions-item>
        <el-descriptions-item label="响应代码	" label-class-name="label">{{ state.details.statusCode }}</el-descriptions-item>
        <el-descriptions-item label="IP地址" label-class-name="label">{{ state.details.ip }} {{ state.details.isp }}</el-descriptions-item>
        <el-descriptions-item label="IP所在地" label-class-name="label"
          >{{ state.details.country }} {{ state.details.province }} {{ state.details.city }}
        </el-descriptions-item>
        <el-descriptions-item label="浏览器信息" label-class-name="label"
          >{{ state.details.os }} {{ state.details.browser }} {{ state.details.device }}
        </el-descriptions-item>
        <el-descriptions-item label="耗时ms" label-class-name="label">{{ state.details.elapsedMilliseconds }}</el-descriptions-item>
        <el-descriptions-item label="操作账号" label-class-name="label">{{ state.details.createdUserName }}</el-descriptions-item>
        <el-descriptions-item label="操作人员" label-class-name="label">{{ state.details.createdUserRealName }}</el-descriptions-item>
        <el-descriptions-item label="创建时间" label-class-name="label">{{
          dayjs(state.details.createdTime).format('YYYY-MM-DD HH:mm:ss')
        }}</el-descriptions-item>
      </el-descriptions>

      <el-collapse class="mt12" v-model="state.activeName">
        <el-collapse-item title="请求参数" name="params">
          <MyJsonEditor
            ref="jsonEditorRef"
            v-model="state.details.params"
            :options="{
              mainMenuBar: false,
              statusBar: false,
              onEditable: () => false,
            }"
            style="height: 400px !important"
          ></MyJsonEditor>
        </el-collapse-item>
        <el-collapse-item title="响应结果" name="result">
          <MyJsonEditor
            ref="jsonEditorRef"
            v-model="state.details.result"
            :options="{
              mainMenuBar: false,
              statusBar: false,
              onEditable: () => false,
            }"
            style="height: 200px !important"
          ></MyJsonEditor>
        </el-collapse-item>
      </el-collapse>
    </div>
  </el-drawer>
</template>

<script lang="ts" setup name="operation-log-details">
import { OperationLogGetPageOutput } from '/@/api/admin/data-contracts'
import dayjs from 'dayjs'
import MyJsonEditor from '/@/components/my-json-editor/index.vue'

const state = reactive({
  showDialog: false,
  details: {} as OperationLogGetPageOutput,
  activeName: ['params', 'result'],
})

// 打开对话框
const open = (row: OperationLogGetPageOutput) => {
  state.showDialog = true
  state.details = row
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
:deep() {
  .el-descriptions .label {
    width: 120px;
  }
}
</style>
