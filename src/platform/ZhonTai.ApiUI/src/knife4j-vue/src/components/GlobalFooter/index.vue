<template>
  <div v-if="copyright" class="globalFooter">
    <a-row v-if="settings.enableFooter">
      <div class="copyright" v-html="copyright"></div>
    </a-row>
    <a-row v-else>
      <Markdown
        v-if="settings.enableFooterCustom"
        :source="settings.footerCustomContent"
      />
    </a-row>
  </div>
</template>
<script>
export default {
  name: 'GlobalFooter',
  props: {
    links: {
      type: Array,
      default: () => {
        ;[]
      },
    },
  },
  components: {
    Markdown: () => import('@/components/Markdown'),
  },
  computed: {
    settings() {
      return this.$store.state.globals.settings
    },
    copyright() {
      const servers = this.$store.state.globals.swaggerCurrentInstance
        ?.swaggerData?.servers
      if (servers && servers.length > 0) {
        return this.$store.state.globals.swaggerCurrentInstance.swaggerData
          .servers[0].extensions?.copyright
      } else {
        return ''
      }
    },
  },
  data() {
    return {}
  },
}
</script>

<style lang="less" scoped>
@import './index.less';
</style>
