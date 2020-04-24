param(
    [string]
    $name = $(Read-Host "Enter migration name")
)

$name = ((,$name + $args) -join "_") -replace "\s+", "_"

$name

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

write-host "$filePath"