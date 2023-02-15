#定义全局变量

$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $buildFolder "../"
$apiKey = $args[0]
if ([System.String]::IsNullOrWhiteSpace($apiKey)) 
{
    $apiKey = $env:NUGET_KEY
}
$sourceUrl = "http://localhost:5000/v3/index.json"

Write-Host "buildFolder:" $buildFolder
Write-Host "rootFolder:" $rootFolder

$nuGetOutputFolder = Join-Path $buildFolder "/packages"
Write-Host "NuGetOutputFolder:" $nuGetOutputFolder

#编译解决方案
$solutionPath = "../ZhonTai.sln";
# Write-Host $solutionPath
# pause

Write-Host "dotnet build -- start"
dotnet build $solutionPath -c Release
Write-Host "dotnet build -- end"

if($LASTEXITCODE -eq 0){
    #success
}
else{
   throw "Build Error!";
}
# pause

#打包之前先删除nuget包
Remove-Item "$nuGetOutputFolder/*" -recurse

# 指定项目打包
$projects = (
    "src\platform\ZhonTai.Admin",
    "src\platform\ZhonTai.ApiUI",
    "src\platform\ZhonTai.Common",
    "src\platform\ZhonTai.DynamicApi"
)

Write-Host "dotnet pack -- start"
foreach($project in $projects) {
$projectFolder = Join-Path $rootFolder $project
Write-Host "projectFolder:" $projectFolder
dotnet pack $projectFolder -p:TargetFrameworks=net7.0 --no-build -c Release /p:SourceLinkCreate=true /p:SolutionDir=$rootFolder -o $nuGetOutputFolder;
}
Write-Host "dotnet pack -- end"
# pause

Write-Host "dotnet nuget push -- start"
$allNuget = Join-Path $nuGetOutputFolder "/*.nupkg"
dotnet nuget push $allNuget -s $sourceUrl -k $apiKey --skip-duplicate 
Write-Host "dotnet nuget push -- end"

pause