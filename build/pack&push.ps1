#定义全局变量

$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $buildFolder "../"
$apiKey = ""
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
    "src\plates\ZhonTai.Plate.Admin\ZhonTai.Plate.Admin.Domain",
    "src\plates\ZhonTai.Plate.Admin\ZhonTai.Plate.Admin.HttpApi",
    "src\plates\ZhonTai.Plate.Admin\ZhonTai.Plate.Admin.Repository",
    "src\plates\ZhonTai.Plate.Admin\ZhonTai.Plate.Admin.Service",
    "src\plates\ZhonTai.Plate.Personal\ZhonTai.Plate.Personnel.Domain",
    "src\plates\ZhonTai.Plate.Personal\ZhonTai.Plate.Personnel.HttpApi",
    "src\plates\ZhonTai.Plate.Personal\ZhonTai.Plate.Personnel.Repository",
    "src\plates\ZhonTai.Plate.Personal\ZhonTai.Plate.Personnel.Service"
)

Write-Host "dotnet pack -- start"
foreach($project in $projects) {
$projectFolder = Join-Path $rootFolder $project
Write-Host "projectFolder:" $projectFolder
dotnet pack $projectFolder --no-build -c Release /p:SourceLinkCreate=true /p:SolutionDir=$rootFolder -o $nuGetOutputFolder;
}
Write-Host "dotnet pack -- end"
# pause

Write-Host "dotnet nuget push -- start"
$allNuget = Join-Path $nuGetOutputFolder "/*.nupkg"
dotnet nuget push $allNuget -s $sourceUrl -k $apiKey --skip-duplicate 
Write-Host "dotnet nuget push -- end"

pause