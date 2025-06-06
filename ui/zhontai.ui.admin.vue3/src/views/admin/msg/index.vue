<template>
  <MySplitPanes>
    <pane size="55" min-size="35" max-size="65">
      <div class="my-flex-column w100 h100">
        <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item label="标题">
              <el-input v-model="state.filter.msgName" placeholder="标题" @keyup.enter="onQuery" />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
              <el-button v-auth="'api:admin:msg:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
            </el-form-item>
          </el-form>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
          <el-table
            ref="msgTableRef"
            v-loading="state.loading"
            :data="state.msgData"
            default-expand-all
            highlight-current-row
            style="width: 100%"
            @current-change="onTableCurrentChange"
            border
          >
            <el-table-column prop="title" label="标题" min-width="120" show-overflow-tooltip />
            <el-table-column prop="typeName" label="消息分类" min-width="120" show-overflow-tooltip />
            <el-table-column prop="status" label="状态" min-width="90" show-overflow-tooltip :formatter="formatterMsgStatusEnum" />
            <el-table-column prop="createdTime" label="创建时间" :formatter="formatterTime" min-width="160" show-overflow-tooltip />
            <el-table-column label="操作" width="100" fixed="right" header-align="center" align="center">
              <template #default="{ row }">
                <my-dropdown-more v-auths="['api:admin:msg:update', 'api:admin:msg:delete']" style="margin-left: 0px">
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item v-if="auth('api:admin:msg:update')" @click="onEdit(row)">编辑</el-dropdown-item>
                      <el-dropdown-item v-if="auth('api:admin:msg:delete')" @click="onDelete(row)">删除</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </my-dropdown-more>
              </template>
            </el-table-column>
          </el-table>
          <div class="my-flex my-flex-end" style="margin-top: 10px">
            <el-pagination
              v-model:currentPage="state.pageInput.currentPage"
              v-model:page-size="state.pageInput.pageSize"
              :total="state.total"
              :page-sizes="[10, 20, 50, 100]"
              background
              @size-change="onSizeChange"
              @current-change="onCurrentChange"
              layout="total, sizes, prev, pager, next, jumper"
            />
          </div>
        </el-card>
      </div>
    </pane>
    <pane>
      <div class="my-flex-column w100 h100">
        <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item label="姓名">
              <el-input v-model="state.filter.name" placeholder="姓名" @keyup.enter="onGetMsgUserList" />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" icon="ele-Search" @click="onGetMsgUserList"> 查询 </el-button>
              <el-button v-auth="'api:admin:msg:add-msg-user'" type="primary" icon="ele-Plus" @click="onAddUser"> 添加 </el-button>
              <el-button v-auth="'api:admin:msg:remove-msg-user'" type="danger" icon="ele-Delete" @click="onRemoveUser"> 移除 </el-button>
            </el-form-item>
          </el-form>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
          <el-table
            ref="userTableRef"
            v-loading="state.userListLoading"
            :data="state.userListData"
            row-key="id"
            style="width: 100%"
            @row-click="onUserRowClick"
            border
          >
            <el-table-column type="selection" width="55" />
            <el-table-column prop="name" label="姓名" min-width="120" show-overflow-tooltip />
            <el-table-column label="是否已读" width="100" align="center">
              <template #default="{ row }">
                <el-tag type="success" v-if="row.isRead">已读</el-tag>
                <el-tag type="info" v-else>未读</el-tag>
              </template>
            </el-table-column>
            <el-table-column prop="readTime" label="已读时间" :formatter="formatterTime" min-width="160" show-overflow-tooltip />
            <!-- <el-table-column prop="mobile" label="手机号" min-width="120" show-overflow-tooltip />
            <el-table-column prop="email" label="邮箱" min-width="180" show-overflow-tooltip /> -->
          </el-table>
        </el-card>
      </div>
    </pane>

    <msg-form ref="msgFormRef" :title="state.msgFormTitle"></msg-form>
    <user-select
      ref="userSelectRef"
      :title="`添加【${state.msgName}】员工`"
      multiple
      :sure-loading="state.sureLoading"
      @sure="onSureUser"
    ></user-select>
  </MySplitPanes>
</template>

<script lang="ts" setup name="admin/msg">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import {
  PageInputMsgGetPageInput,
  MsgGetMsgUserListOutput,
  UserGetPageOutput,
  MsgAddMsgUserListInput,
  MsgGetPageOutput,
} from '/@/api/admin/data-contracts'
import { MsgApi } from '/@/api/admin/Msg'
import { ElTable } from 'element-plus'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import { Pane } from 'splitpanes'
import dayjs from 'dayjs'
import { MsgStatusEnum } from '/@/api/admin/enum-contracts'
import { getDescByValue } from '/@/utils/enum'

// 引入组件
const MsgForm = defineAsyncComponent(() => import('./components/msg-form.vue'))
const UserSelect = defineAsyncComponent(() => import('/@/views/admin/user/components/user-select.vue'))
const MySplitPanes = defineAsyncComponent(() => import('/@/components/my-layout/split-panes.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))

const { proxy } = getCurrentInstance() as any

const msgTableRef = ref()
const msgFormRef = ref()
const userTableRef = ref<InstanceType<typeof ElTable>>()
const userSelectRef = ref()

const state = reactive({
  loading: false,
  userListLoading: false,
  sureLoading: false,
  msgFormTitle: '',
  filter: {
    name: '',
    msgName: '',
  },
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
    filter: {
      title: '',
    },
  } as PageInputMsgGetPageInput,
  msgData: [] as any,
  userListData: [] as MsgGetMsgUserListOutput[],
  msgId: undefined as number | undefined,
  msgName: '' as string | null | undefined,
})

onMounted(async () => {
  onQuery()
  eventBus.off('refreshMsg')
  eventBus.on('refreshMsg', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshMsg')
})

const formatterMsgStatusEnum = (row: any, column: any, cellValue: any) => {
  return getDescByValue(MsgStatusEnum, cellValue)
}

const formatterTime = (row: any, column: any, cellValue: any) => {
  return cellValue ? dayjs(cellValue).format('YYYY-MM-DD HH:mm:ss') : ''
}

const onQuery = async () => {
  state.loading = true
  if (state.pageInput.filter) state.pageInput.filter.title = state.filter.msgName
  const res = await new MsgApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.msgData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0

  if (state.msgData?.length > 0) {
    window.setTimeout(() => {
      msgTableRef.value?.setCurrentRow(state.msgData[0])
    }, 100)
    // nextTick(() => {
    //   msgTableRef.value?.setCurrentRow(state.msgData[0])
    // })
  }

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

const onAdd = () => {
  state.msgFormTitle = '新增消息'
  msgFormRef.value.open({ enabled: true })
}

const onEdit = (row: MsgGetPageOutput) => {
  state.msgFormTitle = '编辑消息'
  msgFormRef.value.open(row)
}

const onDelete = (row: MsgGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除消息【${row.title}】?`)
    .then(async () => {
      await new MsgApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onGetMsgUserList = async () => {
  state.userListLoading = true
  const res = await new MsgApi().getMsgUserList({ MsgId: state.msgId, Name: state.filter.name }).catch(() => {
    state.userListLoading = false
  })
  state.userListLoading = false
  if (res?.success) {
    if (res.data && res.data.length > 0) {
      state.userListData = res.data
    } else {
      state.userListData = []
    }
  }
}

const onTableCurrentChange = (currentRow: MsgGetPageOutput) => {
  if (!currentRow) {
    return
  }

  state.msgId = currentRow.id
  state.msgName = currentRow.title
  onGetMsgUserList()
}

const onUserRowClick = (row: MsgGetMsgUserListOutput) => {
  userTableRef.value!.toggleRowSelection(row, undefined)
}

const onAddUser = () => {
  if (!((state.msgId as number) > 0)) {
    proxy.$modal.msgWarning('请选择消息')
    return
  }
  userSelectRef.value.open({ msgId: state.msgId })
}

const onRemoveUser = () => {
  if (!((state.msgId as number) > 0)) {
    proxy.$modal.msgWarning('请选择消息')
    return
  }

  const selectionRows = userTableRef.value!.getSelectionRows() as UserGetPageOutput[]

  if (!((selectionRows.length as number) > 0)) {
    proxy.$modal.msgWarning('请选择员工')
    return
  }

  proxy.$modal
    .confirm(`确定要移除吗?`)
    .then(async () => {
      const userIds = selectionRows?.map((a) => a.id)
      const input = { msgId: state.msgId, userIds } as MsgAddMsgUserListInput
      await new MsgApi().removeMsgUser(input, { loading: true })
      onGetMsgUserList()
    })
    .catch(() => {})
}

const onSureUser = async (users: UserGetPageOutput[]) => {
  if (!(users?.length > 0)) {
    userSelectRef.value.close()
    return
  }

  state.sureLoading = true
  const userIds = users?.map((a) => a.id)
  const input = { msgId: state.msgId, userIds } as MsgAddMsgUserListInput
  await new MsgApi().addMsgUser(input, { showSuccessMessage: true }).catch(() => {
    state.sureLoading = false
  })
  state.sureLoading = false
  userSelectRef.value.close()
  onGetMsgUserList()
}
</script>

<style scoped lang="scss"></style>
