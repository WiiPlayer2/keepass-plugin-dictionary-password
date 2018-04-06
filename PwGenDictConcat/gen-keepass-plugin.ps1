$KeePass_Exe = "C:\Program Files (x86)\KeePass Password Safe 2\KeePass.exe"

Push-Location
Set-Location $PSScriptRoot

mkdir plgx_files -ErrorAction SilentlyContinue | Out-Null

$files = Get-ChildItem -Exclude bin,obj,plgx_files

foreach($file in $files)
{
    $target = (Join-Path plgx_files $file.Name)
    if($file.Attributes.HasFlag([System.IO.FileAttributes]::Directory))
    {
        Copy-Item -Recurse $file $target -Force
    }
    else
    {
        Copy-Item $file $target -Force
    } 
}

$plgx_files_path = (Get-Item plgx_files).FullName
. $KeePass_Exe --plgx-create "$plgx_files_path" | Out-Host
Remove-Item -Recurse plgx_files | Out-Null

Remove-Item .\PwGenDictConcat.plgx
Rename-Item .\plgx_files.plgx .\PwGenDictConcat.plgx

Pop-Location