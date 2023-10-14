param(
    [string] $config = "Debug",
    [string] $versionId = $null
)

$unixTime = ([DateTimeOffset](Get-Date)).ToUnixTimeSeconds()

if (!$versionId) {
    $versionId = $unixTime
}

Write-Output "Creating new '$config' build @ $unixTime with version identifier: '$versionId'"

dotnet msbuild /p:VersionSuffix=alpha.$unixTime /p:Configuration=$config

return $versionId
