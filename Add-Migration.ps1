param(
    [string]
    $name = $(Read-Host "Enter migration name")
)

$name = ((,$name + $args) -join "_") -replace "\s+", "_"
$rootDir = resolve-path .
$migrationsDir = "$rootDir\db\migrations"

$version = gci -path "$migrationsDir" -Filter *.sql | % { $_.Name -replace "^(\d+).*?$","`$1" } | % { [float]$_ } | sort -Descending | select -First 1
$version++
$version = $version.ToString()

$oldName = $name
$name = $name -replace " ", "_"

$newFileName = "$($version)_$name.sql"
$filePath = "$migrationsDir\$newFileName"

"-- $(Get-Date -Format "yyyy-MM-dd HH:mm") : $oldName" | set-content -Path "$filePath"

dotnet ef migrations add $name --project .\src\Harald.WebApi

(dotnet ef migrations script -i --project .\src\Harald.WebApi) -Split [Environment]::NewLine | Select-Object -Skip 2 | Out-File -FilePath $filePath

Write-Host "Created sql script: " + $filePath