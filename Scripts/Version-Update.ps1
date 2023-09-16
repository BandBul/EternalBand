$version = [System.Reflection.Assembly]::GetExecutingAssembly().GetName().Version
$newVersion = [System.Version]($version.Major, $version.Minor, ($version.Build + 1), 0)
Write-Host "Updating version to $newVersion"
(Get-Content -Path 'AssemblyInfo.cs') -replace '(\d+\.\d+\.\d+\.\d+)', "$newVersion" | Set-Content -Path 'AssemblyInfo.cs'