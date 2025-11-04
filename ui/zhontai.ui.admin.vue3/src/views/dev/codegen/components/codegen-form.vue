<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :close-on-click-modal="false"
      :title="state.title"
      draggable
      class="my-dialog-model"
      :overflow="true"
      align-center
      :show-close="false"
      :fullscreen="state.isFull"
      width="80%"
    >
      <template #header="{ close, titleId, titleClass }">
        <div class="my-flex my-flex-between my-flex-items-center">
          <h4 :id="titleId" :class="titleClass">{{ state.title }}</h4>
          <div class="my-flex">
            <div class="el-dialog__btn">
              <el-icon v-if="state.isFull" @click="state.isFull = !state.isFull" title="还原"><ele-CopyDocument /></el-icon>
              <el-icon v-else @click="state.isFull = !state.isFull" title="最大化"><ele-FullScreen /></el-icon>
            </div>
            <div class="el-dialog__btn">
              <el-icon @click="close" title="关闭"><ele-Close /></el-icon>
            </div>
          </div>
        </div>
      </template>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel"> 取消 </el-button>
          <el-button type="primary" @click="onSure"> 确定 </el-button>
        </span>
      </template>
      <div>
        <div style="margin-bottom: 20px; text-align: center; position: relative">
          <el-radio-group v-model="state.editor">
            <el-radio-button label="infor">基础配置</el-radio-button>
            <el-radio-button label="field">字段配置</el-radio-button>
          </el-radio-group>
          <div style="position: absolute; top: 0; right: 0">
            <el-row v-show="state.editor == 'field'">
              <el-col style="text-align: right">
                <el-space wrap :size="12">
                  <el-dropdown split-button type="primary" placement="bottom-end" @click="appendField(1)">
                    新增字段
                    <template #dropdown>
                      <el-dropdown-menu>
                        <el-dropdown-item @click="appendField(2)">新增主键字段</el-dropdown-item>
                        <el-dropdown-item @click="appendField(3)">新增租户字段</el-dropdown-item>
                        <el-dropdown-item @click="appendField(4)">新增通用字段</el-dropdown-item>
                      </el-dropdown-menu>
                    </template>
                  </el-dropdown>
                  <el-input-number v-model="state.appendCount" :min="1" :max="20" style="width: 105px" controls-position="right">
                    <template #suffix>
                      <span>行</span>
                    </template>
                  </el-input-number>
                </el-space>
              </el-col>
            </el-row>
          </div>
        </div>

        <el-form
          ref="tableInfoFromRef"
          :model="state.config"
          label-width="auto"
          label-position="right"
          v-show="state.editor == 'infor'"
          :rules="editRules"
        >
          <el-row>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item label="数据库" prop="dbKey" :rules="[{ required: true, message: '请选择数据库', trigger: ['change'] }]">
                <el-select v-model="state.config.dbKey" clearable>
                  <el-option v-for="item in state.dbKeys" :key="item.dbKey" :value="item.dbKey" :label="item.dbKey"></el-option>
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item label="表名" prop="tableName">
                <el-input v-model="state.config.tableName" required placeholder="模块_子模块 eg:homely_thing"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item label="业务名" prop="busName">
                <el-input v-model="state.config.busName" required placeholder="子模块名称 eg:物品"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item label="命名空间" prop="namespace">
                <el-input v-model="state.config.namespace" required placeholder="eg:ZhonTai.Module.Homely"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item label="实体名" prop="entityName">
                <el-input v-model="state.config.entityName" required placeholder="留空时取表名大驼峰，会自动加上Entity"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item label="基类" prop="baseEntity">
                <el-select v-model="state.config.baseEntity" style="width: 100%">
                  <el-option v-for="item in entityBaseTypes" :key="item.value" :value="item.value" :label="item.label"></el-option>
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item label="第一级路径名" prop="apiAreaName">
                <el-input v-model="state.config.apiAreaName" placeholder="第一级路径名 eg:homely"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item label="父菜单" prop="menuPid">
                <el-input v-model="state.config.menuPid" placeholder="eg:家常管理"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item label="菜单后缀" prop="menuAfterText">
                <el-input v-model="state.config.menuAfterText" placeholder="eg:列表"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item label="作者" prop="authorName">
                <el-input v-model="state.config.authorName" required></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item label="备注" prop="comment"><el-input v-model="state.config.comment" style="width: 100%"></el-input> </el-form-item>
            </el-col>
            <!-- <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
            <el-form-item label="生成项">
              <el-checkbox v-model="state.config.genEntity" label="实体"/>
              <el-checkbox v-model="state.config.genRepository" label="仓储"/>
              <el-checkbox v-model="state.config.genService" label="服务"/>
            </el-form-item>
          </el-col> -->
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item label="服务接口" :disabled="!state.config.genService">
                <el-checkbox v-model="state.config.genAdd">新增</el-checkbox>
                <el-checkbox v-model="state.config.genUpdate">更新</el-checkbox>
                <el-checkbox v-model="state.config.genGetList">列表查询</el-checkbox>
                <el-checkbox v-model="state.config.genSoftDelete">软删除</el-checkbox>
                <el-checkbox v-model="state.config.genBatchSoftDelete">批量软删除</el-checkbox>
                <el-checkbox v-model="state.config.genDelete">删除</el-checkbox>
                <el-checkbox v-model="state.config.genBatchDelete">批量删除</el-checkbox>
              </el-form-item>
            </el-col>

            <el-col :xl="12" :lg="12" :md="24" :sm="24" :xs="24">
              <el-form-item label="后端输出目录" prop="backendOut">
                <el-input v-model="state.config.backendOut" required placeholder="后端实体，仓储，服务代码目录"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="12" :lg="12" :md="24" :sm="24" :xs="24">
              <el-form-item label="前端输出目录" prop="frontendOut">
                <el-input v-model="state.config.frontendOut" required placeholder="前端视图目录"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item label="脚本输出目录" prop="dbMigrateSqlOut">
                <el-input v-model="state.config.dbMigrateSqlOut" placeholder="数据库迁移脚本.sql"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item label="命名导入" prop="usings">
                <el-input v-model="state.config.usings" style="width: 100%" placeholder="在此导入的命令空间，列的外联类型可以直接使用类型"></el-input>
              </el-form-item>
            </el-col>
          </el-row>
        </el-form>

        <el-form :size="state.tableSize" v-show="state.editor == 'field'">
          <el-table class="my-dialog-table" :data="state.config.fields" border>
            <el-table-column fixed="right" width="50">
              <template #header>
                <el-dropdown @command="setTableSize" placement="bottom-start">
                  <span class="el-dropdown-link"> <i class="iconfont icon-xianshimima" title="显示尺寸"></i> </span>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="large">松散</el-dropdown-item>
                      <el-dropdown-item command="default">正常</el-dropdown-item>
                      <el-dropdown-item command="small">紧凑</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
              </template>
              <template #default="scope">
                <el-button type="danger" link icon="ele-Minus" @click="removeField(scope.row, scope.$index)" />
              </template>
            </el-table-column>
            <el-table-column prop="columnName" label="列名" label-class-name="my-col--required" fixed width="150">
              <template #default="scope">
                <el-input
                  v-if="!showMode"
                  v-model="scope.row.columnName"
                  :class="{ 'field-error': hasFieldError(scope.$index, 'columnName') }"
                  @blur="validateField(scope.$index, 'columnName')"
                ></el-input>
                <div v-else>{{ scope.row.columnName }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="title" label="标题" width="140" fixed>
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.title"></el-input>
                <div v-else>{{ scope.row.title }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="netType" label="类型" width="130">
              <template #default="scope">
                <el-select v-if="!showMode" v-model="scope.row.netType">
                  <el-option v-for="item in netTypes" :key="item" :value="item" :label="item"></el-option>
                </el-select>
                <div v-else>{{ scope.row.netType }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="length" label="长度" width="85">
              <template #default="scope">
                <el-input-number v-if="!showMode" v-model="scope.row.length" :min="1" align="left" :controls="false" style="width: 100%">
                </el-input-number>
                <div v-else>{{ scope.row.length }}</div>
              </template>
            </el-table-column>

            <el-table-column prop="editor" label="组件类型" label-class-name="my-col--required" width="140">
              <template #default="scope">
                <el-select
                  v-if="!showMode"
                  v-model="scope.row.editor"
                  :class="{ 'field-error': hasFieldError(scope.$index, 'editor') }"
                  @change="validateField(scope.$index, 'editor')"
                >
                  <el-option v-for="item in editors" :key="item.value" :value="item.value" :label="item.label"></el-option>
                </el-select>
                <div v-else>{{ scope.row.editor }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="whetherTable" label="表" width="55">
              <template #header>
                <div class="my-flex-y-center">
                  表<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>表格分页列表显示字段</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherTable" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="whetherAdd" label="增" width="50">
              <template #header>
                <div class="my-flex-y-center">
                  增<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>新增页面显示的字段</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherAdd" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="whetherUpdate" label="改" width="50">
              <template #header>
                <div class="my-flex-y-center">
                  改<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>修改页面显示的字段</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherUpdate" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>

            <el-table-column prop="whetherList" label="列" width="50">
              <template #header>
                <div class="my-flex-y-center">
                  列<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>获取的精简数据，下拉框，选择框使用的字段</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherList" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="isNullable" label="空" width="50">
              <template #header>
                <div class="my-flex-y-center">
                  空<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>字段模型是否可空类型</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.isNullable" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>

            <el-table-column prop="whetherQuery" label="查" width="50">
              <template #header>
                <div class="my-flex-y-center">
                  查<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>查询条件显示的字段</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherQuery" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="queryType" label="查询方式" width="130">
              <template #default="scope">
                <el-select v-model="scope.row.queryType" v-if="!showMode">
                  <el-option v-for="item in operates" :key="item.value" :value="item.value" :label="item.label"></el-option>
                </el-select>
                <div v-else>{{ scope.row.queryType }}</div>
              </template>
            </el-table-column>

            <!-- <el-table-column prop="effectType" label="作用类型" width="120">
            <template #default="scope">
              <el-select v-if="!showMode" v-model="scope.row.effectType">
                <el-option key="input" value="input" label="输入"></el-option>
                <el-option key="dict" value="dict" label="字典"></el-option>
              </el-select>
              <div v-else>{{ scope.row.effectType }}</div>
            </template>
          </el-table-column> -->
            <el-table-column prop="includeEntity" width="140">
              <template #header>
                <div class="my-flex-y-center">
                  关联模型<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>用于生成页面下拉选择，一对多则生成多选 eg: CodeGroupEntity</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-input
                  v-if="!showMode"
                  v-model="scope.row.includeEntity"
                  title="用于生成页面下拉选择，一对多则生成多选 eg: CodeGroupEntity，一对一的时候将生成字段 字段_Text, 一对多的时候，模型中将生成 字段_Values，字段_Texts"
                  placeholder="关联模型名 xxxEntity"
                ></el-input>
                <div v-else>{{ scope.row.includeEntity }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="includeEntityKey" width="140">
              <template #header>
                <div class="my-flex-y-center">
                  关联模型字段<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>列表和获取单个时显示文本值，根据Id查询关联模型，显示的字段 eg:Title，关联查询模型时的字段</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-input
                  v-if="!showMode"
                  v-model="scope.row.includeEntityKey"
                  title="列表和获取单个时显示文本值，根据Id查询关联模型，显示的字段 eg:Title"
                  placeholder="关联显示字段 Title"
                ></el-input>
                <div v-else>{{ scope.row.includeEntityKey }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="includeMode" label="外联方式" width="150">
              <template #default="scope">
                <el-select v-if="!showMode" v-model="scope.row.includeMode" title="对应单选/多选">
                  <el-option v-for="item in includes" :key="item.value" :value="item.value" :label="item.label"></el-option>
                </el-select>
                <div v-else>{{ scope.row.includeMode }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="dictTypeCode" label="字典编码" width="120">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.dictTypeCode"></el-input>
                <div v-else>{{ scope.row.dictTypeCode }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="displayColumn" label="选中文本字段" width="140">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.displayColumn"></el-input>
                <div v-else>{{ scope.row.displayColumn }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="valueColumn" label="选中值字段" width="140">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.valueColumn"></el-input>
                <div v-else>{{ scope.row.valueColumn }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="pidColumn" label="父级字段" width="140">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.pidColumn"></el-input>
                <div v-else>{{ scope.row.pidColumn }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="frontendRuleTrigger" label="前端检测触发" width="140">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.frontendRuleTrigger"></el-input>
                <div v-else>{{ scope.row.frontendRuleTrigger }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="isPrimary" label="主键" width="60">
              <template #default="scope"
                ><!-- :disabled="showMode || editMode != 'add'"-->
                <el-checkbox v-model="scope.row.isPrimary" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="defaultValue" label="默认值" width="100">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.defaultValue"></el-input>
                <div v-else>{{ scope.row.defaultValue }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="indexMode" label="索引" width="120">
              <template #default="scope">
                <el-select v-if="!showMode" v-model="scope.row.indexMode">
                  <el-option value="" label="" />
                  <el-option value="ASC" label="ASC" />
                  <el-option value="DESC" label="DESC" />
                </el-select>
                <div v-else>{{ scope.row.indexMode }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="isUnique" label="唯一" width="60">
              <template #default="scope">
                <el-checkbox v-model="scope.row.isUnique" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="length" label="数据库类型" width="150">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.dbType"></el-input>
                <div v-else>{{ scope.row.dbType }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="columnRawName" label="数据库列名" width="150">
              <template #default="scope"
                ><!-- :disabled="editMode != 'add'"-->
                <el-input v-if="!showMode" v-model="scope.row.columnRawName"></el-input>
                <div v-else>{{ scope.row.columnRawName }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="comment" label="备注" width="200">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.comment"></el-input>
                <div v-else>{{ scope.row.comment }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="position" label="列顺序" width="80">
              <template #default="scope">
                <el-input-number v-if="!showMode" v-model="scope.row.position" align="left" :controls="false" style="width: 100%"> </el-input-number>
                <div v-else>{{ scope.row.position }}</div>
              </template>
            </el-table-column>
          </el-table>
        </el-form>
      </div>
    </el-dialog>
  </div>
</template>
<script lang="ts" setup>
import eventBus from '/@/utils/mitt'
import { DatabaseGetOutput, CodeGenFieldGetOutput, CodeGenGetOutput, CodeGenUpdateInput } from '/@/api/dev/data-contracts'
import { AxiosResponse } from 'axios'
import { FormRules } from 'element-plus'
import { CodeGenApi } from '/@/api/dev/CodeGen'

const { proxy } = getCurrentInstance() as any

const tableInfoFromRef = ref()

const props = defineProps({
  showMode: { type: Boolean, default: false },
  entityBaseTypes: {
    type: [] as PropType<Array<SelectOptionType>>,
    default: [
      { value: 'EntityBase', label: '基础类型' },
      { value: 'EntityTenant', label: '租户基础类型' },
    ],
  },
  netTypes: {
    type: [] as PropType<Array<String>>,
    default: [
      'int',
      'string',
      'bool',
      'DateTime',
      'byte',
      'byte[]',
      'sbyte',
      'char',
      'short',
      'long',
      'ushort',
      'uint',
      'ulong',
      'float',
      'double',
      'decimal',
    ],
  },
  editors: {
    type: [] as PropType<Array<SelectOptionType>>,
    default: [
      { label: '文本框', value: 'el-input' },
      { label: '数值框', value: 'el-input-number' },
      { label: '文本域', value: 'my-input-textarea' },
      { label: '日期框', value: 'el-date-picker' },
      { label: '下拉框', value: 'el-select' },
      { label: '开关', value: 'el-switch' },
      { label: '复选框', value: 'el-checkbox' },
      { label: '图片上传', value: 'my-upload' },
      { label: '编辑器', value: 'my-editor' },
      { label: '业务选择', value: 'my-bussiness-select' },
    ],
  },
  includes: {
    type: [] as PropType<Array<SelectOptionType>>,
    default: [
      { value: 0, label: '1对1(单选)' },
      { value: 1, label: '1对多(多选)' },
    ],
  },
  operates: {
    type: [] as PropType<Array<SelectOptionType>>,
    default: [
      { value: '0', label: '包含' },
      // { value:'1', label:"开头等于" },
      // { value:'2', label:"结尾等于" },
      // { value:'3', label:"不包含" },
      // { value:'4', label:"开头不等于" },
      // { value:'5', label:"结尾不等于" },
      { value: '6', label: '等于Eq' },
      // { value:'7', label:"等于Equal" },
      // { value:'8', label:"等于Equals" },
      // { value:'9', label:"不等于" },
      // { value:'10', label:"大于" },
      // { value:'11', label:"大于等于" },
      // { value:'12', label:"小于" },
      // { value:'13', label:"小于等于" },
      // { value:'14', label:"区间" },
      // { value:'15', label:"区间(日期)" },
      // { value:'16', label:"在序列中" },
      // { value:'17', label:"不在序列中" },
    ],
    /*
Contains	0	包含
StartsWith	1	开头等于
EndsWith	2	结尾等于
NotContains	3	不包含
NotStartsWith	4	开头不等于
NotEndsWith	5	结尾不等于
Equal	6	等于
Equals	7	等于
Eq	8	等于
NotEqual	9	不等于
GreaterThan	10	大于
GreaterThanOrEqual	11	大于等于
LessThan	12	小于
LessThanOrEqual	13	小于等于
Range	14	区间
DateRange	15	区间(日期)
Any	16	在序列中
NotAny	17	不在序列中
Custom	18	

    */
  },
})

//const { proxy } = getCurrentInstance() as any
const editRules = reactive<FormRules>({
  /** 作者姓名 */
  authorName: [],
  /** 数据库表名 */
  tableName: [{ required: true, message: '请输入表名', trigger: 'blur' }],
  /** 命名空间 */
  namespace: [],
  /** 实体名称 */
  entityName: [],
  /** 业务名 */
  busName: [{ required: true, message: '请输入业务名', trigger: 'blur' }],
  /** Api分区名称 */
  apiAreaName: [],
  /** 基类名称 */
  baseEntity: [],
  /** 后端输出目录 */
  backendOut: [{ required: true, message: '请输入后端输出目录', trigger: 'blur' }],
  /** 前端输出目录 */
  frontendOut: [],
  dbMigrateSqlOut: [],
  menuAfterText: [],
})

const state = reactive({
  showDialog: false,
  isFull: false,
  sureLoading: false,
  title: '创建表',
  editor: 'infor',
  appendCount: 1,
  tableSize: 'default',
  config: {} as CodeGenGetOutput & CodeGenUpdateInput & any,
  // 界面显示用数据
  dbKeys: [] as Array<DatabaseGetOutput>,
})

const setTableSize = (size: string) => {
  state.tableSize = size
}

function _newCol(name: string, title: string, netType: string, options: CodeGenUpdateInput | any | null): CodeGenFieldGetOutput {
  if (undefined == netType || null == netType) netType = 'string'
  let col: CodeGenFieldGetOutput = {
    id: 0,
    dbKey: '',
    columnName: name,
    netType: netType,
    title: title,
    comment: '',
    dbType: '',
    isPrimary: false,
    isNullable: false,
    defaultValue: '',
    length: -1,
    editor: 'el-input',
    whetherCommon: false,
    whetherAdd: true,
    whetherUpdate: true,
    whetherQuery: false,
    whetherTable: true,
    whetherList: true,
    queryType: '6',
    displayColumn: '',
    valueColumn: '',
    pidColumn: '',
    effectType: 'input',
    dictTypeCode: '',
    includeEntity: '',
    includeMode: 0,
    includeEntityKey: '',
    frontendRuleTrigger: 'blur',
  }

  if (options != null) {
    for (let idx in options) {
      if (idx in col) col[idx as keyof CodeGenFieldGetOutput] = options[idx]
    }
  }
  return col
}

// 添加字段
const appendField = async (fieldType: number) => {
  console.log('append in editor')
  if (!state.config) return
  if (!state.config.fields) state.config.fields = [] as CodeGenFieldGetOutput[]
  var fields = state.config.fields

  if (fieldType == 1) {
    //普通
    for (var i = 0; i < state.appendCount; i++) {
      fields.push(_newCol('', '', 'string', { length: 64 }))
    }
  } else if (fieldType == 2) {
    //主键
    fields.push(
      _newCol('id', '序号', 'long', {
        comment: '主键',
        isPrimary: true,
        isNullable: false,
        whetherCommon: true,
        whetherAdd: false,
        whetherUpdate: false,
      })
    )
  } else if (fieldType == 3) {
    //租户
    fields.push(
      _newCol('TenantId', '租户', 'long', {
        comment: '租户',
        isPrimary: false,
        whetherCommon: true,
        whetherAdd: false,
        whetherUpdate: false,
        whetherTable: false,
      })
    )
  } else if (fieldType == 4) {
    //基础信息
    var cols = [
      ['CreatedUserId', 'long'],
      ['CreatedUserName', 'string'],
      ['CreatedUserRealName', 'string'],
      ['CreatedTime', 'DateTime'],
      ['ModifiedUserId', 'long'],
      ['ModifiedUserName', 'string'],
      ['ModifiedUserRealName', 'string'],
      ['ModifiedTime', 'DateTime'],
      ['IsDeleted', 'bool'],
    ]
    for (let idx in cols) {
      fields.push(
        _newCol(cols[idx][0], '', cols[idx][1], {
          comment: '',
          isPrimary: false,
          whetherCommon: true,
          whetherAdd: false,
          whetherUpdate: false,
          whetherTable: false,
        })
      )
    }
  }
}

// 删除字段
const removeField = async (row: CodeGenFieldGetOutput, index: number) => {
  if (!state.config?.fields) return
  state.config.fields.splice(index, 1)

  // 删除对应的错误状态
  if (fieldErrors.value[index]) {
    delete fieldErrors.value[index]
  }

  // 重新索引错误状态
  const newErrors: Record<number, Record<string, string>> = {}
  Object.keys(fieldErrors.value).forEach((key) => {
    const oldIndex = parseInt(key)
    if (oldIndex > index) {
      newErrors[oldIndex - 1] = fieldErrors.value[oldIndex]
    } else if (oldIndex < index) {
      newErrors[oldIndex] = fieldErrors.value[oldIndex]
    }
  })
  fieldErrors.value = newErrors
}

// 新增响应式数据存储字段校验错误
const fieldErrors = ref<Record<number, Record<string, string>>>({})

// 字段校验规则
const fieldRules = {
  columnName: [{ required: true, message: '列名不能为空', trigger: 'blur' }],
  editor: [{ required: true, message: '组件类型不能为空', trigger: 'blur' }],
}

// 检查字段是否有错误
const hasFieldError = (index: number, fieldName: string) => {
  return fieldErrors.value[index] && fieldErrors.value[index][fieldName]
}

// 获取字段错误信息
const getFieldError = (index: number, fieldName: string) => {
  return fieldErrors.value[index] && fieldErrors.value[index][fieldName]
}

// 校验单个字段
const validateField = (index: number, fieldName: string) => {
  const field = state.config.fields[index]
  const value = field[fieldName]
  const rules = fieldRules[fieldName as keyof typeof fieldRules]

  // 初始化错误对象
  if (!fieldErrors.value[index]) {
    fieldErrors.value[index] = {}
  }

  // 清除之前的错误
  delete fieldErrors.value[index][fieldName]

  // 应用校验规则
  for (const rule of rules) {
    if (rule.required && (!value || value.toString().trim() === '')) {
      fieldErrors.value[index][fieldName] = rule.message
      break
    }
  }

  // 如果没有错误，删除空的错误对象
  if (Object.keys(fieldErrors.value[index]).length === 0) {
    delete fieldErrors.value[index]
  }
}

// 校验所有字段
const validateAllFields = () => {
  // 清除所有现有错误
  fieldErrors.value = {}

  let hasErrors = false

  if (state.config.fields && state.config.fields.length > 0) {
    state.config.fields.forEach((field: any, index: any) => {
      // 校验列名
      if (!field.columnName || field.columnName.trim() === '') {
        if (!fieldErrors.value[index]) {
          fieldErrors.value[index] = {}
        }
        fieldErrors.value[index].columnName = '列名不能为空'
        hasErrors = true
      }

      // 校验组件类型
      if (!field.editor || field.editor.trim() === '') {
        if (!fieldErrors.value[index]) {
          fieldErrors.value[index] = {}
        }
        fieldErrors.value[index].editor = '组件类型不能为空'
        hasErrors = true
      }
    })
  }

  return !hasErrors
}

const onCancel = () => {
  state.showDialog = false
}

const onSure = () => {
  if (state.editor === 'field') {
    const isFieldsValid = validateAllFields()
    if (!isFieldsValid) {
      return
    }
  }

  tableInfoFromRef.value.validate(async (valid: boolean) => {
    if (!valid) {
      if (state.editor === 'field') {
        proxy.$modal.msgError('请检查基础配置中的必填项')
      }
      return
    }

    const isFieldsValid = validateAllFields()
    if (!isFieldsValid) {
      if (state.editor === 'infor') {
        proxy.$modal.msgError('请检查字段配置中的必填项')
      }
      return
    }

    state.sureLoading = true

    eventBus.emit('onConfigEditSure', {
      data: state.config,
      callback: function (res: AxiosResponse) {
        state.sureLoading = false
        if (res?.success) state.showDialog = false
      },
    })
  })
}

// 显示数据
const open = async (config: CodeGenGetOutput & CodeGenUpdateInput & any) => {
  state.showDialog = true
  state.title = config.id == 0 ? '创建表' : '编辑表：' + config.tableName
  if (config.importStatus == '等待导入') {
    state.title = '导入表' + config.tableName
    console.log(config.importKey)
  }
  state.config = config
  const res = await new CodeGenApi().getBaseData()
  state.dbKeys = res?.data?.databases ?? []
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.el-dialog__btn {
  cursor: pointer;
  color: var(--el-color-info);
  font-size: var(--el-message-close-size, 16px);
  width: 1em;
  height: 1em;
  & + & {
    margin-left: 8px;
  }
  &:hover {
    color: var(--el-color-primary);
  }
}

// 新增错误样式
.field-error {
  :deep() {
    .el-input__wrapper,
    .el-select__wrapper {
      box-shadow: 0 0 0 1px var(--el-color-error) inset;
    }
  }
}

.field-error-message {
  color: var(--el-color-error);
  font-size: 12px;
  line-height: 1;
  margin-top: 4px;
}

:deep() {
  .el-dialog {
    .my-dialog-table {
      height: calc(90vh - 203px);
    }
    &.is-fullscreen {
      .el-dialog__body {
        min-height: calc(100vh - 122px) !important;
      }
      .my-dialog-table {
        height: calc(100vh - 214px);
      }
    }
  }
  .el-checkbox {
    margin-right: 20px;
    &:last-of-type {
      margin-right: 0;
    }
  }

  .my-col--required {
    .cell:after {
      color: var(--el-color-danger);
      content: '*';
      margin-left: 4px;
    }
  }
}
</style>
