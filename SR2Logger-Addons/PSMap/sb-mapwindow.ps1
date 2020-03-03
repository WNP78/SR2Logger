param($craft,$RSCtrl,$ScriptDir)
    
Add-Type -AssemblyName System.Drawing,System.Windows.Forms

$RGBBackground = [System.Drawing.Color]::FromArgb(39,44,46);
$RGBText = [System.Drawing.Color]::FromArgb(10,172,222);
    
[System.Windows.Forms.Application]::EnableVisualStyles()

[boolean]$MouseDown = $false;
$lastLocation = [System.Drawing.Point]::New(0,0);

$Form                            = New-Object system.Windows.Forms.Form
$Form.ClientSize                 = '856,428'
$Form.text                       = ""
$Form.ControlBox                 = $true
$Form.TopMost                    = $false
$Form.ForeColor                  = $RGBText
$Form.BackgroundImage            = [System.Drawing.Image]::Fromfile("$ScriptDir\Equirectangular.png")
$Form.BackgroundImageLayout      = [System.Windows.Forms.PictureBoxSizeMode]::zoom
$Form.MaximizeBox                = $false
$Form.MinimizeBox                = $false
#$Form.Icon                       = [System.Drawing.Icon]::Fromfile("$ScriptDir\PSMap.ico")
$objIcon = New-Object system.drawing.icon ("$ScriptDir\PSMap.ico")
$Form.Icon = $objIcon


$ArrowGif = [System.Drawing.Bitmap]::FromFile("$ScriptDir\arrow.gif")
$pbArrow                         = New-Object system.Windows.Forms.PictureBox
$pbArrow.width                   = 17
$pbArrow.height                  = 17
$pbArrow.location                = New-Object System.Drawing.Point(100,100)
$pbArrow.image                   = $ArrowGif
$pbArrow.SizeMode                = [System.Windows.Forms.PictureBoxSizeMode]::zoom
$pbArrow.BackColor               = [System.Drawing.Color]::Transparent
$pbArrow.Enabled                 = $false

Function FixResize ($sender,$event) { 
    If ($sender.Width % 2) { $sender.Width = $sender.Width - 1 }
    $sender.Height = ($sender.Width / 2) + 31
    $sender.Update()
}
    
$Form.Add_ResizeEnd({ 
    param($sender,$e)
    FixResize $this $_ 
    $pbArrow.BringToFront()
    $RSCtrl.'Runspaces'.'MapWindow'."OutEvent" = "move $($sender.Location.X) $($sender.Location.Y) $($sender.width) $($sender.height)"
})
$Form.Add_Move({
    param($sender,$e)
    $pbArrow.BringToFront()
    $RSCtrl.'Runspaces'.'MapWindow'."OutEvent" = "move $($sender.Location.X) $($sender.Location.Y) $($sender.width) $($sender.height)"
})
$Form.Add_Closing({
    $RSCtrl.'Runspaces'.'MapWindow'."OutEvent" = "close"
})



$Timer1 = New-Object System.Windows.Forms.Timer
$Timer1.Interval = 300
$Timer1.Enabled = $true
$Timer1SB = { 
    Param($maincraft,$pb,$mainform)

    Function setArrowDir {
        param([System.Drawing.Bitmap]$gif,[int]$degree)
        $Dimension = [System.Drawing.Imaging.FrameDimension]::new($gif.FrameDimensionsList[0])
        $gif.SelectActiveFrame($Dimension, $degree)
    }

    $latitude = $maincraft.'NavPosition'[0]
    $longitute = $maincraft.'NavPosition'[1]

    $newlong = [Math]::Round((($Form.Width-2)/2) + ($longitute * (($Form.Width-2) / 360)))

    if ($latitute -lt 0) { 
        $newlat = [Math]::Round(((($Form.Width-2)/2)/2) - ($latitude * ((($Form.Width-2)/2) / 180)))
    } elseif ($longitute -gt 0) {
        $newlat = [Math]::Round(((($Form.Width-2)/2)/2) + ($latitude * ((($Form.Width-2)/2) / 180)))
    } else { $newlat = 0 }

    $latOffset = 0 # orig 23

    $pb.location = New-Object System.Drawing.Point(($newlong - 8),(($newlat - 8) + $latOffset))
    setArrowDir -gif $ArrowGif -degree ([Math]::Round($craft.'NavHeading',0))
    $pb.update()
    $pb.refresh()
    If ($RSCtrl.'Runspaces'.'MapWindow'.'InCmd' -eq "close") {
        $mainform.Close()
    }
}
    
$Timer1.add_tick({Invoke-Command $Timer1SB -ArgumentList $craft,$pbArrow,$Form})
$Timer1.Start()
$Form.controls.AddRange(@($pbArrow))
$Form.ShowDialog() | Out-Null
$pbArrow.SendToFront()