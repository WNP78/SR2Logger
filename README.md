# SR2Logger
SR2Logger is a mod for SimpleRockets 2 that transmits simulation variables via UDP for storage to files and/or display in real time. S2Logger provides an example data receiver, `test.py`; however, the user is expected to develop their own listener application to write the log to files or display plots of the data.

## Prerequisites

 1. Set up the SimpleRockets 2 mod project: https://www.simplerockets.com/Forums/View/31506
 2. Python (optional)
 
## Procedure

 1. Enable the SR2Logger mod in SimpleRockets 2.
 2. Start the receiver/listener, e.g. `python test.py`.
 3. In Flight Mode, open the debugger console.
 4. Type `SetSampleRate 2` to start transmitting the UDP packets at 2 samples per second.
 
 
