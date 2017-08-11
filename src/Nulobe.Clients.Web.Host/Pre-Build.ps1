$solutionDir = $args[0]
$projectDir = $args[1]
$nswagExePath = $args[2]

# $solutionDir = 'C:\Users\Michael\Source\Repos\VeganFacts'
# $projectDir = 'C:\Users\Michael\Source\Repos\VeganFacts\src\Nulobe.Clients.Web.Host'
# $nswagExePath = 'C:\Users\Michael\.nuget\packages\nswag.msbuild\11.3.3\build\NSwag.exe'

# Execute Copy-Files.ps1
$copyFilesScriptPath = [io.path]::Combine($solutionDir, 'Copy-Files.ps1')
$copyFilesScriptArgs = "$solutionDir $projectDir"
Invoke-Expression "& `"$copyFilesScriptPath`" $copyFilesScriptArgs"

# Execute NSwag
$nswagExeArgs = "swagger2tsclient /input:`"$projectDir\..\Nulobe.Api\swagger.json`" /output:`"$projectDir/../Nulobe.Clients.Web/src/app/core/api/api.swagger.ts`" /className:`"{controller}Client`" /typescriptVersion:2.0 /template:`"Angular`" /promiseType:`"Promise`" /dateTimeType:`"Date`" /generateClientInterfaces:true /generateDtoTypes:true /typeStyle:`"Interface`""
Start-Process -NoNewWindow -PassThru -Wait -FilePath $nswagExePath -ArgumentList $nswagExeArgs