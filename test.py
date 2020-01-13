import socket, struct

TypeFormats = [
    "f",
    "d",
    "fff",
    "ddd",
    "iii"]

FieldNames = [
    "Velocity",
    "Altitude"]

s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

s.bind(("127.0.0.1", 2873))

def process(data):
    nm, tp = struct.unpack("ii", data[:8])
    fmt = TypeFormats[tp]
    val = struct.unpack(fmt, data[8:8 + struct.calcsize(fmt)])
    print(FieldNames[nm], ": ", val, sep="")
    
    

while 1:
        d, a = s.recvfrom(256)
        process(d)
