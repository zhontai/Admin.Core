<template>
  <div class="my-layout">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="name">
          <el-input v-model="state.filterModel.fileName" placeholder="文件名" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:file:upload-file'" type="primary" icon="ele-Upload" @click="onUpload"> 上传 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.fileListData" row-key="id" style="width: 100%">
        <el-table-column prop="fileName" label="文件名" min-width="220">
          <template #default="{ row }">
            <div class="my-flex">
              <el-image
                v-if="isImage(row.extension)"
                :src="row.linkUrl"
                :preview-src-list="previewImglist"
                :initial-index="getInitialIndex(row.linkUrl)"
                :lazy="true"
                :hide-on-click-modal="true"
                fit="scale-down"
                preview-teleported
                style="width: 80px; height: 80px"
              />
              <div class="ml10 my-flex-fill my-flex-y-center">
                <div>{{ (row.fileName || '') + (row.extension || '') }}</div>
              </div>
            </div>
          </template>
        </el-table-column>
        <el-table-column prop="sizeFormat" label="大小" width="120" />
        <el-table-column prop="createdUserName" label="上传者" width="80">
          <template #default="{ row }">
            {{ row.modifiedUserName || row.createdUserName || '' }}
          </template>
        </el-table-column>
        <el-table-column prop="createdTime" label="更新时间" width="100">
          <template #default="{ row }">
            {{ formatterTime(row.modifiedTime || row.createdTime || '') }}
          </template>
        </el-table-column>
        <el-table-column prop="providerName" label="供应商" width="80" />
        <el-table-column prop="bucketName" label="存储桶" min-width="120" />
        <el-table-column prop="fileDirectory" label="目录" min-width="120" />
        <el-table-column label="操作" width="180" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-popover :width="220">
              <p>{{ row.linkUrl }}</p>
              <div class="mt10" style="text-align: right; margin: 0">
                <el-button icon="ele-CopyDocument" size="small" type="primary" @click="copyText(row.linkUrl)">复制地址</el-button>
              </div>
              <template #reference>
                <el-button size="small" text type="primary">地址</el-button>
              </template>
            </el-popover>
            <el-link
              class="my-el-link mr12 ml12"
              :href="row.linkUrl"
              type="primary"
              icon="ele-Download"
              size="small"
              :underline="false"
              target="_blank"
              >下载</el-link
            >
            <el-button v-auth="'api:admin:file:delete'" icon="ele-Delete" size="small" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
      <div class="my-flex my-flex-end" style="margin-top: 20px">
        <el-pagination
          v-model:currentPage="state.pageInput.currentPage"
          v-model:page-size="state.pageInput.pageSize"
          :total="state.total"
          :page-sizes="[10, 20, 50, 100]"
          small
          background
          @size-change="onSizeChange"
          @current-change="onCurrentChange"
          layout="total, sizes, prev, pager, next, jumper"
        />
      </div>
    </el-card>

    <file-upload ref="fileUploadRef" title="上传文件"></file-upload>
  </div>
</template>

<script lang="ts" setup name="admin/file">
import { ref, reactive, onMounted, onBeforeMount, defineAsyncComponent, computed, getCurrentInstance } from 'vue'
import { PageInputFileGetPageDto, FileGetPageOutput } from '/@/api/admin/data-contracts'
import { FileApi } from '/@/api/admin/File'
import dayjs from 'dayjs'
import eventBus from '/@/utils/mitt'
import { isImage } from '/@/utils/test'
import commonFunction from '/@/utils/commonFunction'

const { proxy } = getCurrentInstance() as any

const FileUpload = defineAsyncComponent(() => import('./components/file-upload.vue'))

const fileUploadRef = ref()

const state = reactive({
  loading: false,
  fileFormTitle: '',
  filterModel: {
    fileName: '',
  },
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputFileGetPageDto,
  fileListData: [] as Array<FileGetPageOutput>,
  fileLogsTitle: '',
})

const { copyText } = commonFunction()

const previewImglist = computed(() => {
  let imgList = [] as string[]
  state.fileListData.forEach((a) => {
    if (isImage(a.extension as string) && a.linkUrl) {
      imgList.push(a.linkUrl as string)
    }
  })
  return imgList
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshFile')
  eventBus.on('refreshFile', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshFile')
})

const formatterTime = (cellValue: any) => {
  return dayjs(cellValue).format('YYYY-MM-DD HH:mm:ss')
}

const getInitialIndex = (imgUrl: string) => {
  return previewImglist.value.indexOf(imgUrl)
}

const onQuery = async () => {
  state.loading = true
  const res = await new FileApi().getPage({ ...state.pageInput, filter: state.filterModel }).catch(() => {
    state.loading = false
  })

  state.fileListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onSizeChange = (val: number) => {
  state.pageInput.currentPage = 1
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}

const onUpload = () => {
  fileUploadRef.value.open()
}

const onDelete = (row: FileGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除文件【${row.fileName}${row.extension}】?`)
    .then(async () => {
      await new FileApi().delete({ id: row.id as number }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss">
.my-el-link {
  font-size: 12px;
}
</style>
