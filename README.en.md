<div align="center">
	<h2>zhontai admin</h2>
	<h3>Front-End and Back-End Separated Backend Permission Management System</h3>
	<p align="center">
		<a href="https://learn.microsoft.com/zh-cn/aspnet/core/introduction-to-aspnet-core" target="_blank">
			<img src="https://img.shields.io/badge/.NET-10.x-green" alt=".Net">
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

A backend permission management system with frontend and backend separation, built on technologies such as .NET 10.0, FreeSql Suite, Autofac, Mapster, CAP, and more. It embraces a development philosophy that anticipates your needs, aiming to facilitate rapid development for everyone. Leveraging FreeSql ORM, it supports mainstream domestic and international databases, read-write separation, sharding, distributed transactions (TCC/SAGA), and other functionalities. Upon project initialization, the database is automatically generated. The CodeFirst mode enables automatic synchronization of table structures and permission data from entity configurations to the database. To explore the project, utilize the new version of Swagger API documentation to view interface request parameters and response data.

#### ⛱️ Online preview

- Admin.Core vue3 version preview <a href="https://admin.zhontai.net/login" target="_blank">https://admin.zhontai.net</a>  
  account：user 
  password：123asd

#### 📚 Development documentation

- View development documentation：<a href="https://www.zhontai.net" target="_blank">https://zhontai.net</a>

#### 💒 Code repository

- Admin.Core v3 version <a href="https://github.com/zhontai/Admin.Core" target="_blank">https://github.com/zhontai/Admin.Core</a>

#### 🚀 Feature introduction

Here is the translated document with numbered items:

1. **User Management**: Manage and query users, supporting advanced query schemes and department-linked user retrieval. Users can be enabled/disabled, set/unset as supervisors, have passwords reset, be configured with multiple roles and departments, allow one-click login for specified users, view online/offline status, be forced offline, transferred between departments, and managed via recycle bin.  
2. **Role Management**: Manage roles and role groups, supporting role-based user linkage, menu and data permission settings, and batch addition/removal of employees.  
3. **Department Management**: Manage departments, supporting tree-structured lists and graphical displays.  
4. **Permission Management**: Manage permission groups, menus, and permission points across multiple platforms. Permission points can be configured with multiple interface addresses and displayed in a tree-structured list.  
5. **Tenant Plans**: Manage tenant plans, supporting menu permission settings and batch addition/removal of plan-associated enterprises.  
6. **Tenant Management**: Manage tenants. New tenants are automatically initialized with default departments, roles, and administrators. Supports plan configuration, enabling/disabling, and one-click login for tenant administrators.  
7. **Dictionary Management**: Manage data dictionary categories and subcategories, supporting category-linked subcategory retrieval. Subcategories feature server-side multi-column sorting, data import, and export functions.  
8. **Task Scheduling**: Manage and view tasks along with their execution logs. Supports task creation, modification, deletion, starting, pausing, immediate execution, failure retries, and alert email notifications.  
9. **Cache Management**: Query cache lists and clear caches based on cache keys.  
10. **Interface Management**: Manage interfaces, supporting synchronization functions. Primarily used for selecting interfaces when adding permission points. Features tree-structured lists, operation log toggles, and configuration of request parameters and response results in log details.  
11. **View Management**: Manage views across multiple platforms, used for selecting view components when adding menus. Supports tree-structured list display.  
12. **File Management**: Manage file uploads, supporting file queries, uploading to OSS or local storage, downloading, copying file addresses, deleting files, and enlarged image previews for pictures.  
13. **Region Management**: Manage and query regions, supporting enabling/disabling, setting/unsetting popular regions, and one-click synchronization of national administrative division data.  
14. **Login Logs**: Query login logs, recording successful and failed login attempts with IP location tracking.  
15. **Operation Logs**: Query operation logs, recording normal and abnormal user operations with IP location tracking and detailed log viewing.  
16. **Personal Center**: Display and modify personal information, including password, phone number, and email updates.  
17. **Message Categories**: Manage message categories, supporting 2-level custom classifications for message management selection.  
18. **Message Management**: Manage messages, supporting targeted user messaging with read status and timestamp tracking.  
19. **Inbox**: Manage internal messages, supporting detailed viewing, deletion, marking as read, and marking all as read.  
20. **Print Templates**: Manage print templates, supporting component drag-and-drop, JSON data source configuration, designer tools, parameter initialization, paper selection/customization, scaling, layout adjustments, printing, previewing, template JSON viewing, and saving/refreshing templates.

#### ⚡ Usage instructions

> Using the latest version of .NET <a href="https://dotnet.microsoft.com/download/dotnet-core" target="_blank">.NET version > 10.0+</a>

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

Running the Frontend Project  

1. Clone the Code  
   The frontend project is located in the `ui/zhontai.ui.admin.vue3` directory.  

2. Set Up Registry (Optional - Faster Chinese Mirror)  
   ```bash
   # Install nrm (NPM registry manager)
   npm install -g nrm --registry https://registry.npmmirror.com
   
   # List available registries
   nrm ls
   
   # Switch to npm mirror
   nrm use npm
   ```

3. Install pnpm  
   ```bash
   npm install -g pnpm
   ```

4. Install Dependencies  
   ```bash
   pnpm install
   ```

5. Run the Development Server  
   ```bash
   pnpm dev
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

#### 💌 Support the author

If you think the framework is good, or if you are already using it, we hope you can go to <a target="_blank" href="https://github.com/zhontai/admin.core">Github</a> or
<a target="_blank" href="https://gitee.com/zhontai/Admin.Core">Gitee</a> Please give me a ⭐ Star, it would be a great encouragement and support to me.
