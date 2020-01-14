import socket, struct
from os import system, name
from time import sleep

TypeFormats = [
    "f",
    "d",
    "fff",
    "ddd",
    "iii",
    "?",
    "i",
    "ffff",
    "dddd",
    None]

FieldNames = [
       "Velocity",
       "Altitude",
       "AltitudeAgl",
       "CanWarp",
       "Brake",
       "Pitch",
       "Roll",
       "Slider1",
       "Slider2",
       "TargetDirection",
       "TargetHeading",
       "Throttle",
       "TranslateForward",
       "TranslateRight",
       "TranslateUp",
       "TranslationModeEnabled",
       "Yaw",
       "CraftMass",
       "CraftPartCount",
       "CurrentStage",
       "NumStages",
       "AirDensity",
       "AirPressure",
       "AtmosphereHeight",
       "SampleAltitude",
       "SpeedOfSound",
       "Temperature",
       "eulerAngles",
       "forward",
       "localEulerAngles",
       "localPosition",
       "localRotation",
       "localScale",
       "position",
       "right",
       "rotation",
       "up",
       "CurrentEngineThrust",
       "GravityMagnitude",
       "MachNumber",
       "MaxActiveEngineThrust",
       "RemainingBattery",
       "RemainingFuelInStage",
       "RemainingMonopropellant",
       "SolarRadiationDirection",
       "SolarRadiationFrameDirection",
       "SolarRadiationIntensity",
       "SurfaceVelocity",
       "SurfaceVelocityMagnitude",
       "VelocityMagnitude",
       "WeightedThrottleResponse",
       "WeightedThrottleResponseTime",
       "FramePosition",
       "GravityForce",
       "GravityNormal",
       "AirEfficiency",
       "AvailableAir",
       "ReEntryIntensity",
       "Heading",
       "InContactWithPlanet",
       "IsDestroyed",
       "Position",
       "SolarPosition",
       "SolarVelocity",
       "SphereOfInfluence",
       "SurfacePosition",
       "SurfaceRotation"
       ]

s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

s.bind(("127.0.0.1", 2873))

def process(data):
    pos = 0
    st = ""
    while pos < len(data):
        nm, tp = struct.unpack("iB", data[pos:pos + 5])
        pos += 5
        fmt = TypeFormats[tp]
        if fmt == None:
            val = None # null type
        else:
            l = struct.calcsize(fmt)
            val = struct.unpack(fmt, data[pos:pos + l])
            pos += l
            if len(val) == 1: val = val[0]

        st += FieldNames[nm] + ": " + str(val) + "\n"
    print(st)

while 1:
        d, a = s.recvfrom(2048)
        process(d)
