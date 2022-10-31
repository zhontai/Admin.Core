<template>
  <a-layout-content :class="['knife4j-body-content', copyright?'':'knife4j-body-content--no-fotter']">
    <a-tabs :value="activeKey" type="editable-card" class="knife4j-tab">
      <a-tab-pane v-for="pane in panes" :tab="pane.title" :key="pane.key" :closable="pane.closable">
        <component :is="pane.content"></component>
      </a-tab-pane>
    </a-tabs>

  </a-layout-content>

</template>
<script>
export default {
  props: {
    panes: {
      type: Array
    },
    activeKey: {
      type: String
    }
  },
  computed:{
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
    return {};
  },
  created() {},
  methods: {}
};
</script>
