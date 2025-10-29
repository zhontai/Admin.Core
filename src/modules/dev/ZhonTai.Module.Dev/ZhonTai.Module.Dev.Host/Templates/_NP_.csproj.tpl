@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    var moduleNamePc = gen.ApiAreaName?.NamingPascalCase();
}
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <Description>@(gen.ApiAreaName)模块</Description>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <DocumentationFile>bin\$(MSBuildProjectName).xml</DocumentationFile>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\platform\ZhonTai.Admin\ZhonTai.Admin.csproj" />
  </ItemGroup>
</Project>
