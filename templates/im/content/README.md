
MyIMServer即时通讯服务端项目模板说明

*********************************************************

### nuget下载地址
```
https://www.nuget.org/downloads
```
> 将`nuget.exe`放到 `F:\zhontai\Admin.Core\templates` 目录下

### 查看模板列表
```
dotnet new list
```

### 生成nuget包
在 `F:\zhontai\Admin.Core\templates` 目录下 cmd 执行以下命令生成nuget包
```
nuget pack F:\zhontai\Admin.Core\templates\im\templates.nuspec -NoDefaultExcludes
```
### 安装模板
```
dotnet new install ZhonTai.Template.IMServer
```
安装本地
```
dotnet new install F:\zhontai\Admin.Core\templates\ZhonTai.Template.IMServer.1.0.0.nupkg
```

### 创建项目
```
dotnet new MyIMServer -n MyCompanyName.IMServer
```

### 卸载模板
```
dotnet new uninstall ZhonTai.Template.IMServer
```
