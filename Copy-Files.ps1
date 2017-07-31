$solutionDir = $args[0]
$projectDir = $args[1]

# $solutionDir = 'C:\Users\Michael\Source\Repos\VeganFacts'
# $projectDir = 'C:\Users\Michael\Source\Repos\VeganFacts\src\VeganFacts'

Write-Output "Started copying appsetting files from solution configuration for $projectDir"

$solutionConfigDir = [io.path]::combine($solutionDir, 'config')
$projectConfigDir = $projectDir

$fileArgs = "$solutionConfigDir $projectConfigDir appsettings.*json"
$optionArgs = '/is /xo'
$silentOptionArgs = '/NFL /NDL /NJH /NJS /nc /ns /np'
$args = "$fileArgs $optionArgs $silentOptionArgs"

$result = Start-Process -NoNewWindow -PassThru -Wait robocopy -args $args

Write-Output "Finished copying appsetting files from solution configuration for $projectDir"