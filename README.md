<div align="center">
	<h2>中台admin</h2>
	<h3>前后端分离后台权限管理系统</h3>
	<p align="center">
		<a href="https://learn.microsoft.com/zh-cn/aspnet/core/introduction-to-aspnet-core" target="_blank">
			<img src="https://img.shields.io/badge/.Net-8.x-green" alt=".Net">
		</a>
		<a href="https://freesql.net" target="_blank">
			<img src="https://img.shields.io/nuget/v/FreeSql?label=FreeSql&color=blue" alt="FreeSql">
		</a>
		<a href="https://autofac.org" target="_blank">
		    <img src="https://img.shields.io/nuget/v/Autofac?label=Autofac&color=blueviolet" alt="Autofac">
		</a>
		<a href="https://github.com/rivenfx/Mapster-docs" target="_blank">
			<img src="https://img.shields.io/nuget/v/Mapster?label=Mapster&color=orange" alt="Mapster">
		</a>
		<a href="https://cap.dotnetcore.xyz" target="_blank">
			<img src="https://img.shields.io/nuget/v/DotNetCore.CAP?label=CAP&color=yellow" alt="DotNetCore.CAP">
		</a>
		<a href="https://github.com/zhontai/admin.ui.plus/blob/master/LICENSE" target="_blank">
			<img src="https://img.shields.io/badge/license-MIT-success" alt="license">
		</a>
	</p>
	<p align="center">
		<span>中文</span> |   
    <a href="README.en.md">English</a>
	</p>
	<p>&nbsp;</p>
</div>

#### 🌈 介绍

基于 .Net9.0 + FreeSql 全家桶 + Autofac + Mapster + CAP 等技术，前后端分离后台权限管理系统。想你所想的开发理念，帮助大家快速上手开发。基于 FreeSql Orm 开发，支持国内外主流数据库、读写分离、分表分库、分布式事务 TCC/ SAGA 等功能。启动项目即自动生成数据库，CodeFirst 模式支持从实体配置自动同步表结构和权限数据到数据库中。启动项目使用新版 swagger 接口文档查看接口请求参数和响应数据。

#### ⛱️ 线上预览

- Admin.Core v3 版本预览 <a href="https://admin.zhontai.net/login" target="_blank">https://admin.zhontai.net</a>  
  账号：user 密码：123asd

#### 📚 开发文档

- 查看开发文档：<a href="https://www.zhontai.net" target="_blank">https://zhontai.net</a>

#### 💒 代码仓库

- Admin.Core v3 版本 <a href="https://github.com/zhontai/Admin.Core" target="_blank">https://github.com/zhontai/Admin.Core</a>

#### 🚀 功能介绍

1. **用户管理**：管理和查询用户，支持高级查询方案和按部门联动用户，用户可禁用/启用、设置/取消主管、重置密码、配置多角色和多部门、一键登录指定用户、查看在线离线状态、强制下线、部门转移、回收站。
2. **角色管理**：管理角色和角色分组，支持按角色联动用户，设置菜单和数据权限，批量添加和移除员工。
3. **部门管理**：管理部门，支持树形列表和图形展示。
4. **权限管理**：多平台管理权限分组、菜单和权限点，权限点可设置多个接口地址，支持树形列表展示。
5. **租户套餐**：管理租户套餐，支持设置菜单权限、批量添加和移除套餐企业。
6. **租户管理**：管理租户，新增租户后自动初始化租户部门、默认角色和管理员。支持配置套餐、禁用/启用、一键登录租户管理员功能。
7. **字典管理**：管理数据字典大类及其小类，支持按字典大类联动字典小类、字典小类有服务端多列排序、数据导入和导出功能。
8. **任务调度**：管理和查看任务及其任务运行日志，支持任务新增、修改、删除、启动、暂停、立即执行、失败重试、发送告警邮件功能。
9. **缓存管理**：缓存列表查询，支持根据缓存键清除缓存
10. **接口管理**：管理接口，支持接口同步功能，主要用于新增权限点时选择接口，支持树形列表展示、操作日志开关、操作日志详情请求参数和响应结果配置。
11. **视图管理**：管理视图，多平台视图，用于新增菜单时选择视图组件，支持树形列表展示。
12. **文件管理**：管理文件上传，支持文件查询、上传到OSS或本地、下载、复制文件地址、删除文件、图片支持查看大图功能。
13. **地区管理**：管理和查询地区，支持禁用/启用、设置/取消热门地区、一键同步全国行政区划地区数据
14. **登录日志**：登录日志列表查询，记录用户登录成功和失败日志，支持IP归属地记录。
15. **操作日志**：操作日志列表查询，记录用户操作正常和异常日志，支持IP归属地记录，查看操作日志详情。
16. **个人中心**：个人信息展示和基本信息修改， 支持个人密码、手机和邮箱修改。
17. **消息分类**：管理消息分类，支持2级自定义消息分类，用于消息管理消息分类选择。
18. **消息管理**：管理消息，支持发送指定用户消息，可查看用户是否已读和已读时间。
19. **站内信**：站内消息管理，支持消息详细查看、删除、标为已读、全部已读功能。
20. **打印模板**：打印模板管理，支持组件拖拽、Json数据源配置、设计器、配置参数初始化、选择和自定义纸张、缩放、排版、打印、预览、查看模板 JSON 、保存和刷新打印模板功能。

#### ⚡ 使用说明

> 使用 .Net 最新版本 <a href="https://dotnet.microsoft.com/download/dotnet-core" target="_blank">.Net 版本 >= 9.0+</a>

- 使用项目源码新建项目

```bash
# 克隆项目
git clone https://github.com/zhontai/Admin.Core.git

# 进入项目
cd Admin.Core

# 打开项目
打开 ZhonTai.sln 解决方案

# 运行项目
设置 ZhonTai.Admin.Host 为启动项目 Ctrl + F5 直接编译运行项目
或 在 ZhonTai.Admin.Host 目录打开 cmd 输入 dotnet run 命令运行项目

# 打包发布
选择 ZhonTai.Admin.Host 右键菜单点击发布
```

- 使用项目模板新建项目

```bash
# 安装模板
dotnet new install ZhonTai.Template.App

# 查看帮助
dotnet new MyApp -h

# 新建项目
dotnet new MyApp -n MyCompanyName.MySys -at sys -ac sys -p 16010 -gp 16011 -db MySql

# 运行项目
设置 MyCompanyName.MySys.Host 为启动项目 Ctrl + F5 直接编译运行项目
或 在 MyCompanyName.MySys.Host 目录打开 cmd 输入 dotnet run 命令运行项目

# 打包发布
选择 MyCompanyName.MySys.Host 右键菜单点击发布
```

使用Tye运行&调试模块项目：

1、安装Tye
```
dotnet tool install -g Microsoft.Tye --version "0.12.0-*" --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json
```

2、运行&调试
```
1、vs安装拓展EasyRun

2、打开VS的 视图-> 其他窗口 -> EasyRun，点击打开EasyRun窗口

3、点击Tye按钮运行

4、选择要调试的微服务点击Debugger按钮开启调试
```

运行前端项目

1、克隆代码

前端项目位于`ui\zhontai.ui.admin.vue3`文件夹内

2、安装nrm
```
npm install -g nrm --registry https://registry.npmmirror.com

#通过 nrm 列出所有可用的镜像源
nrm ls

#通过 nrm 使用淘宝的镜像源
nrm use taobao
```

3、安装pnpm
```
npm install -g nrm --registry https://registry.npmmirror.com
```

4、安装 npm 包
```
pnpm run install:pkg
```

5、运行项目
```
pnpm run dev
```

#### 💯 学习交流加 QQ 群

> 中台 admin 开发群（2000 人群）。

- QQ 群号：<a target="_blank" href="//qm.qq.com/cgi-bin/qm/qr?k=zjVRMcdD_oxPokw7zG1kv8Ud4kPJUZAk&jump_from=webapi&authKey=smP6idH1QaIqi6NSiBck8nZuY1BokW4fpi/IGcRi6w/Xt/HTyqfqrC5WpVRsSi22">1058693879</a>

  <a target="_blank" href="//qm.qq.com/cgi-bin/qm/qr?k=zjVRMcdD_oxPokw7zG1kv8Ud4kPJUZAk&jump_from=webapi&authKey=smP6idH1QaIqi6NSiBck8nZuY1BokW4fpi/IGcRi6w/Xt/HTyqfqrC5WpVRsSi22">
  	<img src="https://zhontai.net/images/qq-group-1058693879.png" width="220" height="220" alt="中台admin 开发群" title="中台admin 开发群"/>
  </a>

#### 💕 特别感谢

- <a href="https://github.com/dotnetcore/FreeSql" target="_blank">FreeSql</a>
- <a href="https://github.com/2881099/FreeRedis" target="_blank">FreeRedis</a>
- <a href="https://github.com/2881099/FreeSql.Cloud" target="_blank">FreeSql.Cloud</a>
- <a href="https://github.com/2881099/FreeScheduler" target="_blank">FreeScheduler</a>

#### 💌 支持作者

如果觉得框架不错，或者已经在使用了，希望你可以去 <a target="_blank" href="https://github.com/zhontai/admin.core">Github</a> 或者
<a target="_blank" href="https://gitee.com/zhontai/Admin.Core">Gitee</a> 帮我点个 ⭐ Star，这将是对我极大的鼓励与支持。
