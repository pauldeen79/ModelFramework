# Set the directory path and file extension
$directoryPath = Get-Location
$fileExtension = "*.cs"

# Set the search phrase
$searchPhrase = "using *"  # Change this to your desired search phrase

# Get all files with the specified extension in the directory
$fileList = Get-ChildItem -Path $directoryPath -Filter $fileExtension -Recurse | Where-Object { $_.FullName -notlike '*\obj\*' -and $_.FullName -notlike '*.generated.cs' }

# Loop through each file and read its contents line by line
foreach ($file in $fileList) {
    $fileContent = Get-Content $file.FullName

    foreach ($line in $fileContent) {
        if ($line -like "$searchPhrase*") {
            Write-Host "$($file.FullName):$line"
            break
        }
    }
}
