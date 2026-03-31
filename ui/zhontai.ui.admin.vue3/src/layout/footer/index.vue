<template>
  <div class="layout-footer pb15">
    <div class="layout-footer-warp">
      <div>{{ t('中台Admin') }}</div>
      <div class="mt5">{{ t('Copyright © 2022 中台Admin All rights reserved.') }}</div>
    </div>
  </div>
</template>

<script setup lang="ts" name="layoutFooter">
// 此处需有内容（注释也得），否则缓存将失败
import { t } from '/@/i18n'
</script>

<style scoped lang="scss">
.layout-footer {
  width: 100%;
  display: flex;
  &-warp {
    margin: auto;
    color: var(--el-text-color-secondary);
    text-align: center;
    animation: error-num 0.3s ease;
  }
}
</style>
