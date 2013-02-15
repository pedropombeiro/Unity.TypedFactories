Param($AssemblyVersion, $AssemblyFileVersion, $AssemblyInformationalVersion) 

$globalAssemblyInfoPath = [System.IO.Path]::GetFullPath(".\GlobalAssemblyInfo.cs")

(gc $globalAssemblyInfoPath) |
  ForEach-Object {
  (($_ -replace "^(.*AssemblyVersion.{1}).*(.{2})$", ('$1"' + $AssemblyVersion + '"$2')) -replace "^(.*AssemblyFileVersion.{1}).*(.{2})$", ('$1"' + $AssemblyFileVersion + '"$2')) -replace "^(.*AssemblyInformationalVersion.{1}).*(.{2})$", ('$1"' + $AssemblyInformationalVersion + '"$2')
  } | sc $globalAssemblyInfoPath