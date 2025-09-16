<template>
  <div>
    <el-dialog draggable title="更换头像" v-model="state.isShowDialog" width="769px">
      <div class="cropper-warp">
        <div class="cropper-warp-left">
          <cropper-canvas background style="height: 100%">
            <cropper-image :src="state.cropperImg" alt="Picture" rotatable scalable skewable translatable crossorigin="anonymous"></cropper-image>
            <cropper-shade hidden></cropper-shade>
            <cropper-handle action="select" plain></cropper-handle>
            <cropper-selection ref="sourceRef" aspect-ratio="1" initial-aspect-ratio="1" initial-coverage="0.5" movable resizable @change="onChange">
              <cropper-grid role="grid" covered hidden></cropper-grid>
              <cropper-crosshair centered></cropper-crosshair>
              <cropper-handle action="move" theme-color="rgba(255, 255, 255, 0.35)"></cropper-handle>
              <cropper-handle action="n-resize"></cropper-handle>
              <cropper-handle action="e-resize"></cropper-handle>
              <cropper-handle action="s-resize"></cropper-handle>
              <cropper-handle action="w-resize"></cropper-handle>
              <cropper-handle action="ne-resize"></cropper-handle>
              <cropper-handle action="nw-resize"></cropper-handle>
              <cropper-handle action="se-resize"></cropper-handle>
              <cropper-handle action="sw-resize"></cropper-handle>
            </cropper-selection>
          </cropper-canvas>
        </div>
        <div class="cropper-warp-right">
          <div class="cropper-warp-right-title">预览</div>
          <div class="cropper-warp-right-item">
            <div class="cropper-warp-right-value">
              <img :src="state.cropperImgBase64" class="cropper-warp-right-value-img" />
            </div>
            <div class="cropper-warp-right-label">100 x 100</div>
          </div>
          <div class="cropper-warp-right-item">
            <div class="cropper-warp-right-value">
              <img :src="state.cropperImgBase64" class="cropper-warp-right-value-img cropper-size" />
            </div>
            <div class="cropper-warp-right-label">50 x 50</div>
          </div>
        </div>
      </div>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel">取 消</el-button>
          <el-button type="primary" @click="onSubmit">更 换</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts" name="cropper">
import 'cropperjs'

const sourceRef = useTemplateRef('sourceRef')

const cropperImg = defineModel('cropperImg', { type: String })

// 定义变量内容
const state = reactive({
  isShowDialog: false,
  cropperImg: '',
  cropperImgBase64: '',
  cropper: '' as RefType,
})

// 打开弹窗
const openDialog = (imgUrl: string) => {
  state.cropperImg = imgUrl
  state.isShowDialog = true
}
// 关闭弹窗
const closeDialog = () => {
  state.isShowDialog = false
}
// 取消
const onCancel = () => {
  closeDialog()
}
// 更换
const onSubmit = () => {
  cropperImg.value = state.cropperImgBase64
  closeDialog()
}

const onChange = async () => {
  sourceRef.value.$toCanvas().then((canvas: HTMLCanvasElement) => {
    state.cropperImgBase64 = canvas.toDataURL('image/jpeg')
  })
}

// 暴露变量
defineExpose({
  openDialog,
})
</script>

<style scoped lang="scss">
.cropper-warp {
  display: flex;
  .cropper-warp-left {
    position: relative;
    display: inline-block;
    height: 350px;
    flex: 1;
    border: 1px solid var(--el-border-color);
    background: var(--el-color-white);
    overflow: hidden;
    background-repeat: no-repeat;
    cursor: move;
    border-radius: var(--el-border-radius-base);
    .cropper-warp-left-img {
      width: 100%;
      height: 100%;
    }
  }
  .cropper-warp-right {
    width: 150px;
    height: 350px;
    .cropper-warp-right-title {
      text-align: center;
      height: 20px;
      line-height: 20px;
    }
    .cropper-warp-right-item {
      margin: 15px 0;
      .cropper-warp-right-value {
        display: flex;
        .cropper-warp-right-value-img {
          width: 100px;
          height: 100px;
          border-radius: var(--el-border-radius-circle);
          margin: auto;
        }
        .cropper-size {
          width: 50px;
          height: 50px;
        }
      }
      .cropper-warp-right-label {
        text-align: center;
        font-size: 12px;
        color: var(--el-text-color-primary);
        height: 30px;
        line-height: 30px;
      }
    }
  }
}
</style>
