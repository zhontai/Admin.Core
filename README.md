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
	<p>&nbsp;</p>
</div>

#### 🌈 介绍

基于 .Net8.0 + FreeSql 全家桶 + Autofac + CAP + Mapster 等技术，前后端分离后台权限管理系统。想你所想的开发理念，希望减少工作量，帮助大家实现快速开发。基于 FreeSql Orm 开发，支持国内外主流数据库、读写分离、分表分库、分布式事务 TCC/ SAGA 等功能。启动项目即自动生成数据库，CodeFirst 模式支持从实体配置自动同步表结构和权限数据至数据库。新版 swagger 接口文档更易于接口阅读和测试。

#### ⛱️ 线上预览

- Admin.Core v3 版本预览 <a href="https://admin.zhontai.net/login" target="_blank">https://admin.zhontai.net</a>

#### 💒 代码仓库

- Admin.Core v3 版本 <a href="https://github.com/zhontai/Admin.Core" target="_blank">https://github.com/zhontai/Admin.Core</a>

#### 🚀 功能介绍

1. 用户管理：配置用户，查看部门用户列表，支持禁用/启用、重置密码、设置主管、用户可配置多角色、多部门和上级主管。
2. 角色管理：配置角色，支持角色分组、设置角色菜单和数据权限、批量添加和移除角色员工。
3. 部门管理：配置部门，支持树形列表展示。
4. 权限管理：配置分组、菜单、操作、权限点、权限标识，支持树形列表展示。
5. 租户套餐：配置租户套餐，支持新增/移除套餐企业。
6. 租户管理：配置租户，新增租户时初始化部门、角色和管理员数据，支持租户配置套餐、禁用/启用功能。
7. 字典管理：配置字典，查看字典类型和字典数据列表，支持字典类型和字典数据维护。
8. 任务调度：查看任务和任务日志列表，支持任务启动、执行、暂停等功能。
9. 缓存管理：缓存列表查询，支持根据缓存键清除缓存
10. 接口管理：配置接口，支持接口同步功能，用于新增权限点选择接口，支持树形列表展示。
11. 视图管理：配置视图，支持视图维护功能，用于新增菜单选择视图，支持树形列表展示。
12. 文件管理：支持文件列表查询、文件上传/下载、查看大图、复制文件地址、删除文件功能。
13. 登录日志：登录日志列表查询，记录用户登录成功和失败日志。
14. 操作日志：操作日志列表查询，记录用户操作正常和异常日志。

#### ⚡ 使用说明

> 使用 .Net 最新版本 <a href="https://dotnet.microsoft.com/download/dotnet-core" target="_blank">.Net 版本 > 7.0+</a>

使用项目源码新建项目

```bash
# 克隆项目
git clone https://github.com/zhontai/Admin.Core.git

# 进入项目
cd Admin.Core

# 打开项目
打开 ZhonTai.sln 解决方案

# 运行项目
设置 ZhonTai.Host 为启动项目 Ctrl + F5 直接编译运行项目
或 在 ZhonTai.Host 目录打开 cmd 输入 dotnet run 命令运行项目

# 打包发布
选择 ZhonTai.Host 右键菜单点击发布
```

使用项目模板新建项目

```bash
# 安装模板
dotnet new install ZhonTai.Template

# 查看帮助
dotnet new MyApp -h

# 新建项目
dotnet new MyApp -n MyCompanyName.MyProjectName

# 运行项目
设置 MyCompanyName.MyProjectName.Host 为启动项目 Ctrl + F5 直接编译运行项目
或 在 MyCompanyName.MyProjectName.Host 目录打开 cmd 输入 dotnet run 命令运行项目

# 打包发布
选择 MyCompanyName.MyProjectName.Host 右键菜单点击发布
```

#### 📚 开发文档

- 查看开发文档：<a href="https://www.zhontai.net" target="_blank">https://zhontai.net</a>

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

#### ❤️ 鸣谢列表

- <a href="https://github.com/dotnet/core" target="_blank">.Net</a>
- <a href="https://github.com/autofac/Autofac" target="_blank">Autofac</a>
- <a href="https://github.com/MapsterMapper/Mapster" target="_blank">Mapster</a>
- <a href="https://github.com/dotnetcore/CAP" target="_blank">DotNetCore.CAP</a>
- <a href="https://github.com/NLog/NLog" target="_blank">NLog</a>
- <a href="https://github.com/yitter/idgenerator" target="_blank">Yitter.IdGenerator</a>
- <a href="https://github.com/JamesNK/Newtonsoft.Json" target="_blank">Newtonsoft.Json</a>
- <a href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore" target="_blank">Swashbuckle.AspNetCore</a>
- <a href="https://github.com/FluentValidation/FluentValidations" target="_blank">FluentValidation.AspNetCore</a>
- <a href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks" target="_blank">AspNetCore.Diagnostics.HealthChecks</a>
- <a href="https://github.com/MiniProfiler/dotnet" target="_blank">MiniProfiler</a>
- <a href="https://github.com/IdentityServer/IdentityServer4" target="_blank">IdentityServer4</a>
- <a href="https://github.com/stefanprodan/AspNetCoreRateLimit" target="_blank">AspNetCoreRateLimit</a>
- <a href="https://github.com/oncemi/OnceMi.AspNetCore.OSS" target="_blank">OnceMi.AspNetCore.OSS</a>
- <a href="https://gitee.com/pojianbing/lazy-slide-captcha" target="_blank">Lazy.SlideCaptcha.Core</a>
- <a href="https://github.com/ua-parser/uap-csharp" target="_blank">UAParser</a>

#### 💌 支持作者

如果觉得框架不错，或者已经在使用了，希望你可以去 <a target="_blank" href="https://github.com/zhontai/admin.core">Github</a> 或者
<a target="_blank" href="https://gitee.com/zhontai/admin.core">Gitee</a> 帮我点个 ⭐ Star，这将是对我极大的鼓励与支持。
