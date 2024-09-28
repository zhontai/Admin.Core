<template>
  <div class="personal layout-pd" v-loading="state.loading">
    <el-row>
      <!-- 个人信息 -->
      <el-col :xs="24" :sm="16">
        <el-card shadow="hover" header="个人信息">
          <div class="personal-user">
            <div class="personal-user-left">
              <el-upload
                class="h100 personal-user-left-upload"
                :action="avatarAction"
                :headers="avatarHeaders"
                :data="{ autoUpdate: true }"
                :show-file-list="false"
                :before-upload="
                  () => {
                    state.token = storesUserInfo.getToken()
                    state.avatarLoading = true
                  }
                "
                :on-success="onAvatarSuccess"
                :on-error="onAvatarError"
              >
                <img :src="avatar" />
              </el-upload>
            </div>
            <div class="personal-user-right">
              <el-row>
                <el-col :span="24" class="personal-title mb18">{{ currentTime }}，{{ personalInfo.name }} </el-col>
                <el-col :span="24">
                  <el-row>
                    <el-col v-if="personalForm.nickName" :xs="24" :sm="8" class="personal-item mb6">
                      <div class="personal-item-label">昵称：</div>
                      <div class="personal-item-value">{{ personalInfo.nickName }}</div>
                    </el-col>
                    <el-col :xs="24" :sm="16" class="personal-item mb6">
                      <div class="personal-item-label">登录地区：</div>
                      <div class="personal-item-value">
                        {{ personalInfo.lastLoginCountry }} {{ personalInfo.lastLoginProvince }} {{ personalInfo.lastLoginCity }}
                      </div>
                    </el-col>
                  </el-row>
                </el-col>
                <el-col :span="24">
                  <el-row>
                    <el-col :xs="24" :sm="8" class="personal-item mb6">
                      <div class="personal-item-label">登录IP：</div>
                      <div class="personal-item-value">{{ personalInfo.lastLoginIP }}</div>
                    </el-col>
                    <el-col :xs="24" :sm="16" class="personal-item mb6">
                      <div class="personal-item-label">登录时间：</div>
                      <div class="personal-item-value">{{ personalInfo.lastLoginTime }}</div>
                    </el-col>
                  </el-row>
                </el-col>
              </el-row>
            </div>
          </div>
        </el-card>
      </el-col>

      <!-- 消息通知 -->
      <el-col :xs="24" :sm="8" class="pl15 personal-info">
        <el-card shadow="hover">
          <template #header>
            <span>消息通知</span>
            <span class="personal-info-more">更多</span>
          </template>
          <div class="personal-info-box">
            <ul class="personal-info-ul">
              <li v-for="(v, k) in state.newsInfoList" :key="k" class="personal-info-li">
                <a :href="v.link" target="_block" class="personal-info-li-title">{{ v.title }}</a>
              </li>
            </ul>
          </div>
        </el-card>
      </el-col>

      <!-- 更新信息 -->
      <el-col :span="24">
        <el-card shadow="hover" class="mt15 personal-edit" header="更新信息">
          <div class="personal-edit-title">基本信息</div>
          <el-form ref="formRef" :model="personalForm" size="default" label-width="60px" class="mt35 mb35">
            <el-row :gutter="35">
              <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4">
                <el-form-item label="姓名" prop="name" :rules="[{ required: true, message: '请输入姓名', trigger: ['blur', 'change'] }]">
                  <el-input v-model="personalForm.name" placeholder="请输入姓名" clearable></el-input>
                </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4">
                <el-form-item label="昵称">
                  <el-input v-model="personalForm.nickName" placeholder="请输入昵称" clearable></el-input>
                </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                <el-form-item>
                  <el-popconfirm title="确定要更新个人信息吗？" hide-icon hide-after="0" width="180" @confirm="onUpdateBasic">
                    <template #reference>
                      <el-button :loading="state.updateLoading" type="primary">
                        <el-icon>
                          <ele-Position />
                        </el-icon>
                        更新个人信息
                      </el-button>
                    </template>
                  </el-popconfirm>
                </el-form-item>
              </el-col>
            </el-row>
          </el-form>
          <div class="personal-edit-title mb15">账号安全</div>
          <div class="personal-edit-safe-box">
            <div class="personal-edit-safe-item">
              <div class="personal-edit-safe-item-left">
                <div class="personal-edit-safe-item-left-label">账户密码</div>
                <div class="personal-edit-safe-item-left-value">当前密码强度：强</div>
              </div>
              <div class="personal-edit-safe-item-right">
                <el-button text type="primary" @click="onChangePassword">立即修改</el-button>
              </div>
            </div>
          </div>
          <div class="personal-edit-safe-box">
            <div class="personal-edit-safe-item">
              <div class="personal-edit-safe-item-left">
                <div class="personal-edit-safe-item-left-label">密保手机</div>
                <div class="personal-edit-safe-item-left-value">已绑定手机：{{ personalInfo.mobile }}</div>
              </div>
              <div class="personal-edit-safe-item-right">
                <el-button text type="primary">立即修改</el-button>
              </div>
            </div>
          </div>
          <div class="personal-edit-safe-box">
            <div class="personal-edit-safe-item">
              <div class="personal-edit-safe-item-left">
                <div class="personal-edit-safe-item-left-label">密保邮箱</div>
                <div class="personal-edit-safe-item-left-value">已绑定邮箱：{{ personalInfo.email }}</div>
              </div>
              <div class="personal-edit-safe-item-right">
                <el-button text type="primary">立即修改</el-button>
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <change-password-form ref="changePasswordFormRef" title="修改密码"></change-password-form>
  </div>
</template>

<script setup lang="ts" name="personal">
import { reactive, computed, onMounted, toRefs, ref, getCurrentInstance, defineAsyncComponent } from 'vue'
import { formatAxis } from '/@/utils/formatTime'
import { UserApi } from '/@/api/admin/User'
import { UserGetBasicOutput } from '/@/api/admin/data-contracts'
import { useUserInfo } from '/@/stores/userInfo'
import pinia from '/@/stores/index'
import { storeToRefs } from 'pinia'
import { AxiosResponse } from 'axios'

// 引入组件
const ChangePasswordForm = defineAsyncComponent(() => import('./components/change-password-form.vue'))

const { proxy } = getCurrentInstance() as any
const changePasswordFormRef = ref()
const formRef = ref()
const storesUserInfo = useUserInfo(pinia)
const { userInfos } = storeToRefs(storesUserInfo)

// 定义变量内容
const state = reactive({
  loading: false,
  newsInfoList: [] as any,
  recommendList: [] as any,
  personalInfo: {
    avatar: '',
    mobile: '',
    email: '',
    name: '',
    nickName: '',
  } as UserGetBasicOutput,
  personalForm: {
    name: '',
    nickName: '',
  },
  avatarLoading: false,
  updateLoading: false,
  token: storesUserInfo.getToken(),
})

const { personalInfo, personalForm } = toRefs(state)

// 当前时间提示语
const currentTime = computed(() => {
  return formatAxis(new Date())
})

// 上传头像请求头部
const avatarHeaders = computed(() => {
  return { Authorization: 'Bearer ' + state.token }
})

// 头像地址
const avatar = computed(() => {
  return userInfos.value.photo || 'https://img2.baidu.com/it/u=1978192862,2048448374&fm=253&fmt=auto&app=138&f=JPEG?w=504&h=500'
})

// 上传头像请求url
const avatarAction = computed(() => {
  return import.meta.env.VITE_API_URL + '/api/admin/user/avatar-upload'
})

onMounted(() => {
  initData()
})

// 初始化数据
const initData = async () => {
  state.loading = true
  const res = await new UserApi().getBasic().catch(() => {
    state.loading = false
  })
  if (res?.success) {
    state.personalForm.name = res.data?.name as string
    state.personalForm.nickName = res.data?.nickName as string
    state.personalInfo = res.data as UserGetBasicOutput
  }
  state.loading = false
}

// 上传头像成功
const onAvatarSuccess = (res: AxiosResponse) => {
  state.avatarLoading = false
  if (!res?.success) {
    if (res.msg) {
      proxy.$modal.msgError(res.msg)
    }
    return
  }
  state.personalInfo.avatar = res.data
  storesUserInfo.setPhoto(res.data)
}

// 上传头像失败
const onAvatarError = (error: any) => {
  state.avatarLoading = false
  let message = ''
  if (error.message) {
    try {
      message = JSON.parse(error.message)?.msg
    } catch (err) {
      message = error.message || ''
    }
  }
  if (message) proxy.$modal.msgError(message)
}

// 更新个人信息
const onUpdateBasic = async () => {
  formRef.value.validate(async (valid: boolean) => {
    if (!valid) return

    state.updateLoading = true
    const res = await new UserApi().updateBasic(state.personalForm, { showSuccessMessage: true }).catch(() => {
      state.updateLoading = false
    })
    state.updateLoading = false

    if (res?.success) {
      state.personalInfo.nickName = state.personalForm.nickName
      state.personalInfo.name = state.personalForm.name
      storesUserInfo.setUserName(state.personalForm.nickName || state.personalForm.name)
    }
  })
}

// 修改密码
const onChangePassword = () => {
  changePasswordFormRef.value.open()
}
</script>

<style scoped lang="scss">
@import '/@/theme/mixins/index.scss';
.personal {
  .personal-user {
    height: 130px;
    display: flex;
    align-items: center;
    .personal-user-left {
      width: 100px;
      height: 130px;
      border-radius: 3px;
      :deep(.el-upload) {
        height: 100%;
      }
      .personal-user-left-upload {
        img {
          width: 100%;
          height: 100%;
          border-radius: 3px;
        }
        &:hover {
          img {
            animation: logoAnimation 0.3s ease-in-out;
          }
        }
      }
    }
    .personal-user-right {
      flex: 1;
      padding: 0 15px;
      .personal-title {
        font-size: 18px;
        @include text-ellipsis(1);
      }
      .personal-item {
        display: flex;
        align-items: center;
        font-size: 13px;
        .personal-item-label {
          color: var(--el-text-color-secondary);
          @include text-ellipsis(1);
        }
        .personal-item-value {
          @include text-ellipsis(1);
        }
      }
    }
  }
  .personal-info {
    .personal-info-more {
      float: right;
      color: var(--el-text-color-secondary);
      font-size: 13px;
      &:hover {
        color: var(--el-color-primary);
        cursor: pointer;
      }
    }
    .personal-info-box {
      height: 130px;
      overflow: hidden;
      .personal-info-ul {
        list-style: none;
        .personal-info-li {
          font-size: 13px;
          padding-bottom: 10px;
          .personal-info-li-title {
            display: inline-block;
            @include text-ellipsis(1);
            color: var(--el-text-color-secondary);
            text-decoration: none;
          }
          & a:hover {
            color: var(--el-color-primary);
            cursor: pointer;
          }
        }
      }
    }
  }
  .personal-recommend-row {
    .personal-recommend-col {
      .personal-recommend {
        position: relative;
        height: 100px;
        border-radius: 3px;
        overflow: hidden;
        cursor: pointer;
        &:hover {
          i {
            right: 0px !important;
            bottom: 0px !important;
            transition: all ease 0.3s;
          }
        }
        i {
          position: absolute;
          right: -10px;
          bottom: -10px;
          font-size: 70px;
          transform: rotate(-30deg);
          transition: all ease 0.3s;
        }
        .personal-recommend-auto {
          padding: 15px;
          position: absolute;
          left: 0;
          top: 5%;
          color: var(--next-color-white);
          .personal-recommend-msg {
            font-size: 12px;
            margin-top: 10px;
          }
        }
      }
    }
  }
  .personal-edit {
    .personal-edit-title {
      position: relative;
      padding-left: 10px;
      color: var(--el-text-color-regular);
      &::after {
        content: '';
        width: 2px;
        height: 10px;
        position: absolute;
        left: 0;
        top: 50%;
        transform: translateY(-50%);
        background: var(--el-color-primary);
      }
    }
    .personal-edit-safe-box {
      border-bottom: 1px solid var(--el-border-color-light, #ebeef5);
      padding: 15px 0;
      .personal-edit-safe-item {
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: space-between;
        .personal-edit-safe-item-left {
          flex: 1;
          overflow: hidden;
          .personal-edit-safe-item-left-label {
            color: var(--el-text-color-regular);
            margin-bottom: 5px;
          }
          .personal-edit-safe-item-left-value {
            color: var(--el-text-color-secondary);
            @include text-ellipsis(1);
            margin-right: 15px;
          }
        }
      }
      &:last-of-type {
        padding-bottom: 0;
        border-bottom: none;
      }
    }
  }
}
</style>
