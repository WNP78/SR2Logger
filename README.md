# SR2Logger
SR2Logger is a mod for SimpleRockets 2 that transmits simulation variables via UDP for storage to files and/or display in real time. SR2Logger is represented by a flight data instrumentation part in the game.

![Orion PA-1 Instrumentation System](https://www.nasa.gov/sites/default/files/images/503496main_Orion_PA-1_inst_system_226.jpg)

_Orion PA-1 Instrumentation System_

S2Logger provides an example data receiver, `recevier.py`; however, the user is expected to develop their own listener application to write the log to files or display plots of the data.

## Prerequisites

 1. Set up the SimpleRockets 2 mod project: https://www.simplerockets.com/Forums/View/31506
 2. Python (optional)
 
## Procedure

 1. Enable the SR2Logger mod in SimpleRockets 2.
 2. Add an "Orange Black Box" part from the gizmos category to your craft.
 3. Edit the part's program.
     1. The program should set a variable "LogFrequency" to the number of samples per second to be send
     2. The program then can set any number of variables beginning with "log_". These will be logged.
 4. In the part's settings panel you can also set the host and port to send packets to. This is localhost:2873 by default.
 5. Start either the provided `reciever.py` script or your own derived receiver script.
 6. Launch the craft, click the Orange Black Box, and click the activate button in the context menu to start logging.
 
 
