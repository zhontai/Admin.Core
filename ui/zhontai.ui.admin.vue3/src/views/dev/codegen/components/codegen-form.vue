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
        <div class="my-flex my-flex-between my-flex-items-center my-dialog-header" @dblclick="state.isFull = !state.isFull">
          <h4 :id="titleId" :class="titleClass">{{ state.title }}</h4>
          <div class="my-flex">
            <div class="el-dialog__btn">
              <el-icon v-if="state.isFull" @click="state.isFull = !state.isFull" :title="t('还原')"><ele-CopyDocument /></el-icon>
              <el-icon v-else @click="state.isFull = !state.isFull" :title="t('最大化')"><ele-FullScreen /></el-icon>
            </div>
            <div class="el-dialog__btn">
              <el-icon @click="close" :title="t('关闭')"><ele-Close /></el-icon>
            </div>
          </div>
        </div>
      </template>
      <template #footer>
        <span class="dialog-footer">
          <el-button auto-insert-space @click="onCancel">{{ t('取消') }}</el-button>
          <el-button auto-insert-space type="primary" @click="onSure">{{ t('确定') }}</el-button>
        </span>
      </template>
      <div>
        <div style="text-align: center; position: relative">
          <el-radio-group class="mb20" v-model="state.editor">
            <el-radio-button label="infor">{{ t('基础配置') }}</el-radio-button>
            <el-radio-button label="field">{{ t('字段配置') }}</el-radio-button>
          </el-radio-group>
          <div class="mb20" :style="{ position: themeConfig.isMobile ? 'relative' : 'absolute' }" style="top: 0; right: 0">
            <el-row v-show="state.editor == 'field'">
              <el-col style="text-align: right">
                <el-space wrap :size="12">
                  <el-dropdown split-button type="primary" placement="bottom-end" @click="appendField(1)">
                    {{ t('新增字段') }}
                    <template #dropdown>
                      <el-dropdown-menu>
                        <el-dropdown-item @click="appendField(2)">{{ t('新增主键字段') }}</el-dropdown-item>
                        <el-dropdown-item @click="appendField(3)">{{ t('新增租户字段') }}</el-dropdown-item>
                        <el-dropdown-item @click="appendField(4)">{{ t('新增通用字段') }}</el-dropdown-item>
                      </el-dropdown-menu>
                    </template>
                  </el-dropdown>
                  <el-input-number v-model="state.appendCount" :min="1" :max="20" style="width: 105px" controls-position="right">
                    <template #suffix>
                      <span>{{ t('行') }}</span>
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
              <el-form-item :label="t('数据库')" prop="dbKey" :rules="[{ required: true, message: t('请选择数据库'), trigger: ['change'] }]">
                <el-select v-model="state.config.dbKey" clearable>
                  <el-option v-for="item in state.dbKeys" :key="item.dbKey" :value="item.dbKey" :label="item.dbKey"></el-option>
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item :label="t('表名')" prop="tableName">
                <el-input v-model="state.config.tableName" required :placeholder="t('表名 eg:sys_lang')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item :label="t('业务名')" prop="busName">
                <el-input v-model="state.config.busName" required :placeholder="t('业务名 eg:国际化')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item :label="t('命名空间')" prop="namespace">
                <el-input v-model="state.config.namespace" required :placeholder="t('命名空间 eg:MyCompanyName.Module.MySys')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item :label="t('实体名')" prop="entityName">
                <el-input v-model="state.config.entityName" required :placeholder="t('留空时取表名大驼峰，会自动加上Entity')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item :label="t('基类')" prop="baseEntity">
                <el-select v-model="state.config.baseEntity" style="width: 100%">
                  <el-option v-for="item in entityBaseTypes" :key="item.value" :value="item.value" :label="item.label"></el-option>
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item :label="t('项目编码')" prop="apiAreaName">
                <el-input v-model="state.config.apiAreaName" :placeholder="t('项目编码 eg:sys')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item :label="t('上级菜单')" prop="menuPid">
                <el-input v-model="state.config.menuPid" :placeholder="t('eg:国际化管理')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item :label="t('菜单后缀')" prop="menuAfterText">
                <el-input v-model="state.config.menuAfterText" :placeholder="t('eg:列表')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
              <el-form-item :label="t('作者')" prop="authorName">
                <el-input v-model="state.config.authorName" required></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item :label="t('备注')" prop="comment"><el-input v-model="state.config.comment" style="width: 100%"></el-input> </el-form-item>
            </el-col>
            <!-- <el-col :xl="8" :lg="8" :md="12" :sm="12" :xs="24">
            <el-form-item :label="t('生成项')">
              <el-checkbox v-model="state.config.genEntity" :label="t('实体')"/>
              <el-checkbox v-model="state.config.genRepository" :label="t('仓储')"/>
              <el-checkbox v-model="state.config.genService" :label="t('服务')"/>
            </el-form-item>
          </el-col> -->
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item :label="t('服务接口')" :disabled="!state.config.genService">
                <el-checkbox v-model="state.config.genGet">{{ t('查询单条记录') }}</el-checkbox>
                <el-checkbox v-model="state.config.genGetPage">{{ t('分页查询') }}</el-checkbox>
                <el-checkbox v-model="state.config.genGetList">{{ t('列表查询') }}</el-checkbox>
                <el-checkbox v-model="state.config.genAdd">{{ t('新增') }}</el-checkbox>
                <el-checkbox v-model="state.config.genUpdate">{{ t('修改') }}</el-checkbox>
                <el-checkbox v-model="state.config.genSoftDelete">{{ t('删除') }}</el-checkbox>
                <el-checkbox v-model="state.config.genBatchSoftDelete">{{ t('批量删除') }}</el-checkbox>
                <el-checkbox v-model="state.config.genDelete">{{ t('彻底删除') }}</el-checkbox>
                <el-checkbox v-model="state.config.genBatchDelete">{{ t('批量彻底删除') }}</el-checkbox>
              </el-form-item>
            </el-col>

            <el-col :xl="12" :lg="12" :md="24" :sm="24" :xs="24">
              <el-form-item :label="t('后端输出目录')" prop="backendOut">
                <el-input v-model="state.config.backendOut" required :placeholder="t('后端输出目录 eg:E:\my-code-gen\backend')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="12" :lg="12" :md="24" :sm="24" :xs="24">
              <el-form-item :label="t('前端输出目录')" prop="frontendOut">
                <el-input v-model="state.config.frontendOut" required :placeholder="t('前端输出目录 eg:E:\my-code-gen\frontend')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item :label="t('脚本输出目录')" prop="dbMigrateSqlOut">
                <el-input v-model="state.config.dbMigrateSqlOut" :placeholder="t('脚本输出目录 eg:E:\my-code-gen\sql')"></el-input>
              </el-form-item>
            </el-col>
            <el-col :xl="24" :lg="24" :md="24" :sm="24" :xs="24">
              <el-form-item :label="t('命名导入')" prop="usings">
                <el-input
                  v-model="state.config.usings"
                  style="width: 100%"
                  :placeholder="t('在此导入的命令空间，列的外联类型可以直接使用类型')"
                ></el-input>
              </el-form-item>
            </el-col>
          </el-row>
        </el-form>

        <el-form :size="state.tableSize" v-show="state.editor == 'field'">
          <el-table class="my-dialog-table" :data="state.config.fields" border>
            <el-table-column fixed="right" width="50">
              <template #header>
                <el-dropdown @command="setTableSize" placement="bottom">
                  <span class="el-dropdown-link"> <i class="iconfont icon-xianshimima" :title="t('显示尺寸')"></i> </span>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="large">{{ t('松散') }}</el-dropdown-item>
                      <el-dropdown-item command="default">{{ t('正常') }}</el-dropdown-item>
                      <el-dropdown-item command="small">{{ t('紧凑') }}</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
              </template>
              <template #default="scope">
                <el-button type="danger" link icon="ele-Minus" @click="removeField(scope.row, scope.$index)" />
              </template>
            </el-table-column>
            <el-table-column prop="columnName" :label="t('属性名')" label-class-name="my-col--required" fixed width="150">
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
            <el-table-column prop="title" :label="t('属性说明')" width="140" fixed>
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.title"></el-input>
                <div v-else>{{ scope.row.title }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="netType" :label="t('属性类型')" width="130">
              <template #default="scope">
                <el-select v-if="!showMode" v-model="scope.row.netType">
                  <el-option v-for="item in netTypes" :key="item" :value="item" :label="item"></el-option>
                </el-select>
                <div v-else>{{ scope.row.netType }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="length" :label="t('长度')" width="85">
              <template #default="scope">
                <el-input-number v-if="!showMode" v-model="scope.row.length" :min="1" align="left" :controls="false" style="width: 100%">
                </el-input-number>
                <div v-else>{{ scope.row.length }}</div>
              </template>
            </el-table-column>

            <el-table-column prop="editor" :label="t('组件类型')" label-class-name="my-col--required" width="140">
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
            <el-table-column prop="whetherTable" :label="t('表')" width="55">
              <template #header>
                <div class="my-flex-y-center my-flex-wrap">
                  {{ t('表') }}
                  <el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>{{ t('表格分页列表显示字段') }}</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherTable" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="whetherAdd" :label="t('增')" width="55">
              <template #header>
                <div class="my-flex-y-center my-flex-wrap">
                  {{ t('增') }}
                  <el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>{{ t('新增页面显示的字段') }}</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherAdd" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="whetherUpdate" :label="t('改')" width="55">
              <template #header>
                <div class="my-flex-y-center my-flex-wrap">
                  {{ t('改') }}
                  <el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>{{ t('修改页面显示的字段') }}</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherUpdate" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>

            <el-table-column prop="whetherList" :label="t('列')" width="60">
              <template #header>
                <div class="my-flex-y-center my-flex-wrap">
                  {{ t('列') }}
                  <el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>{{ t('获取的精简数据，下拉框，选择框使用的字段') }}</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherList" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="isNullable" :label="t('空')" width="55">
              <template #header>
                <div class="my-flex-y-center my-flex-wrap">
                  {{ t('空') }}
                  <el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>{{ t('字段模型是否可空类型') }}</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.isNullable" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>

            <el-table-column prop="whetherQuery" :label="t('查')" width="60">
              <template #header>
                <div class="my-flex-y-center my-flex-wrap">
                  {{ t('查') }}
                  <el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>{{ t('查询条件显示的字段') }}</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-checkbox v-model="scope.row.whetherQuery" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="queryType" :label="t('查询方式')" width="130">
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
                <el-option key="input" value="input" :label="t('输入')"></el-option>
                <el-option key="dict" value="dict" :label="t('字典')"></el-option>
              </el-select>
              <div v-else>{{ scope.row.effectType }}</div>
            </template>
          </el-table-column> -->
            <el-table-column prop="includeEntity" width="140">
              <template #header>
                <div class="my-flex-y-center my-flex-wrap">
                  {{ t('关联模型') }}
                  <el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>{{ t('用于生成页面下拉选择，一对多则生成多选 eg: CodeGroupEntity') }}</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-input
                  v-if="!showMode"
                  v-model="scope.row.includeEntity"
                  :title="
                    t(
                      '用于生成页面下拉选择，一对多则生成多选 eg: CodeGroupEntity，一对一的时候将生成字段 字段_Text, 一对多的时候，模型中将生成 字段_Values，字段_Texts'
                    )
                  "
                  :placeholder="t('关联模型名 xxxEntity')"
                ></el-input>
                <div v-else>{{ scope.row.includeEntity }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="includeEntityKey" width="140">
              <template #header>
                <div class="my-flex-y-center">
                  {{ t('关联模型字段') }}
                  <el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>{{ t('列表和获取单个时显示文本值，根据Id查询关联模型，显示的字段 eg:Title，关联查询模型时的字段') }}</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <template #default="scope">
                <el-input
                  v-if="!showMode"
                  v-model="scope.row.includeEntityKey"
                  :title="t('列表和获取单个时显示文本值，根据Id查询关联模型，显示的字段 eg:Title')"
                  :placeholder="t('关联显示字段 Title')"
                ></el-input>
                <div v-else>{{ scope.row.includeEntityKey }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="includeMode" :label="t('外联方式')" width="150">
              <template #default="scope">
                <el-select v-if="!showMode" v-model="scope.row.includeMode" :title="t('对应单选/多选')">
                  <el-option v-for="item in includes" :key="item.value" :value="item.value" :label="item.label"></el-option>
                </el-select>
                <div v-else>{{ scope.row.includeMode }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="dictTypeCode" :label="t('字典编码')" width="120">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.dictTypeCode"></el-input>
                <div v-else>{{ scope.row.dictTypeCode }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="displayColumn" :label="t('选中文本字段')" width="140">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.displayColumn"></el-input>
                <div v-else>{{ scope.row.displayColumn }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="valueColumn" :label="t('选中值字段')" width="140">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.valueColumn"></el-input>
                <div v-else>{{ scope.row.valueColumn }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="pidColumn" :label="t('父级字段')" width="140">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.pidColumn"></el-input>
                <div v-else>{{ scope.row.pidColumn }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="frontendRuleTrigger" :label="t('前端检测触发')" width="140">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.frontendRuleTrigger"></el-input>
                <div v-else>{{ scope.row.frontendRuleTrigger }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="isPrimary" :label="t('主键')" width="60">
              <template #default="scope"
                ><!-- :disabled="showMode || editMode != 'add'"-->
                <el-checkbox v-model="scope.row.isPrimary" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="defaultValue" :label="t('默认值')" width="100">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.defaultValue"></el-input>
                <div v-else>{{ scope.row.defaultValue }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="indexMode" :label="t('索引')" width="120">
              <template #default="scope">
                <el-select v-if="!showMode" v-model="scope.row.indexMode">
                  <el-option value="" label="" />
                  <el-option value="ASC" label="ASC" />
                  <el-option value="DESC" label="DESC" />
                </el-select>
                <div v-else>{{ scope.row.indexMode }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="isUnique" :label="t('唯一')" width="60">
              <template #default="scope">
                <el-checkbox v-model="scope.row.isUnique" :disabled="showMode"></el-checkbox>
              </template>
            </el-table-column>
            <el-table-column prop="dataType" :label="t('数据类型')" width="150">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.dataType"></el-input>
                <div v-else>{{ scope.row.dataType }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="columnRawName" :label="t('列名')" width="150">
              <template #default="scope"
                ><!-- :disabled="editMode != 'add'"-->
                <el-input v-if="!showMode" v-model="scope.row.columnRawName"></el-input>
                <div v-else>{{ scope.row.columnRawName }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="comment" :label="t('列名说明')" width="200">
              <template #default="scope">
                <el-input v-if="!showMode" v-model="scope.row.comment"></el-input>
                <div v-else>{{ scope.row.comment }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="position" :label="t('列顺序')" width="80">
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
import { useThemeConfig } from '/@/stores/themeConfig'
import { t } from '/@/i18n'

const storesThemeConfig = useThemeConfig()
const { themeConfig } = storeToRefs(storesThemeConfig)

const { proxy } = getCurrentInstance() as any

const tableInfoFromRef = ref()

const props = defineProps({
  showMode: { type: Boolean, default: false },
  entityBaseTypes: {
    type: [] as PropType<Array<SelectOptionType>>,
    default: () => [
      { value: 'EntityBase', label: t('基础类型') },
      { value: 'EntityTenant', label: t('租户基础类型') },
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
    default: () => [
      { label: t('无组件'), value: 'none' },
      { label: t('文本框'), value: 'el-input' },
      { label: t('数值框'), value: 'el-input-number' },
      { label: t('文本域'), value: 'my-input-textarea' },
      { label: t('日期框'), value: 'el-date-picker' },
      { label: t('下拉框'), value: 'el-select' },
      { label: t('开关'), value: 'el-switch' },
      { label: t('复选框'), value: 'el-checkbox' },
      { label: t('图片上传'), value: 'my-upload' },
      { label: t('编辑器'), value: 'my-editor' },
      { label: t('业务选择'), value: 'my-bussiness-select' },
    ],
  },
  includes: {
    type: [] as PropType<Array<SelectOptionType>>,
    default: () => [
      { value: 0, label: t('1对1(单选)') },
      { value: 1, label: t('1对多(多选)') },
    ],
  },
  operates: {
    type: [] as PropType<Array<SelectOptionType>>,
    default: () => [
      { value: '0', label: t('包含') },
      // { value:'1', label: t('开头等于') },
      // { value:'2', label: t('结尾等于') },
      // { value:'3', label: t('不包含') },
      // { value:'4', label: t('开头不等于') },
      // { value:'5', label: t('结尾不等于') },
      { value: '6', label: t('等于Eq') },
      // { value:'7', label: t('等于Equal') },
      // { value:'8', label: t('等于Equals') },
      // { value:'9', label: t('不等于') },
      // { value:'10', label: t('大于') },
      // { value:'11', label: t('大于等于') },
      // { value:'12', label: t('小于') },
      // { value:'13', label: t('小于等于') },
      // { value:'14', label: t('区间') },
      // { value:'15', label: t('区间(日期)') },
      // { value:'16', label: t('在序列中') },
      // { value:'17', label: t('不在序列中') },
    ],
  },
})

//const { proxy } = getCurrentInstance() as any
const editRules = reactive<FormRules>({
  /** 作者姓名 */
  authorName: [],
  /** 数据库表名 */
  tableName: [{ required: true, message: t('请输入表名'), trigger: 'blur' }],
  /** 命名空间 */
  namespace: [],
  /** 实体名称 */
  entityName: [],
  /** 业务名 */
  busName: [{ required: true, message: t('请输入业务名'), trigger: 'blur' }],
  /** Api分区名称 */
  apiAreaName: [],
  /** 基类名称 */
  baseEntity: [],
  /** 后端输出目录 */
  backendOut: [{ required: true, message: t('请输入后端输出目录'), trigger: 'blur' }],
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
    dataType: '',
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
      _newCol('Id', '主键Id', 'long', {
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
      _newCol('TenantId', '租户Id', 'long', {
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
  columnName: [{ required: true, message: t('列名不能为空'), trigger: 'blur' }],
  editor: [{ required: true, message: t('组件类型不能为空'), trigger: 'blur' }],
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

const onSure = async () => {
  const isFieldsValid = validateAllFields()

  const isTableInfoValid = await tableInfoFromRef.value.validate((valid: boolean) => {})

  if (!isFieldsValid || !isTableInfoValid) {
    if (state.editor === 'infor') {
      if (isTableInfoValid) {
        state.editor = 'field'
        proxy.$modal.msgWarning(t('请检查字段配置中的必填项'))
      } else {
        proxy.$modal.msgWarning(t('请检查基础配置中的必填项'))
      }
    } else if (state.editor === 'field') {
      if (isFieldsValid) {
        state.editor = 'infor'
        proxy.$modal.msgWarning(t('请检查基础配置中的必填项'))
      } else {
        proxy.$modal.msgWarning(t('请检查字段配置中的必填项'))
      }
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
}

// 显示数据
const open = async (config: CodeGenGetOutput & CodeGenUpdateInput & any) => {
  fieldErrors.value = {}
  state.showDialog = true
  state.title = config.id == 0 ? t('创建表') : t('编辑表：') + config.tableName
  if (config.importStatus == '等待导入') {
    state.title = t('导入表') + config.tableName
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
  .el-overlay .el-overlay-dialog .el-dialog .el-dialog__header {
    padding: 0px;
  }
  .el-dialog {
    .my-dialog-header {
      padding: 16px;
    }
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
