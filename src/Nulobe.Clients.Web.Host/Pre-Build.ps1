$solutionDir = $args[0]
$projectDir = $args[1]
$nswagExePath = $args[2]

# $solutionDir = 'D:\Source2\Nulobe'
# $projectDir = 'D:\Source2\Nulobe\src\Nulobe.Clients.Web.Host'
# $nswagExePath = 'C:\Users\Michael Fry-White\.nuget\packages\nswag.msbuild\11.3.3\build\NSwag.exe'

# Execute Copy-App-Settings.ps1
$copyFilesScriptPath = [io.path]::Combine($solutionDir, 'Copy-App-Settings.ps1')
$copyFilesScriptArgs = "$solutionDir $projectDir"
Invoke-Expression "& `"$copyFilesScriptPath`" $copyFilesScriptArgs"

# Execute NSwag
$nswagExeArgs = "swagger2tsclient /input:`"$projectDir\..\Nulobe.Api\swagger.json`" /output:`"$projectDir/../Nulobe.Clients.Web/src/app/core/api/api.swagger.ts`" /className:`"{controller}Client`" /typescriptVersion:2.0 /template:`"Angular`" /promiseType:`"Promise`" /dateTimeType:`"Date`" /generateClientInterfaces:true /generateDtoTypes:true /typeStyle:`"Interface`""
Start-Process -NoNewWindow -PassThru -Wait -FilePath `"$nswagExePath`" -ArgumentList $nswagExeArgs

# Create symbolic link for submodules
$ngxChipsSource = [io.path]::Combine($solutionDir, 'submodules', 'ngx-chips', 'modules')
$ngxChipsDestination = [io.path]::Combine($solutionDir, 'src', 'Nulobe.Clients.Web', 'src', 'app', 'imports', 'ngx-chips')
Write-Host "Creating symbolic link from $ngxChipsSource to $ngxChipsDestination"
If ([io.directory]::Exists($ngxChipsDestination)) {
    Write-Host 'Symbolic link to ngx-chips already exists'
}
Else {
    New-Item -ItemType SymbolicLink -Path $ngxChipsDestination -Target $ngxChipsSource
}
