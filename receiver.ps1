$LogPort = 2873;

Function Read([byte[]]$data) {
    $stream = [System.IO.MemoryStream]::new($data)
    $reader = [System.IO.BinaryReader]::new($stream)
    $script:outstr = ""
    do {
        [int]$length = $reader.ReadUInt16();
        [byte[]]$name = @($null)*$length;
        $reader.Read($name,0,$length) | Out-Null;
        $script:outstr += "$([System.Text.Encoding]::Default.GetString($name)) = ";
        [int]$type = $reader.ReadByte();
        switch ($type) {
            0       { $script:outstr += "null`n"; }
            1       { $script:outstr += "$($reader.ReadDouble())`n"; }
            2       { $script:outstr += "$($reader.ReadBoolean())`n"; }
            3       { $script:outstr += "($($reader.ReadDouble()),$($reader.ReadDouble()),$($reader.ReadDouble()))`n"; }
            default { $script:outstr += "Unknown type`n"; }
        }
    } while ($stream.Position -lt $stream.Length)
    $reader.Dispose();
    $stream.Dispose();
}

If ($script:cli) { $script:cli.Dispose(); }
[System.Net.Sockets.UdpClient]$script:cli = [System.Net.Sockets.UdpClient]::new($LogPort);

If ($ep) { $ep = $null; } 
$ep = [System.Net.IPEndPoint]::new([System.Net.IPAddress]::Any,$LogPort);

do {
    If ($script:cli.Available -gt 0) { 
        Read($script:cli.Receive([ref]$ep))
        cls
        Write-Output $script:outStr ;
    }
} while ($true)

$script:cli.Dispose(); $ep = $null;