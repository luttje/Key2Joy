param($config = "Debug")

$unixTime = ([DateTimeOffset](Get-Date)).ToUnixTimeSeconds()

Write-Output "Creating new '$config' build @ $unixTime"

dotnet msbuild /p:VersionSuffix=alpha.$unixTime /p:Configuration=$config
