___

# SR2Logger-PSMap
![Scale](https://i.ibb.co/ZKNgkXY/SR2-Logger-PSMap.png)
Hi All,

With the help of plenty of community members (WNP, pedro, nathan, cofi, mario, nota, mod), I've managed to leverage the output of SR2Logger to create an external real time map. 
___

## Basic features: 
* **Resizable** window which scales everything accordingly (*automaintains a 2:1 aspect ratio for mapping*) 
* Shows 'realtime' position of craft 
* Shows 'realtime' heading of craft
* Completely written in **powershell** (*requires no additional software on windows*)
___

## Requires
* SR2Logger mod installed and enabled
___

## Download / Links
[github repo (currently a fork a few commits ahead of WNPs repo)](https://github.com/nethereal/SR2Logger)

[SR2Logger Mod Download (SR2 Beta ONLY)](https://drive.google.com/file/d/1WReOzMlEweiYChXjeBBmXfnQtqpOKhos/view?usp=sharing)

[SR2 Forum Post for SR2Logger-PSMap](https://www.simplerockets.com/Forums/View/56931)

[Get SimpleRockets 2 on Steam!](https://store.steampowered.com/widget/870200/)
___

## Installation notes
* Make sure you are running the ***Beta*** branch of SimpleRockets 2 in steam
* Download, install, and enable the SR2Logger mod in SR2 (2nd link), then exit SR2. 
* **Close** SR2
* Download and extract the zip from the repo link below. the PSMap files are located in: 
* **Before SR2 is running**, launch the script:   
  - SR2Logger/SR2Logger-Addons/PSMap/SR2Logger-PSMap-v0.1.ps1
-  How to launch: 
1. Open a new **powershell** console (Windows Key + R, type: powershell, hit Enter)
2.  Navigate to the directory which contains the script files:
        - cd <fullpath>
        - example:
          - cd C:\gitdirectory\SR2Logger\SR2Logger-Addons\PSMap
3. Execute the script:
          - .\SR2Logger-PSMap-v0.1.ps1
4. Now Launch SR2
5. Load whatever craft you would like
6. Add the gizmo "SR2Logger-PSMap" somewhere on your craft
7. Launch craft
8. To enable map connection: Activate AG3 
        - (you can change this to whatever AG on the SR2Logger-PSMap gizmo)

In the future, you can launch the script whenever you like, but the process above ensures that the prefab'd gizmo and SR2Logger_PSMap flight program assets are available when you first try PSMap.
___

## Additional notes and details
It does feature some advanced powershell(and programming in general) concepts such as classes and runspaces, but should be very familiar to anyone comfortable with C#, as it leverages a lot of the same structures, but simplified (imagine c# had a baby with python) 

I've uploaded a copy of the mod I compiled on my own Windows 10 machine. The mod is currently staged in the website mods list, but not currently available, so only making this available through the post to enable people to try using PSMap.
___

## Possible future features
* orbit display
* flightpath display/history
* day/night positions
* network functionality (see where your friends are flying!)
* Aperture Radar parts so that you have to scan planetary surface to use the map: giving a purpose of launching sats [AnotherFirefox]
