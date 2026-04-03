<template>
  <template v-for="val in chils">
    <el-sub-menu :class="menuSize" :index="val.path" :key="val.path" v-if="val.children && val.children.length > 0">
      <template #title>
        <SvgIcon :name="val.meta.icon" />
        <span class="my-line-1">{{ $t(val.meta.title) }}</span>
      </template>
      <sub-item :chil="val.children" />
    </el-sub-menu>
    <template v-else>
      <el-menu-item :index="val.path" :key="val.path">
        <template v-if="!val.meta.isLink || (val.meta.isLink && val.meta.isIframe)">
          <SvgIcon :name="val.meta.icon" />
          <!-- <span class="my-line-1">{{ $t(val.meta.title) }}</span> -->
          <el-text truncated>{{ $t(val.meta.title) }}</el-text>
        </template>
        <template v-else>
          <a class="w100" @click.prevent="onALinkClick(val)">
            <SvgIcon :name="val.meta.icon" />
            {{ $t(val.meta.title) }}
          </a>
        </template>
      </el-menu-item>
    </template>
  </template>
</template>

<script setup lang="ts" name="navMenuSubItem">
import { RouteRecordRaw } from 'vue-router'
import other from '/@/utils/other'
import { useThemeConfig } from '/@/stores/themeConfig'

const storesThemeConfig = useThemeConfig()
const { themeConfig } = storeToRefs(storesThemeConfig)

// 定义父组件传过来的值
const props = defineProps({
  // 菜单列表
  chil: {
    type: Array<RouteRecordRaw>,
    default: () => [],
  },
})

const menuSize = computed(() => {
  return 'el-menu--' + themeConfig.value.globalComponentSize
})

// 获取父级菜单数据
const chils = computed(() => {
  return <RouteItems>props.chil
})
// 打开外部链接
const onALinkClick = (val: RouteItem) => {
  other.handleOpenLink(val)
}
</script>
