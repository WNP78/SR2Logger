Add-Type -AssemblyName System.Drawing,System.Windows.Forms

If ($runspaces) {
    while ($runspaces.Status.IsCompleted -notcontains $true) {}
    foreach ($runspace in $runspaces ) {
        $results += $runspace.Pipe.EndInvoke($runspace.Status)
        $runspace.Pipe.Dispose()
    }
    $pool.Close() 
    $pool.Dispose()
}

$RunTimeInSeconds = 5

$ScriptDir = $PSScriptRoot
Set-Location $ScriptDir

If (!(Test-Path "$($env:appdata.Replace("Roaming","LocalLow"))\Jundroo\SimpleRockets 2\UserData\FlightPrograms\SR2Logger_PSMap.xml")) {
    Copy-Item "$ScriptDir\SR2Logger_PSMap.xml" "$($env:appdata.Replace("Roaming","LocalLow"))\Jundroo\SimpleRockets 2\UserData\FlightPrograms\SR2Logger_PSMap.xml" -Force:$true -Confirm:$false
}
If (!(Test-Path "$($env:appdata.Replace("Roaming","LocalLow"))\Jundroo\SimpleRockets 2\UserData\SubAssemblies\cedb62a6-ae49-43e2-90f5-6a5220b1d9bf.xml")) {
    Copy-Item "$ScriptDir\cedb62a6-ae49-43e2-90f5-6a5220b1d9bf.xml" "$($env:appdata.Replace("Roaming","LocalLow"))\Jundroo\SimpleRockets 2\UserData\SubAssemblies\cedb62a6-ae49-43e2-90f5-6a5220b1d9bf.xml" -Force:$true -Confirm:$false
}

$craft = $null
$craft = .\spacecraft.ps1

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$sessionstate = [system.management.automation.runspaces.initialsessionstate]::CreateDefault()
$pool = [RunspaceFactory]::CreateRunspacePool(1, [int]20, $sessionstate, $Host)
$pool.ApartmentState = "MTA"
$pool.Open()
$runspaces = $results = @()

$RSControl = @{
    Runspaces = @{
        MapReceiver = @{
            InstanceID = $ReceiverRSInstanceID.Guid
            InCmd = ""    
            OutEvent = ""
            ctrlOutput = ""
        }
        MapWindow = @{
            InstanceID = $MapWindowRSInstanceID.Guid
            InCmd = ""
            OutEvent = ""
            CtrlOutput = ""
        }
    }
}
$RSControl = [hashtable]::Synchronized($RSControl)

$ReceiverSB = get-command "$ScriptDir\sb-mapreceiver.ps1" | select -ExpandProperty ScriptBlock 

$MapWindowSB = get-command "$ScriptDir\sb-mapwindow.ps1" | select -ExpandProperty ScriptBlock 

$ReceiverSBrunspace = [PowerShell]::Create().AddScript($ReceiverSB).AddArgument($craft.state).AddArgument($RSControl).AddArgument($RunTimeInSeconds)
$ReceiverSBrunspace.RunspacePool = $pool
$ReceiverRSInstanceID = $ReceiverSBrunspace.InstanceId
$runspaces += [PSCustomObject]@{ Pipe = $ReceiverSBrunspace; Status = $ReceiverSBrunspace.BeginInvoke() }

$MapWindowSBrunspace = [PowerShell]::Create().AddScript($MapWindowSB).AddArgument($craft.state).AddArgument($RSControl).AddArgument($ScriptDir)
$MapWindowSBrunspace.RunspacePool = $pool
$MapWindowRSInstanceID = $MapWindowSBrunspace.InstanceId
$runspaces += [PSCustomObject]@{ Pipe = $MapWindowSBrunspace; Status = $MapWindowSBrunspace.BeginInvoke() }

$RGBBackground = [System.Drawing.Color]::FromArgb(39,44,46);
$RGBText = [System.Drawing.Color]::FromArgb(10,172,222);

$fontpath = "$ScriptDir\*.ttf"
$ttffiles = Get-ChildItem $fontpath
$fontCollection = New-Object System.Drawing.Text.PrivateFontCollection
$ttffiles | ForEach-Object { $fontCollection.AddFontFile($_.fullname) }
$FontAnita = $fontCollection.Families[0]

$tmpForm                            = New-Object System.Windows.Forms.Form
$tmpForm.ClientSize                 = '200,40'
$tmpForm.TopMost                    = $true
$tmpForm.BackColor                  = $RGBBackground
$tmpForm.ForeColor                  = $RGBText
$tmpForm.FormBorderStyle            = [System.Windows.Forms.BorderStyle]::None

$lbl = New-Object System.Windows.Forms.Label
$lbl.Location = [System.Drawing.Point]::new(5,5)
$lbl.Text = "SR2Logger - PSMap v0.1"
$lbl.Width = 200
$lbl.Height = 30
$lbl.Font = [System.Drawing.Font]::new($FontAnita, 8, [System.Drawing.FontStyle]::Bold)

$btnX = New-Object System.Windows.Forms.Button
$btnX.Location = [System.Drawing.Point]::new(150,5)
$btnX.BackColor = $RGBBackground
$btnX.ForeColor = $RGBText
$btnX.Text = "X"
$btnX.Width = 30
$btnX.Height = 30 
$btnX.Font = [System.Drawing.Font]::new($FontAnita, 10, [System.Drawing.FontStyle]::Bold)

$btnX.add_Click({
    param($sender,$e)
    $RSControl.'Runspaces'.Keys | % { 
        $RSControl.'Runspaces'."$_".'InCmd' = "close"
    }
    while ( ( $runspaces.Status.IsCompleted | Select -unique ) -ne $true) { Start-Sleep -Milliseconds 100}
    foreach ($runspace in $runspaces ) {
        $results += $runspace.Pipe.EndInvoke($runspace.Status)
        $runspace.Pipe.Dispose()
    }
    $pool.Close() 
    $pool.Dispose()
    $sender.Parent.Close()
})

$tmpForm.add_Move({
    param($sender,$e)
    $btnX.Location = [System.Drawing.Point]::new(($sender.width - (5 + $btnX.Width)),$btnX.Location.Y)
    $sender.BringToFront()
})

$TimerRSCtrl = New-Object System.Windows.Forms.Timer
$TimerRSCtrl.Interval = 100
$TimerRSCtrl.Enabled = $true
$prvOutput = ""
$TimerRSCtrlSB = { 
    param($frm,$rsctrl)
    If ($rsctrl.'Runspaces'.'MapWindow'.'OutEvent' -like "move *") {
        #Write-Host "$($rsctrl.'Runspaces'.'MapWindow'.'OutEvent')"
        $tmpstr = $RSCtrl.'Runspaces'.'MapWindow'.'OutEvent'
        $rawCoordArr = (($tmpstr.Replace("move ","")) -split " ")
        $MapWindowLocation = [System.Drawing.Point]::new($rawCoordArr[0],$rawCoordArr[1])
        $MapWindowWidth = $rawCoordArr[2]
        $MapWindowHeight = $rawCoordArr[3]
        $frmNewWidth = $MapWindowWidth / 3
        $frmXpos = $MapWindowLocation.X + ($MapWindowWidth / 2) - ($frmNewWidth / 2)
        $frmYpos = $MapWindowLocation.y + 31
        $frm.Width = $frmNewWidth
        $frm.Location = [System.Drawing.Point]::New($frmXpos,$frmYpos)
        $frm.Update()
        $rsctrl.'Runspaces'.'MapWindow'.'OutEvent' = ""
    }

    If (($rsctrl.'Runspaces'.'MapWindow'.'CtrlOutput' -ne $prvOutput) -and ($rsctrl.'Runspaces'.'MapWindow'.'CtrlOutput' -ne "")) {
        #Write-Host "[MapWindow:ctrlOutput] $($rsctrl.'Runspaces'.'MapWindow'.'CtrlOutput')"
        $rsctrl.'Runspaces'.'MapWindow'.'CtrlOutput' = ""
    }
    If ($rsctrl.'Runspaces'.'MapWindow'.'OutEvent' -eq "close") {
        #Write-Host "[MapWindow:OutEvent] CLOSE"
        $rsctrl.'Runspaces'.'MapReceiver'.'InCmd' = 'Close'
        $frm.Close()
    }
}
    
$TimerRSCtrl.add_tick({Invoke-Command $TimerRSCtrlSB -ArgumentList $tmpForm,$RSControl})

$tmpForm.Controls.AddRange(@($lbl,$btnX))

$tmpForm.ShowDialog() | Out-Null

$TimerRSCtrl.Start()