param($craft,$RSCtrl,$TimeToRun)

$LogPort = 2873;

Function Read([byte[]]$data) {
    $stream = [System.IO.MemoryStream]::new($data)
    $reader = [System.IO.BinaryReader]::new($stream)
    [hashtable]$tmphash = @{}
    do {
        [int]$length = $reader.ReadUInt16();
        [byte[]]$name = @($null)*$length;
        $reader.Read($name,0,$length) | Out-Null;
        $propPath = [System.Text.Encoding]::Default.GetString($name) -split "_"
        $propVal = $null
        [int]$type = $reader.ReadByte();
        switch ($type) {
            0       { $propVal = $null }
            1       { $propVal = $reader.ReadDouble() }
            2       { $propVal = $reader.ReadBoolean() }
            3       { $propVal = @($reader.ReadDouble(),$reader.ReadDouble(),$reader.ReadDouble()) }
            default { $propVal = "UnknownType" }
        }
        $tmphash.Add("$($propPath[0])$($propPath[1])",$propVal)
    } while ($stream.Position -lt $stream.Length)
    ForEach ($key in $tmphash.Keys) { $craft."$key" = $tmphash."$key" }
    $reader.Dispose();
    $stream.Dispose();
}

If ($script:cli) { $script:cli.Dispose(); }
[System.Net.Sockets.UdpClient]$script:cli = [System.Net.Sockets.UdpClient]::new($LogPort);

If ($ep) { $ep = $null; } 
$ep = [System.Net.IPEndPoint]::new([System.Net.IPAddress]::Any,$LogPort);

$startTime = Get-Date
$endTime = $startTime.AddSeconds($TimeToRun)

do { 
    If ($script:cli.Available -gt 0) { 
        Read($script:cli.Receive([ref]$ep)) 
    } 
} while ($RSCtrl.'Runspaces'.'MapReceiver'.'InCmd' -ne "close")
#} while ($true -and ($RSCtrl.'Runspaces'.'MapReceiver'.'InCmd' -ne "close" -and ((Get-Date) -lt $endTime)))

$script:cli.Dispose(); $ep = $null;