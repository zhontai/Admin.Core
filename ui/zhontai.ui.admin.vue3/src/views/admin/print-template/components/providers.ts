import { hiprint } from 'vue-plugin-hiprint'
import logoImg from '/@/assets/logo-mini.svg'

export const comProvider = function () {
  var addElementTypes = function (context: any) {
    context.removePrintElementTypes('comModule')
    context.addPrintElementTypes('comModule', [
      new hiprint.PrintElementTypeGroup('常用组件', [
        {
          tid: 'comModule.text',
          title: '文本',
          data: '',
          type: 'text',
        },
        {
          tid: 'comModule.longText',
          title: '长文本',
          data: '',
          type: 'longText',
        },

        {
          tid: 'comModule.table',
          field: 'table',
          title: '表格',
          type: 'table',
          groupFields: ['name'],
          groupFooterFormatter: function (group: any, option: any) {
            return '这里自定义统计脚信息'
          },
          columns: [
            [
              {
                title: '行号',
                fixed: true,
                rowspan: 2,
                field: 'id',
                width: 70,
              },
              { title: '人员信息', colspan: 2 },
              { title: '销售统计', colspan: 2 },
            ],
            [
              {
                title: '姓名',
                align: 'left',
                field: 'name',
                width: 100,
              },
              { title: '性别', field: 'gender', width: 100 },
              {
                title: '销售数量',
                field: 'count',
                width: 100,
              },
              {
                title: '销售金额',
                field: 'amount',
                width: 100,
              },
            ],
          ],
          editable: true,
          columnDisplayEditable: true, //列显示是否能编辑
          columnDisplayIndexEditable: true, //列顺序显示是否能编辑
          columnTitleEditable: true, //列标题是否能编辑
          columnResizable: true, //列宽是否能调整
          columnAlignEditable: true, //列对齐是否调整
          isEnableEditField: true, //编辑字段
          isEnableContextMenu: true, //开启右键菜单 默认true
          isEnableInsertRow: true, //插入行
          isEnableDeleteRow: true, //删除行
          isEnableInsertColumn: true, //插入列
          isEnableDeleteColumn: true, //删除列
          isEnableMergeCell: true, //合并单元格
        },
        {
          tid: 'comModule.emptyTable',
          title: '空白表格',
          type: 'table',
          columns: [
            [
              {
                title: '',
                field: '',
                width: 100,
              },
              {
                title: '',
                field: '',
                width: 100,
              },
            ],
          ],
        },
        {
          tid: 'comModule.html',
          title: 'html',
          formatter: function (data: any, options: any) {
            return '<div style="height:50pt;width:50pt;background:red;border-radius: 50%;"></div>'
          },
          type: 'html',
        },
      ]),
      new hiprint.PrintElementTypeGroup('辅助组件', [
        {
          tid: 'comModule.hline',
          title: '横线',
          type: 'hline',
        },
        {
          tid: 'comModule.vline',
          title: '竖线',
          type: 'vline',
        },
        {
          tid: 'comModule.rect',
          title: '矩形',
          type: 'rect',
        },
        {
          tid: 'comModule.oval',
          title: '椭圆',
          type: 'oval',
        },
        {
          tid: 'comModule.barcode',
          title: '条形码',
          type: 'barcode',
        },
        {
          tid: 'comModule.qrcode',
          title: '二维码',
          type: 'qrcode',
        },
      ]),
    ])
  }
  return {
    addElementTypes: addElementTypes,
  }
}

export default [
  {
    name: '常用组件',
    value: 'comModule',
    type: 1,
    f: comProvider(),
  },
]
