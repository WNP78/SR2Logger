import socket, struct

s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
s.bind(("localhost", 2873))

TypeFormats = [
    "",     # null
    "d",    # double (float64)
    "?",    # bool
    "ddd"]  # Vector3d

class Packet:
    def __init__(self, data):
        self.data = data
        self.pos = 0
    def get(self, l): # get l bytes
        self.pos += l
        return self.data[self.pos - l:self.pos]
    def read(self, fmt): #  read all formatted values
        v = self.readmult(fmt)
        if len(v) == 0: return None
        if len(v) == 1: return v[0]
        return v
    def readmult(self, fmt): # read multiple formatted values
        return struct.unpack(fmt, self.get(struct.calcsize(fmt)))
    @property
    def more(self): # is there more data?
        return self.pos < len(self.data)

def readPacket(dat):
    p = Packet(dat)

    while p.more:
        nameLen = p.read("H")
        name = p.get(nameLen).decode()
        
        tp = p.read("B")
        typeCode = TypeFormats[tp]
        val = p.read(typeCode)

        print(name, "=", val)

    print("")

print("Starting")
while 1:
    d, a = s.recvfrom(2048)

    readPacket(d)
