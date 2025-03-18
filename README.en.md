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

A backend permission management system with frontend and backend separation, built on technologies such as .NET 9.0, FreeSql Suite, Autofac, Mapster, CAP, and more. It embraces a development philosophy that anticipates your needs, aiming to facilitate rapid development for everyone. Leveraging FreeSql ORM, it supports mainstream domestic and international databases, read-write separation, sharding, distributed transactions (TCC/SAGA), and other functionalities. Upon project initialization, the database is automatically generated. The CodeFirst mode enables automatic synchronization of table structures and permission data from entity configurations to the database. To explore the project, utilize the new version of Swagger API documentation to view interface request parameters and response data.

#### ⛱️ Online preview

- Admin.Core v3 version preview <a href="https://admin.zhontai.net/login" target="_blank">https://admin.zhontai.net</a>

#### 📚 Development documentation

- View development documentation：<a href="https://www.zhontai.net" target="_blank">https://zhontai.net</a>

#### 💒 Code repository

- Admin.Core v3 version <a href="https://github.com/zhontai/Admin.Core" target="_blank">https://github.com/zhontai/Admin.Core</a>

#### 🚀 Feature introduction

1. **User Management**: Manage and query users, supporting advanced search and user linkage by department. Users can be disabled/enabled, supervisors can be set/removed, passwords can be reset, multiple roles and departments can be configured, superior supervisors can be assigned, and one-click login to specified users is supported.
2. **Role Management**: Manage roles and role groups, supporting user linkage by role, setting menu and data permissions, and bulk adding and removing employees.
3. **Department Management**: Manage departments, supporting tree-list display.
4. **Permission Management**: Manage permission groups, menus, and permission points, supporting tree-list display.
5. **Tenant Packages**: Manage tenant packages, supporting menu permission settings for tenant packages and bulk adding and removing enterprises.
6. **Tenant Management**: Manage tenants, automatically initializing tenant departments, default roles, and administrators upon adding a new tenant. Supports configuring packages, disabling/enabling, and one-click login to tenant administrators.
7. **Dictionary Management**: Manage data dictionary categories and subcategories, supporting subcategory linkage by dictionary category, multi-column sorting on the server, data import and export.
8. **Task Scheduling**: Manage and view tasks and their run logs, supporting task creation, modification, deletion, starting, pausing, immediate execution, retry on failure, and sending alert emails.
9. **Cache Management**: Query cache lists and clear cache based on cache keys.
10. **API Management**: Manage APIs, supporting API synchronization functionality, primarily used for selecting APIs when adding permission points. Supports tree-list display, operation log request parameters, and response result configuration.
11. **View Management**: Manage views, used for selecting view components when adding menus. Supports tree-list display.
12. **File Management**: Manage file uploads, supporting file querying, uploading to OSS or locally, downloading, copying file addresses, deleting files, and viewing large images for image files.
13. **Region Management**: Manage and query regions, supporting disabling/enabling, setting/removing popular regions, and one-click synchronization of statistical bureau region data.
14. **Login Logs**: Query login log lists, recording successful and failed user login logs, with IP location tracking.
15. **Operation Logs**: Query operation log lists, recording normal and abnormal user operations, with IP location tracking and detailed operation log viewing.
16. **Personal Center**: This feature allows users to display and modify their personal information, view their last login information, change their passwords, and more.
17. **Message Classification**: Manage message classifications, supporting 2-level custom message classifications for message management and classification selection.
18. **Message Management**: Manage messages, support sending messages to specified users, and be able to check whether the user has read the message and the reading time.
19. **In-site Messages**: Manage in-site messages, supporting functions such as detailed message viewing, deletion, marking as read, and marking all as read. 

#### ⚡ Usage instructions

> Using the latest version of .NET <a href="https://dotnet.microsoft.com/download/dotnet-core" target="_blank">.Net version > 9.0+</a>

Create a new project using the source code of an existing project.

```bash
# Clone a project
git clone https://github.com/zhontai/Admin.Core.git

# Enter the project
cd Admin.Core

# Open the project
Open the ZhonTai.sln solution

# Run the project
Set ZhonTai.Admin.Host as the startup project, press Ctrl + F5 to build and run the project directly without debugging
Alternatively, navigate to the ZhonTai.Admin.Host directory in Command Prompt (cmd) and enter the dotnet run command to execute the project.

# Package and Publish
Select ZhonTai.Admin.Host, then right-click and choose Publish from the context menu.
```

Create a new project using a project template.

```bash
# Installation template
dotnet new install ZhonTai.Template.App

# View help
dotnet new MyApp -h

# New the project
dotnet new MyApp -n MyCompanyName.MySys -at sys -ac sys -p 16010 -gp 16011 -db MySql

# Run the project
Set MyCompanyName.MySys.Host as the startup project, press Ctrl + F5 to compile and run the project directly (without debugging)
Alternatively, navigate to the 'MyCompanyName.MySys.Host' directory in Command Prompt (cmd) and enter the 'dotnet run' command to execute the project.

# Package and Publish
Select MyCompanyName.MySys.Host, then right-click and choose Publish from the context menu.
```

Using Tye to Run & Debug Modular Projects:

1. Install Tye
```
dotnet tool install -g Microsoft.Tye --version "0.12.0-*" --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json
```

2. Run & Debug
```
1. Install the EasyRun extension for Visual Studio.

2. Open Visual Studio, navigate to View -> Other Windows -> EasyRun to open the EasyRun window.

3. Click the Tye button to run the application.

4. Select the microservice you want to debug and click the Debugger button to start debugging.
```


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
<a target="_blank" href="https://gitee.com/zhontai/Admin.Core">Gitee</a> Please give me a ⭐ Star, it would be a great encouragement and support to me.
