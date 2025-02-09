MyFrameworK多模块开发框架模板说明

*********************************************************

### nuget下载地址
```
https://www.nuget.org/downloads
```
> 将`nuget.exe`放到 `E:\zhontai\templates\MyFramework` 目录下

### 查看模板列表
```
dotnet new list
```

### 生成nuget包
在 `F:\zhontai\templates\MyFramework` 目录下 cmd 执行以下命令生成nuget包
```
nuget pack E:\zhontai\templates\MyFramework\templates.nuspec -NoDefaultExcludes
```
### 安装模板
```
dotnet new install ZhonTai.Template.Framework
```
安装本地
```
dotnet new install E:\zhontai\templates\ZhonTai.Template.Framework.9.0.0.nupkg
```

### 创建项目
```
dotnet new MyFramework -n MyCompanyName.MyProjectName
```

### 卸载模板
```
dotnet new uninstall ZhonTai.Template.Framework
```
