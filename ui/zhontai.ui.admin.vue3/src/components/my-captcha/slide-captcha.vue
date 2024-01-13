<template>
  <div class="captcha">
    <div class="captcha__main" :style="imgWrapperStyle">
      <img v-if="state.src" :src="state.src" class="captcha_background" alt="background" ref="backgroundRef" />
      <img
        v-if="state.sliderSrc"
        :src="state.sliderSrc"
        class="captcha_slider"
        alt="slider"
        ref="slider"
        :class="{ goFirst: state.isOk, goKeep: state.isKeep }"
        @mousedown="handleDragStart"
        @touchstart="handleDragStart"
      />
      <div class="captcha_message" v-if="state.showVerifyTip">
        <div class="captcha_message__icon">
          <svg v-if="state.isPassing" width="28" height="28" viewBox="0 0 28 28" xmlns="http://www.w3.org/2000/svg">
            <g stroke="#fff" stroke-width="1.5" fill="none" fill-rule="evenodd" stroke-linecap="round" stroke-linejoin="round">
              <path
                d="M22.776 4.073A13.2 13.2 0 0 0 14 .75C6.682.75.75 6.682.75 14S6.682 27.25 14 27.25 27.25 21.318 27.25 14c0-.284-.009-.566-.027-.845"
              ></path>
              <path d="M7 12.5l7 7 13-13"></path>
            </g>
          </svg>
          <svg v-else width="28" height="28" viewBox="0 0 28 28" xmlns="http://www.w3.org/2000/svg">
            <g stroke="#fff" stroke-width="1.5" fill="none" fill-rule="evenodd">
              <circle cx="14" cy="14" r="13.25"></circle>
              <path stroke-linecap="round" stroke-linejoin="round" d="M8.75 8.75l10.5 10.5M19.25 8.75l-10.5 10.5"></path>
            </g>
          </svg>
        </div>
        <div class="captcha_message__text">{{ state.isPassing ? successTip : failTip }}</div>
      </div>
      <div class="captcha_message loadding" v-if="state.showGenerateLoadding">
        <div class="captcha_message__icon captcha_message__icon--loadding"></div>
        <div class="captcha_message__text">加载中...</div>
      </div>
      <div class="captcha_message" v-if="state.showVerifyLoadding">
        <div class="captcha_message__icon captcha_message__icon--loadding"></div>
        <div class="captcha_message__text"></div>
      </div>
    </div>
    <div class="captcha__bar" :style="dragVerifyStyle" ref="dragVerify">
      <div class="captcha_progress_bar" :class="{ goFirst2: state.isOk }" ref="progressBar" :style="progressBarStyle"></div>
      <div class="captcha_progress_bar__text" :style="textStyle">{{ state.tracks.length > 0 || state.isPassing ? '' : text }}</div>
      <div
        class="captcha_handler"
        :class="{ goFirst: state.isOk }"
        :style="handlerStyle"
        ref="handler"
        @mousedown="handleDragStart"
        @touchstart="handleDragStart"
      >
        <svg v-if="state.isPassing" width="16" height="16" viewBox="0 0 28 28" xmlns="http://www.w3.org/2000/svg">
          <g stroke="rgb(118, 198, 29)" stroke-width="1.5" fill="none" fill-rule="evenodd" stroke-linecap="round" stroke-linejoin="round">
            <path
              d="M22.776 4.073A13.2 13.2 0 0 0 14 .75C6.682.75.75 6.682.75 14S6.682 27.25 14 27.25 27.25 21.318 27.25 14c0-.284-.009-.566-.027-.845"
            ></path>
            <path d="M7 12.5l7 7 13-13"></path>
          </g>
        </svg>
        <svg v-else :style="handlerSvgStyle" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="819">
          <path
            d="M500.864 545.728a47.744 47.744 0 0 0 6.72-48.896 24.704 24.704 0 0 0-4.48-8.384L240.256 193.088a34.24 34.24 0 0 0-28.608-17.408 34.24 34.24 0 0 0-25.856 12.864 46.592 46.592 0 0 0 0 59.52l238.08 264.512-238.08 264.512a46.592 46.592 0 0 0-1.088 59.52 32 32 0 0 0 50.56 0l265.6-290.88z"
            p-id="820"
          ></path>
          <path
            d="M523.84 248.064l236.992 264.512-238.08 264.512a46.592 46.592 0 0 0 0 59.52 32 32 0 0 0 50.56 0l265.6-292.608a47.744 47.744 0 0 0 6.72-48.832 24.704 24.704 0 0 0-4.48-8.448L578.304 191.36a34.24 34.24 0 0 0-55.552-2.816 46.592 46.592 0 0 0 1.088 59.52z"
            p-id="821"
          ></path>
        </svg>
      </div>
    </div>
    <div class="captcha__actions" v-if="showRefresh && !state.isPassing">
      <a class="captcha__action" @click="handleRefresh">
        <svg fill="#FFF" width="20px" height="20px" viewBox="0 0 20 20" version="1.1" xmlns="http://www.w3.org/2000/svg">
          <path
            d="M10,4 C12.0559549,4 13.9131832,5.04358655 15.0015086,6.68322231 L15,5.5 C15,5.22385763 15.2238576,5 15.5,5 C15.7761424,5 16,5.22385763 16,5.5 L16,8.5 C16,8.77614237 15.7761424,9 15.5,9 L12.5,9 C12.2238576,9 12,8.77614237 12,8.5 C12,8.22385763 12.2238576,8 12.5,8 L14.5842317,8.00000341 C13.7999308,6.20218044 12.0143541,5 10,5 C7.23857625,5 5,7.23857625 5,10 C5,12.7614237 7.23857625,15 10,15 C11.749756,15 13.3431487,14.0944653 14.2500463,12.6352662 C14.3958113,12.4007302 14.7041063,12.328767 14.9386423,12.4745321 C15.1731784,12.6202971 15.2451415,12.9285921 15.0993765,13.1631281 C14.0118542,14.9129524 12.0990688,16 10,16 C6.6862915,16 4,13.3137085 4,10 C4,6.6862915 6.6862915,4 10,4 Z"
            fill-rule="nonzero"
          ></path>
        </svg>
      </a>
    </div>
  </div>
</template>

<script lang="ts" setup name="my-slide-captcha">
import { reactive, computed, ref, onMounted, onBeforeMount, onUnmounted } from 'vue'

const props = defineProps({
  width: {
    type: [Number, String],
    default: 340,
  },
  height: {
    type: [Number, String],
    default: 212,
  },
  barHeight: {
    type: Number,
    default: 40,
  },
  handlerIconWidth: {
    type: Number,
    default: 16,
  },
  handlerIconHeigth: {
    type: Number,
    default: 16,
  },
  background: {
    type: String,
    default: '#eee',
  },
  circle: {
    type: Boolean,
    default: false,
  },
  radius: {
    type: String,
    default: '4px',
  },
  text: {
    type: String,
    default: '按住滑块拖动',
  },
  progressBarBg: {
    type: String,
    default: '#76c61d',
  },
  successTip: {
    type: String,
    default: '验证通过',
  },
  failTip: {
    type: String,
    default: '验证未通过，拖动滑块将悬浮图像正确合并',
  },
  showRefresh: {
    type: Boolean,
    default: true,
  },
})

const emits = defineEmits(['finish', 'refresh'])

const backgroundRef = ref()
const slider = ref()
const dragVerify = ref()
const progressBar = ref()
const handler = ref()

const state = reactive({
  isMoving: false,
  x: 0,
  y: 0,
  isOk: false,
  isKeep: false,
  isFinish: false,
  tracks: [],
  startSlidingTime: new Date(),
  showVerifyTip: false,
  showVerifyLoadding: false,
  showGenerateLoadding: false,
  src: '',
  sliderSrc: '',
  isPassing: false,
  width: 340,
})

const imgWrapperStyle = computed(() => {
  return {
    width: props.width + 'px',
    height: props.height + 'px',
    //position: 'relative',
    overflow: 'hidden',
  }
})
const dragVerifyStyle = computed(() => {
  return {
    width: props.width + 'px',
    height: props.barHeight + 'px',
    lineHeight: props.barHeight + 'px',
    background: props.background,
    borderRadius: props.circle ? props.barHeight / 2 + 'px' : props.radius,
  }
})
const progressBarStyle = computed(() => {
  return {
    background: props.progressBarBg,
    height: props.barHeight + 'px',
    borderRadius: props.circle ? props.barHeight / 2 + 'px 0 0 ' + props.barHeight / 2 + 'px' : props.radius,
  }
})
const textStyle = computed(() => {
  return {
    height: props.barHeight + 'px',
    width: props.width + 'px',
  }
})
const handlerStyle = computed(() => {
  return {
    width: props.barHeight + 'px',
    height: props.barHeight + 'px',
  }
})
const handlerSvgStyle = computed(() => {
  return {
    width: props.handlerIconWidth + 'px',
    height: props.handlerIconHeigth + 'px',
  }
})

onMounted(() => {
  const dragEl = dragVerify.value
  dragEl.style.setProperty('--textColor', '#333')
  let width = dragEl.clientWidth
  width = width > 0 ? width : state.width
  dragEl.style.setProperty('--width', Math.floor(width / 2) + 'px')
  dragEl.style.setProperty('--pwidth', -Math.floor(width / 2) + 'px')
  document.documentElement.style.setProperty('--my-captcha-width', width + 'px')
  handleRefresh()
})

const onLayoutResize = () => {
  const width = dragVerify.value?.clientWidth
  if (width > 0) document.documentElement.style.setProperty('--my-captcha-width', width + 'px')
}

// 页面加载前
onBeforeMount(() => {
  document.documentElement.style.setProperty('--my-captcha-width', state.width + 'px')
  window.addEventListener('resize', onLayoutResize)
})
// 页面卸载时
onUnmounted(() => {
  window.removeEventListener('resize', onLayoutResize)
})

// 开始请求生成图片时调用
const startRequestGenerate = () => {
  reset()
  state.showGenerateLoadding = true
}
// 结束请求生成图片时调用
const endRequestGenerate = (src: string, sliderSrc: string) => {
  state.showGenerateLoadding = false
  state.src = src
  state.sliderSrc = sliderSrc
}
// 开始请求校验时调用
const startRequestVerify = () => {
  state.showVerifyLoadding = true
}
// 结束请求校验时调用
const endRequestVerify = (isPassing: boolean) => {
  state.isPassing = isPassing
  state.showVerifyLoadding = false
  state.showVerifyTip = true
}
// 重置
const reset = () => {
  state.x = 0
  state.y = 0
  state.tracks = []
  state.isMoving = false
  state.isFinish = false
  state.showGenerateLoadding = false
  state.showVerifyLoadding = false
  state.showVerifyTip = false
  state.isPassing = false

  if (progressBar.value) progressBar.value.style.width = 0
  if (slider.value) slider.value.style.left = 0
  if (handler.value) handler.value.style.left = 0
}

//解绑事件
const removeEventListeners = () => {
  window.removeEventListener('touchmove', handleDragMoving)
  window.removeEventListener('touchend', handleDragFinish)
  window.removeEventListener('mousemove', handleDragMoving)
  window.removeEventListener('mouseup', handleDragFinish)
}
//开始拖拽
const handleDragStart = (e: any) => {
  e?.preventDefault()
  if (!state.isPassing && state.src && state.sliderSrc && !state.isFinish) {
    window.addEventListener('touchmove', handleDragMoving)
    window.addEventListener('touchend', handleDragFinish)
    window.addEventListener('mousemove', handleDragMoving)
    window.addEventListener('mouseup', handleDragFinish)

    state.isMoving = true
    state.startSlidingTime = new Date()
    state.x = e.touches ? e.touches[0].pageX : e.clientX
    state.y = e.touches ? e.touches[0].pageY : e.clientY
    state.width = dragVerify.value.clientWidth
  }
}
//拖拽中
const handleDragMoving = (e: any) => {
  e?.preventDefault()
  if (state.isMoving && !state.isPassing && state.src && state.sliderSrc && !state.isFinish) {
    var _x = (e.touches ? e.touches[0].pageX : e.clientX) - state.x
    if (_x > 0 && _x <= state.width - props.barHeight) {
      var _y = (e.touches ? e.touches[0].pageY : e.clientY) - state.y

      handler.value.style.left = _x + 'px'
      progressBar.value.style.width = _x + props.barHeight / 2 + 'px'
      slider.value.style.left = _x + 'px'

      state.tracks.push({
        x: Math.round(_x),
        y: Math.round(_y),
        t: new Date().getTime() - state.startSlidingTime.getTime(),
      } as never)
    }
    if (_x <= 0) {
      handler.value.style.left = '0px'
      progressBar.value.style.width = '0px'
    }
    if (_x > state.width - props.barHeight) {
      handler.value.style.left = state.width - props.barHeight + 'px'
      progressBar.value.style.width = state.width + props.barHeight / 2 + 'px'
    }
  }
}
//拖拽结束
const handleDragFinish = (e: any) => {
  e?.preventDefault()
  if (state.isMoving && !state.isPassing && state.src && state.sliderSrc && !state.isFinish) {
    state.isMoving = false
    state.isFinish = true
    removeEventListeners()
    if (state.tracks.length > 0) {
      emits('finish', {
        backgroundImageWidth: backgroundRef.value.offsetWidth,
        backgroundImageHeight: backgroundRef.value.offsetHeight,
        sliderImageWidth: slider.value.offsetWidth,
        sliderImageHeight: slider.value.offsetHeight,
        startTime: state.startSlidingTime,
        endTime: new Date(),
        tracks: state.tracks,
      })
    } else {
      reset()
    }
  }
}
//刷新
const handleRefresh = () => {
  reset()
  emits('refresh')
}
//导出方法
defineExpose({
  startRequestGenerate,
  endRequestGenerate,
  startRequestVerify,
  endRequestVerify,
  handleRefresh,
})
</script>

<style scoped lang="scss">
.captcha {
  position: relative;
  user-select: none;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.captcha__main {
  width: 100%;
  height: calc(var(--my-captcha-width) * (43 / 69));
  position: relative;
  background: rgb(244, 245, 246);
}
.captcha_background {
  width: 100%;
}
.captcha_slider {
  position: absolute;
  top: 0;
  left: 0;
  display: block;
  height: 100%;
}
.captcha_message {
  position: absolute;
  left: 0px;
  top: 0px;
  width: 100%;
  height: 100%;
  z-index: 999999;
  background-color: rgba(34, 34, 34, 0.85);
  display: flex;
  -webkit-box-pack: center;
  justify-content: center;
  -webkit-box-align: center;
  align-items: center;
  flex-direction: column;
}
.captcha_message__icon {
  width: 28px;
  height: 28px;
  margin: 0px auto;
}
.captcha_message__icon--loadding {
  border-radius: 50%;
  width: 24px;
  height: 24px;
  animation: 1s linear 0s infinite normal none running turn;
  background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEgAAABICAYAAABV7bNHAAAAAXNSR0IArs4c6QAADw5JREFUeAHlnHts1WcZx8/vd0rpHSjtBAcMGCABgY1xKXeqYpQwgzHjDxPnXAzRaTRZTPxjMbBETabxL40mBLPExGVhcXMxME1wEOQy7hBGgFG5jCKutKWlLb33+Pk+57w/f6ftgdNy2rWHN3n7vLff+z7P9/c8z3s5769eZBhDLBbzjxw5MtP3/RnEKQw9BTqpq6uryPO8AtL5PT09UcpbFaPR6D2e+YT0Dcqrqa9asmTJZdr2UDYswRvqUQ4fPlyKoCsQaiFjfZ5YhNAeZZ4oeQ/hfVFCRAABROQ+bVqoO0v7k+3t7fvXrl17m/SQhSEB6Pz587ktLS0VcL2OuIjoS3qEFzFgHEBkI93d3VFRgv4YWKkAUhsFUdooc4L0e7W1tfs2btzYTjqjIaMAXb16Ne/27dsb4PBZmB8PDYQJpyU8eT9BA1Akd1iD0KRwGwNFHfJcACJNnCbWU/UG8S+Y4T36z0jICEAwHD127NhXYG4zAhaHmKYo/rbhNkiHAaKtgRJvlqxB1A0EIAOQce4SX0eD36ysrOx6WJQeGqDjx49/Dia+AzDTnEb0B4oYpVxvXiHQArV1GqFyYlg77guQ2isk+gvSysPPFfD95dKlS0+TH3QYNEAIlcOM9E2Y2CDO8Cliwql7mJr/UR1Bwnfy7AXiFYS4OWbMmOqxY8fe4Y235uTkaPaKjBs3Lv/OnTsF5EuZ4aZRNJX2c6GL6COPdBjEYKxE/waY+iEo/SY8/gaz67SSAf4ZFECHDh16LDc39yWc60yYshkHYZKccII50w7aNJH/F22OAsrlwTJ74sSJMQg7r6OjoxL6ZYAqpW8DhLRHWaBxyouHRP0F0j9ZtmzZDeiAwoAB+uCDD+bBxw9hoICRgrcnDTJu4NfNSoBxCVD+tnjx4jPUdQ+Iswc03rdvX05xcXEFGvY8fT8tXuArAMvxlgBIpIX4Y17O0Qd0nVQ9IIBOnTq1pLOz83uAo2lZZhVzjFBmb49iOd2PAGYXzFxMGm2IMqdPn34KoL4PD0uJTnPMDB1/ogAo8/4pmvT3dFlJGyBmqbUM8C20I1jTAILA0MARGNQCr5GB34CBw+kykMl2+MSv8tJeBoRyeOljbgJPL5WwHR53pTN2WgAdPXp0CR1vpWMfQNSvPUeZ3pKCB0AfAtjv0ZqMrUHiXQ/s78GDB4tx/K+hyWvEl56G5/gMQqFwS/T4MjPce4l0SvJAgHCMcxngR/SgQdSeYcwxmq9RzwD1j927d+/avn37sO2RNG6qoBfJ8uNl6l9QG9g1gJSUBYjSpovZ80X84xG1SRXuC9CBAwfKma1eYYA8BwodmdYozyDdkD/xJg6lGuDTLEfzNzP+q2hNLryaCG4CoVz5Zuq+htZ/nIpPZyJ96jVL5OXlfZdO8qik/3hwDcmp81+NVHDE5/Lly/8KIC/A6x3ksOWIKAFi+RKs43faO6qwv5ASoIKCgq/zwOOAYMt1qNmu8nTeDv0tyP+7v05HUtnKlStPw+8PAAKsuvEG0Qi8WwQ4WcGCpqamV1Lx3C9A+J0n6WStHqIDwQ3BsG1iiMnn/Hk0gOOE1nYDGX4mYBBDwmhRq9lYTSDet/FZy1z7MO0D0K5du6I8vIVZSZ3FSAuQHoFDlBPe+yDHFh5gpKSZ1t9Gjj8KGHgykJw2iSLqL+RWevPbB6CZM2eu5YHPJACRZxNIFlHR88xW7/TuZLTkn3nmmdfg9QDRZmPkkjJEkSsHgOYWFRW92FuWJIAuX748lgc28EBgVoBFUY/yjWjV6yNlKu8tSDp5ZOgBCC1ZahNaQ1F8IldCdWfPni0M95UEUH19/SoqdS4cmJWeU574Dg7PdtvhDkZbuqKi4i4y/RwwTHZoHKH48qWUY9znwzIFAGmnTON1RJmVXL5AibH3irAWusSUqaPNrAg47XeR7ShapJW1RQQzs4O+hC/S0sZCABC5BTSWernZyvyPfBGd7Yk3z56/yPVr5LL9GjrhcBBI5fiiTU5SV6HN5hKBIZNSpIFF0lUgfsU9kC0UmY4hm44+nOaERdviMgYQzrmEjeYMzEoW1gNYMTlnZaD7XONso8j7B0R0u35N8dqzKa5ndT1J8hpADQ0NC2hoO11pkcAhaG3QXFVVdUkNszG0tra+j7x1aJJz1LaQRFaPI+BnJbMzsSeVARAtBM20SMsXndqyZUtGTwI1zkgJiV893paoCYykJJqchMs68emzrlHldKLAEUgCSOuebhqeUzqbAzLuFiiYm07SdN4lPATQGuX9zZs3TyYxlmiaQ4WSAqvz3r17N7IZHMnGjHUSedsACVzM1Jw2jWN/ttAHOTmjwKyEjgJtr2Xih7eRDvD8+fM7EPd4HBsZjrkjo/xZ4DNjlQkQVEu1gQ8CuOqRLlym+EN7ziC/mRh9SmlsjwYsc3xmrIkUSmNoYzOXrYMoH9JbE5kSLhP9AEQV0dZDoshu50X0PUdz/3gQNMccHoyGteF8NqdRDt05koj2x8kKLk/kYGI6rzWAwkDx62Wza5jtFI3RJS235HFrIdHxOSCXS3QABVjk5+dn/K5N0PkIS7AhbwYD550NIGWJxZr3TYPIxHBOUiYLHC49MgAx1evugPNBbpqXU9YWLEfXvuydYmIj7N0OKzvag1lweEB9mVgbKpPvKh09efLkWNKf6q+kjpehps3NzcXgEDho8HBDNudgVh3k+gDEIZl+K3okAAKbMch6wKHiKOWf5GBWrSSKXKGo1kOU6fCsIVyerWnkvQcOr/aWj7IWrYMaaFAerqQixvRfStnNcHm2pjmnlpPuo0GS18dJ1wmQcFQF6yAB9EgETjRsBkNYRwO5fU7x61kH6HBe07wO6y2yeHosaJXliW3btnkc1NteDAetE0alLY/i+DVOe/A7tg+D6pfUx9Uoy7GxReGOHTv89evXR/bv328zmaPIHotS2XLz5s3FgKJFIyS+qqbSr66uvr5z507dO87aILmnTZsWbWtri3BfyLt165bH73+SV2DZUaPOoG9Ii2hsB/Vojo4/uimzo9isRQfBNm3aFH3uuee8c+fORWtqanzFt956y0dxTFls6Qwe1wWKEAtFqd9corXJRpCQzWMy0n1vf/r06RH2n97s2bMjHNj7W7dutSNoE567QJcFTCi48+k87vxNz0ZwJBO+Jsp62Kurq/NmzZrlycT41sTAovr/AHHs2IxJXde+DLOyqy7ol67X9QDeU9kKEFuMXC5PyQd5aFFUaX7m8qdMmWKTleQOzAc/dCG8m6fOzA3gJqNFn802kJjWczCnKDdiPQFTXl6uOwjeokWLIkxOwU9dWklboFEV2365b335F2N9ZD8k6vICqreU8nfjLbPmbx4apIWy7UwnTJhgdN68eaIBQDbvO5FRM033K5QXSKJoUESmx9bjnyzJ5atGfdA9KJxzITOW+RlkiyGj7nrHcNZtaFZwFhaYmKRG3c4BUBtJ23qIChxpESZYsWfPHh2BjOqgH0pxxIVojydQFMvKykxRACeGs+4IC5gEEBczOwHpREJ7DCQA0xYkAkgFJSUlX9TUGO5gNKXFO+ueYnyNyS3Ki/f4jd7y+J4OyRuWqY+wdOKfOXPmGzxU6sxMWqSHtAyggw8BUtdGRl24ePFiMVM6y518LYQNCE1MEqS0tLQT7WnqDVCSBqkhDbRZPShQBJCCyqVFCtjpfD6Jmm2ZUfSHG3QFd+/eLcQK9LuXh6WY7EpLjMbGRp2LJWmPyvsApEI05BbkvMARUALHaRNUG7YV+qhObUdDkO9krzWeaNftkMGo450vCtrkXlw+TPsFSA2uXbumO3z246GOQhQBykxNqBO/wPdjSQdt4Y5HSlozFlebJ7KEkUM2eXEfpjUsbXTkHFu4cGHKo+WUAOleEOb0PrbaGTY3Z3YAlAtoG9CkEbuh5ZZYEVP5JNZ0vrSHXUGEb2ERwZdj9uSLuMHS2J9puRfcx0m7Cke1imah+CXAUsdmowka/I5G2wuskXTdv48Nu36Gk+IZPD4ALOXlFhNj+B1ITwxTijFTaeErYCIzZsyoC695+uPxgQDpIT55nM7Cap3SDG7eWv4JLbKZTYChTf9h8CNoXtI6Qs8MZ9CnFJiUPjrOF48ChiuGdqVZwMgaBBQA1adz7zstgCQg317NgVRoTOU1uNMkUa1CGVwzwRkc3sdqM9yB2bWEl1bGy/IBSCzGRLkgb1SHYiwKu8k3VFZWpnX3IG2AJCxO+QkGX8VbsecAJNCmxDLAQCNdA2PnAWpYbohoCoe9MsbVN/UxpnDtJbX5NMoL7BY40ia+yb8NXymdcu+XOiCA9DC2PYkB18CMbubbEkBMobbu2rDMrYc3KX9UC1NVTLM1mf7GgyE9tLpYGkNan0+Y+YgXpeHP+BHPOGe7dABINemYlZ5xYcAA6UHNDgy2ijhB5gUzcnyQbu3ZxKABJGZprnw7plddWFh4i91yA2kzU8dEulT7KI5HC3gB4/CJ4+PDxO9zkw4AUn+0CbQbntrR9k9SrXXuN/6gAFKHcoaTJ09+CmFnCRQJLaoAaMHbg/H4EhygBKbytK2HNmACLdAWhO0g3cWNEmvLvQAdXumsZgxvPw//lkeTAvovJK01mPXPs3anW2PKrKQ1ogpKq572jWvWrKllzEHNsIMGyKGO/ZfB8NPY/DjKkrRJgMCgbVlU54CTAEqrTMEksZTNjCY8GtnDTBMTVZUDpXdas5LKAFCr/W5RgcSzHcxm/506depDfaH00AAJKPjz8U2zYHYOgOgigHi24IBQO95ioPauXI36AwhAJHSglakAcuUChb2WAO1Ce2pXrFihD3kHpTXi1YXgRNEVDIbCiLThI2S9guOcAdOzYDLpS2IAERbmo5RQnmeUtPNfUcC1etF0AqDYgR7mqeZdmGMdZqp/tDQoH9ffmBnRoN4dyz9xED6Z8ikwO1HCq400JdG2j7mpjYLTCGhKDUJT4435izNuotv61atXa8vg+k8M8/BkSAAKs3WVf9vFSnYSQkzEZDTrmSd3QEhSmZuoNAdt0B7QNKk/EwME7XmaiHfps2EwM1OYvwelhxygMANg4O3du7eEXxC0TChA2AKEzAMU3QeMAoh+p8I6sU9mNbSjEyDaaNcKtX8bqJ03+Yf2LWG+7pf+H1cNxp97QPvbAAAAAElFTkSuQmCC);
  background-size: contain;
  background-position: center center;
  background-repeat: no-repeat;
}
.captcha_message.loadding {
  background-color: rgb(244 245 246);
}
.captcha_message__text {
  padding: 10px;
  color: rgb(255, 255, 255);
  display: inline-block;
  text-align: center;
  max-width: 200px;
  font-size: 14px;
}
.captcha_message.loadding .captcha_message__text {
  color: rgb(202, 202, 202);
}
.captcha__bar {
  position: relative;
  text-align: center;
  overflow: hidden;
  width: 100%;
  margin-top: 5px;
  border: 1px solid #dcdfe6;
}
.captcha_progress_bar {
  position: absolute;
  top: 0px;
  left: 0px;
  width: 0;
}
.captcha_progress_bar__text {
  position: absolute;
  top: 0px;
  width: 100%;
  font-size: 12px;
  color: transparent;
  -moz-user-select: none;
  -webkit-user-select: none;
  user-select: none;
  -o-user-select: none;
  -ms-user-select: none;
  background: -webkit-gradient(
    linear,
    left top,
    right top,
    color-stop(0, var(--textColor)),
    color-stop(0.4, var(--textColor)),
    color-stop(0.5, #fff),
    color-stop(0.6, var(--textColor)),
    color-stop(1, var(--textColor))
  );
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  -webkit-text-size-adjust: none;
  -webkit-animation: slidetounlock 3s infinite;
  animation: slidetounlock 3s infinite;
}

.captcha_handler {
  position: absolute;
  top: 0px;
  left: 0px;
  cursor: move;
  background: rgb(255, 255, 255);
  display: flex;
  align-items: center;
  justify-content: center;
}

.captcha__actions {
  display: flex;
  -webkit-box-pack: justify;
  justify-content: space-between;
  -webkit-box-align: center;
  align-items: center;
  line-height: 20px;
  min-height: 20px;
  color: rgb(80, 80, 80);
  position: absolute;
  top: 0px;
  right: 0px;
  opacity: 0.8;
  background: rgba(0, 0, 0, 0.12);
  padding: 0px 4px;
  &:hover {
    opacity: 1;
    background: rgba(0, 0, 0, 0.2);
  }
}

.captcha__action__text {
  color: rgb(80, 80, 80);
  font-size: 14px !important;
}

.captcha__action {
  display: flex;
  align-items: center;
  cursor: pointer;
  text-decoration: none;
}

.goFirst {
  left: 0px !important;
  transition: left 0.5s;
}
.goKeep {
  transition: left 0.2s;
}
.goFirst2 {
  width: 0px !important;
  transition: width 0.5s;
}

@keyframes slidetounlock {
  0% {
    background-position: var(--pwidth) 0;
  }
  100% {
    background-position: var(--width) 0;
  }
}
@keyframes slidetounlock2 {
  0% {
    background-position: var(--pwidth) 0;
  }
  100% {
    background-position: var(--pwidth) 0;
  }
}

@keyframes turn {
  0% {
    -webkit-transform: rotate(0deg);
  }
  25% {
    -webkit-transform: rotate(90deg);
  }
  50% {
    -webkit-transform: rotate(180deg);
  }
  75% {
    -webkit-transform: rotate(270deg);
  }
  100% {
    -webkit-transform: rotate(360deg);
  }
}
</style>
