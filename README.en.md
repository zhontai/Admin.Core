<div align="center">
	<h2>zhontai admin</h2>
	<h3>Front-End and Back-End Separated Backend Permission Management System</h3>
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
    <span>English</span> |   
		<a href="README.md">中文</a>
	</p>
	<p>&nbsp;</p>
</div>

#### 🌈 Introduction

Based on technologies such as .NET 8.0, FreeSql full stack, Autofac, CAP, and Mapster, we have developed a front-end and back-end separated backend permission management system. With a development philosophy of anticipating your needs, we aim to reduce workload and help everyone achieve rapid development. Developed using FreeSql ORM, the system supports mainstream databases both domestically and internationally, read-write separation, table and database sharding, distributed transactions TCC/SAGA, and other features. Upon starting the project, the database is automatically generated, and the CodeFirst model supports automatic synchronization of table structures and permission data from entity configurations to the database. The new version of the Swagger interface documentation provides easier interface reading and testing.

#### ⛱️ Online preview

- Admin.Core v3 version preview <a href="https://admin.zhontai.net/login" target="_blank">https://admin.zhontai.net</a>

#### 💒 Code repository

- Admin.Core v3 version <a href="https://github.com/zhontai/Admin.Core" target="_blank">https://github.com/zhontai/Admin.Core</a>

#### 🚀 Feature introduction

1. User Management: Configure users, view departmental user lists, supports disabling/enabling, password resets, setting supervisors, users can be assigned multiple roles, departments, and higher-level supervisors.
2. Role Management: Configure roles, supports role grouping, setting role menus and data permissions, bulk adding and removing role members.
3. Department Management: Configure departments, supports tree-view list display.
4. Permission Management: Configure groups, menus, actions, permission points, and permission identifiers, supports tree-view list display.
5. Tenant Packages: Configure tenant packages, supports adding/removing package enterprises.
6. Tenant Management: Configure tenants, initialize department, role, and administrator data when adding new tenants, supports tenant package configuration, disabling/enabling features.
7. Dictionary Management: Configure dictionaries, view dictionary types and dictionary data lists, supports maintenance of dictionary types and dictionary data, can export dictionary subclass data.
8. Task Scheduling: View task and task log lists, supports adding new tasks, modifying, starting, executing, pausing, copying tasks, and viewing logs.
9. Cache Management: Query cache lists, supports clearing caches based on cache keys.
10. Interface Management: Configure interfaces, supports interface synchronization for adding permission points and selecting interfaces, supports tree-view list display.
11. View Management: Configure views, supports view maintenance for adding menus and selecting views, supports tree-view list display.
12. File Management: Supports file list queries, file uploads/downloads, viewing large images, copying file addresses, and deleting files.
13. Regional Management: managing and querying regions, supporting the disabling/enabling of regions, setting/canceling popular regions, and one-click synchronization of statistical bureau's regional data.
14. Login Logs: Query login log lists, records user successful and failed login attempts.
15. Operation Logs: Query operation log lists, records user normal and abnormal operation logs.

#### ⚡ Usage instructions

> Using the latest version of .NET <a href="https://dotnet.microsoft.com/download/dotnet-core" target="_blank">.Net version > 7.0+</a>

Create a new project using the source code of an existing project.

```bash
# Clone a project
git clone https://github.com/zhontai/Admin.Core.git

# Enter the project
cd Admin.Core

# Open the project
Open the ZhonTai.sln solution

# Run the project
Set ZhonTai.Host as the startup project, press Ctrl + F5 to build and run the project directly without debugging
Alternatively, navigate to the ZhonTai.Host directory in Command Prompt (cmd) and enter the dotnet run command to execute the project.

# Package and Publish
Select ZhonTai.Host, then right-click and choose Publish from the context menu.
```

Create a new project using a project template.

```bash
# Installation template
dotnet new install ZhonTai.Template

# View help
dotnet new MyApp -h

# New the project
dotnet new MyApp -n MyCompanyName.MyProjectName

# Run the project
Set MyCompanyName.MyProjectName.Host as the startup project, press Ctrl + F5 to compile and run the project directly (without debugging)
Alternatively, navigate to the 'MyCompanyName.MyProjectName.Host' directory in Command Prompt (cmd) and enter the 'dotnet run' command to execute the project.

# Package and Publish
Select MyCompanyName.MyProjectName.Host, then right-click and choose Publish from the context menu.
```

#### 📚 Development documentation

- View development documentation：<a href="https://www.zhontai.net" target="_blank">https://zhontai.net</a>

#### 💯 Join a QQ group for learning and communication

> Zhontai Admin development group (2000-member capacity)

- QQ Group Number：<a target="_blank" href="//qm.qq.com/cgi-bin/qm/qr?k=zjVRMcdD_oxPokw7zG1kv8Ud4kPJUZAk&jump_from=webapi&authKey=smP6idH1QaIqi6NSiBck8nZuY1BokW4fpi/IGcRi6w/Xt/HTyqfqrC5WpVRsSi22">1058693879</a>

  <a target="_blank" href="//qm.qq.com/cgi-bin/qm/qr?k=zjVRMcdD_oxPokw7zG1kv8Ud4kPJUZAk&jump_from=webapi&authKey=smP6idH1QaIqi6NSiBck8nZuY1BokW4fpi/IGcRi6w/Xt/HTyqfqrC5WpVRsSi22">
  	<img src="https://zhontai.net/images/qq-group-1058693879.png" width="220" height="220" alt="Zhontai Admin development group" title="Zhontai Admin development group"/>
  </a>

#### 💕 Special thanks

- <a href="https://github.com/dotnetcore/FreeSql" target="_blank">FreeSql</a>
- <a href="https://github.com/2881099/FreeRedis" target="_blank">FreeRedis</a>
- <a href="https://github.com/2881099/FreeSql.Cloud" target="_blank">FreeSql.Cloud</a>
- <a href="https://github.com/2881099/FreeScheduler" target="_blank">FreeScheduler</a>

#### ❤️ Acknowledgments list

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

#### 💌 Support the author

If you think the framework is good, or if you are already using it, we hope you can go to <a target="_blank" href="https://github.com/zhontai/admin.core">Github</a> or
<a target="_blank" href="https://gitee.com/zhontai/admin.core">Gitee</a> Please give me a ⭐ Star, it would be a great encouragement and support to me.
