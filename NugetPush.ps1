$srcRoot = '.\src'                     # relative to script directory
$versionFile = 'SharedAssemblyInfo.cs' # relative to $srcRoot
$outputPath = "$home\Dropbox\Packages"
$scriptRoot = "$home\Dropbox\Scripts"

$version = . "$scriptRoot\Get-Version.ps1" (Join-Path $srcRoot $versionFile -Resolve)

. "$scriptRoot\Push-Project.ps1" Simple.Data.PostgreSql $srcRoot $version $outputPath
