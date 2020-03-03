Requirements:

    SR2 NOT running 
    SR2Logger mod installed and enabled


Instructions:

    Before SR2 is running, launch the script:   SR2Logger-PSMap-v0.1.ps1
        How to launch: 
            Open a new powershell console
            Navigate to the directory which contains the script files:
                cd <fullpath>
            Execute the script:
                .\SR2Logger-PSMap-v0.1.ps1
    Launch SR2
    Load whatever craft you would like
    Add the gizmo "SR2Logger-PSMap" somewhere on your craft
    Launch craft
    Activate AG3 in order to begin sending data to the map for live tracking. (you can change this to whatever AG on the SR2Logger-PSMap gizmo)

Notes:

    I am not sure of the behaviour in time accelerated situations, so please DISABLE the action group before using fast forward or other time acceleration modes (you may experience performance issuess otherwise)
    
    Files included for convenience:
        SR2Logger_PSMap.xml:
            Example flight program to load into the Orange Black Box. (this is the default program used for PSMap)
        Prefab Gizmo (cedb62a6-ae49-43e2-90f5-6a5220b1d9bf.xml)
            This is the gizmo named SR2Logger-PSMap
    *Both of these files are installed/copied by the main script. 


    
