param(
    [string] $config = "Debug",
    [string] $versionId = $null
)

$unixTime = ([DateTimeOffset](Get-Date)).ToUnixTimeSeconds()

if (!$versionId) {
    $versionId = $unixTime
}

$versionId = $versionId.Substring(0, [Math]::Min(10, $versionId.Length))

$versionId = "alpha.$versionId"

Write-Output "Creating new '$config' build @ $unixTime with version identifier: '$versionId'"

dotnet msbuild /p:VersionSuffix=$versionId /p:Configuration=$config

$versionPrefix = [xml](Get-Content Core/Key2Joy.Core/Key2Joy.Core.csproj) | Select-Xml -XPath "/Project/PropertyGroup/VersionPrefix" | Select-Object -ExpandProperty Node | Select-Object -ExpandProperty "#text"

$versionId = "$versionPrefix-$versionId"

Write-Output $versionId
