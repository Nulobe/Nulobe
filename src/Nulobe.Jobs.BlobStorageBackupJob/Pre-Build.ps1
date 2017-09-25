$solutionDir = $args[0]
$projectDir = $args[1]
$targetDir = $args[2]

# $solutionDir = 'D:\Source2\Nulobe'
# $projectDir = 'D:\Source2\Nulobe\src\Nulobe.Jobs.BlobStorageBackupJob'
# $targetDir = 'D:\Source2\Nulobe\src\Nulobe.Jobs.BlobStorageBackupJob\bin\x64\Debug'

# Execute Copy-App-Settings.ps1
$copyFilesScriptPath = [io.path]::Combine($solutionDir, 'Copy-App-Settings.ps1')
$copyFilesScriptArgs = "$solutionDir $projectDir"
Invoke-Expression "& `"$copyFilesScriptPath`" $copyFilesScriptArgs"

# Copy DocumentDB Import Tool
Write-Output "Started copying dt-1.7"

$sourceDtDir = [io.path]::Combine($solutionDir, 'exe', 'dt-1.7')
$destDtDir = [io.path]::Combine($targetDir, 'dt-1.7')

$fileArgs = "$sourceDtDir $destDtDir * /E"
$optionArgs = '/is /xo'
$silentOptionArgs = '/NFL /NDL /NJH /NJS /nc /ns /np'
$args = "$fileArgs $optionArgs $silentOptionArgs"

$result = Start-Process -NoNewWindow -PassThru -Wait robocopy -args $args

Write-Output "Finished copying dt-1.7"