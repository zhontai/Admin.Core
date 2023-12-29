import { markRaw } from 'vue'
import { ElMessage, ElMessageBox, ElNotification, ElLoading, ElMessageBoxOptions } from 'element-plus'
import { Delete } from '@element-plus/icons-vue'
import { i18n } from '/@/i18n/index'

let loadingInstance: any

export default {
  // 消息提示
  msg(content: any) {
    ElMessage.info({
      message: content,
      grouping: true,
    })
  },
  // 错误消息
  msgError(content: any) {
    ElMessage.error({
      message: content,
      grouping: true,
    })
  },
  // 成功消息
  msgSuccess(content: any) {
    ElMessage.success({
      message: content,
      grouping: true,
    })
  },
  // 警告消息
  msgWarning(content: any) {
    ElMessage.warning({
      message: content,
      grouping: true,
    })
  },
  // 弹出提示
  alert(content: any) {
    ElMessageBox.alert(content, i18n.global.t('el.messagebox.title'))
  },
  // 错误提示
  alertError(content: any) {
    ElMessageBox.alert(content, i18n.global.t('el.messagebox.title'), { type: 'error' })
  },
  // 成功提示
  alertSuccess(content: any) {
    ElMessageBox.alert(content, i18n.global.t('el.messagebox.title'), { type: 'success' })
  },
  // 警告提示
  alertWarning(content: any) {
    ElMessageBox.alert(content, i18n.global.t('el.messagebox.title'), { type: 'warning' })
  },
  // 通知提示
  notify(content: any) {
    ElNotification.info(content)
  },
  // 错误通知
  notifyError(content: any) {
    ElNotification.error(content)
  },
  // 成功通知
  notifySuccess(content: any) {
    ElNotification.success(content)
  },
  // 警告通知
  notifyWarning(content: any) {
    ElNotification.warning(content)
  },
  // 确认窗体
  confirm(content: any, elMessageBoxOptions: ElMessageBoxOptions) {
    return ElMessageBox.confirm(content, i18n.global.t('el.messagebox.title'), {
      confirmButtonText: i18n.global.t('el.messagebox.confirm'),
      cancelButtonText: i18n.global.t('el.messagebox.cancel'),
      type: 'warning',
      ...elMessageBoxOptions,
    })
  },
  // 确认删除窗体
  confirmDelete(content: any, elMessageBoxOptions: ElMessageBoxOptions) {
    return ElMessageBox.confirm(content, i18n.global.t('el.messagebox.title'), {
      confirmButtonText: i18n.global.t('el.messagebox.confirm'),
      cancelButtonText: i18n.global.t('el.messagebox.cancel'),
      type: 'warning',
      icon: markRaw(Delete),
      ...elMessageBoxOptions,
    })
  },
  // 提交内容
  prompt(content: any, elMessageBoxOptions: ElMessageBoxOptions) {
    return ElMessageBox.prompt(content, i18n.global.t('el.messagebox.title'), {
      confirmButtonText: i18n.global.t('el.messagebox.confirm'),
      cancelButtonText: i18n.global.t('el.messagebox.cancel'),
      type: 'warning',
      ...elMessageBoxOptions,
    })
  },
  // 打开遮罩层
  loading(content: any) {
    loadingInstance = ElLoading.service({
      text: content,
    })
  },
  // 关闭遮罩层
  closeLoading() {
    loadingInstance.close()
  },
}
