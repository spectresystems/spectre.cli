if(!$PSScriptRoot){
    $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
}

# Make sure the tool path exists.
$Tools = Join-Path $PSScriptRoot "tools"
if(!(Test-Path $Tools)) {
    New-Item $Tools -ItemType Directory | Out-Null
}

# Make sure that cakeup is present.
$Cakeup = Join-Path $Tools "cakeup-x86_64-latest.exe"
if (!(Test-Path $Cakeup)) {
    Write-Verbose -Message "Downloading cakeup.exe ($CakeupVersion)..."
    try {        
        $wc = (New-Object System.Net.WebClient);
        $wc.DownloadFile("https://cakeup.blob.core.windows.net/windows/cakeup-x86_64-latest.exe", $Cakeup) } catch {
            Throw "Could not download cakeup.exe."
    }
}

# Execute Cakeup
&$Cakeup run "--cake=0.30.0" "--sdk=2.1.400" `
             "--bootstrap" "--execute" `
             "--" "$args"

# Return the exit code from Cakeup.
exit $LASTEXITCODE;