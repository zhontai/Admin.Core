import { createApp } from 'vue'
import pinia from '/@/stores/index'
import App from '/@/App.vue'
import router from '/@/router'
import { directive } from '/@/directive/index'
import { i18n } from '/@/i18n/index'
import other from '/@/utils/other'

import ElementPlus from 'element-plus'
import 'virtual:svg-icons-register'
import '/@/theme/index.scss'
import VueGridLayout from 'vue-grid-layout'
import globalProperties from '/@/globalProperties'

const app = createApp(App)

directive(app)
other.elSvg(app)

app.use(pinia).use(router).use(ElementPlus).use(i18n).use(VueGridLayout).use(globalProperties).mount('#app')
