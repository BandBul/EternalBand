# $version = [System.Reflection.Assembly]::GetExecutingAssembly().GetName().Version
$projectDirectory = Split-Path -Path $PSScriptRoot -Parent
$assemblyPath = Join-Path -Path $projectDirectory -ChildPath "bin\Debug\net7.0\EternalBAND.dll"
$assembly = [System.Reflection.Assembly]::LoadFrom($assemblyPath)
$version = $assembly.GetName().Version
$newVersion = [System.Version]::new($version.Major, $version.Minor, ($version.Build + 1), 0)
Write-Host "Updating version to $newVersion"
(Get-Content ..\Properties\AssemblyInfo.cs) -replace '(\d+\.\d+\.\d+\.\d+)', "$newVersion" | Set-Content -Path ..\Properties\AssemblyInfo.cs