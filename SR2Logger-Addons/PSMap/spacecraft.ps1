Class Spacecraft 
{
    [hashtable]$state 
    [hashtable]$history
    [array]$listeners

    Spacecraft(){
        $this.state = @{
            ActivationGroup1 = 0
            ActivationGroup2 = 0
            ActivationGroup3 = 0
            ActivationGroup4 = 0
            ActivationGroup5 = 0
            ActivationGroup6 = 0
            ActivationGroup7 = 0
            ActivationGroup8 = 0
            ActivationGroup9 = 0
            ActivationGroup10 = 0
            AltitudeAGL = 0
            AltitudeASL = 0
            AtmosphereAirDensity = 0
            AtmosphereAirPressure = 0
            AtmosphereSpeedOfSound = 0
            AtmosphereTemperature = 0
            CurrentPlanetAtmosphereHeight = 0
            CurrentPlanetChildPlanetsCount = 0
            CurrentPlanetCraftsCount = 0
            CurrentPlanetMass = 0
            CurrentPlanetName = 0
            CurrentPlanetParent = 0
            CurrentPlanetRadius = 0
            CurrentPlanetSolarPosition = 0
            FuelAllStages = 0
            FuelBattery = 0
            FuelMono = 0
            FuelStage = 0
            InputBrake = 0
            InputPitch = 0
            InputRoll = 0
            InputSlider1 = 0
            InputSlider2 = 0
            InputThrottle = 0
            InputTranslateForward = 0
            InputTranslateRight = 0
            InputTranslateUp = 0
            InputYaw = 0
            MiscGrounded = 0
            MiscSolarRadiation = 0
            NameCraft = 0
            NamePlanet = 0
            NameTargetName = 0
            NameTargetPlanet = 0
            NavAngleOfAttack = 0
            NavBankAngle = 0
            NavCraftPitchAxis = 0
            NavCraftRollAxis = 0
            NavCraftYawAxis = 0
            NavEast = 0
            NavHeading = 0
            NavNorth = 0
            NavPitch = 0
            NavPosition = 0
            NavSideSlip = 0
            NavTargetPosition = 0
            OrbitApoapsis = 0
            OrbitEccentricity = 0
            OrbitInclination = 0
            OrbitPeriapsis = 0
            OrbitPeriod = 0
            OrbitTimeToApoapsis = 0
            OrbitTimeToPeriapsis = 0
            PerformanceCurrentISP = 0
            PerformanceEngineThrust = 0
            PerformanceMass = 0
            PerformanceMaxEngineThrust = 0
            PerformanceStageBurnTime = 0
            PerformanceStageDeltaV = 0
            PerformanceTWR = 0
            TimeFrameDeltaTime = 0
            TimeTimeSinceLaunch = 0
            TimeTotalTime = 0
            VelocityAcceleration = 0
            VelocityAngular = 0
            VelocityGravity = 0
            VelocityLateral = 0
            VelocityMachNumber = 0
            VelocityOrbit = 0
            VelocitySurface = 0
            VelocityTarget = 0
            VelocityVertical = 0
        }
        $this.history = @{
            ActivationGroup1 = @()
            ActivationGroup2 = @()
            ActivationGroup3 = @()
            ActivationGroup4 = @()
            ActivationGroup5 = @()
            ActivationGroup6 = @()
            ActivationGroup7 = @()
            ActivationGroup8 = @()
            ActivationGroup9 = @()
            ActivationGroup10 = @()
            AltitudeAGL = @()
            AltitudeASL = @()
            AtmosphereAirDensity = @()
            AtmosphereAirPressure = @()
            AtmosphereSpeedOfSound = @()
            AtmosphereTemperature = @()
            CurrentPlanetAtmosphereHeight = @()
            CurrentPlanetChildPlanetsCount = @()
            CurrentPlanetCraftsCount = @()
            CurrentPlanetMass = @()
            CurrentPlanetName = @()
            CurrentPlanetParent = @()
            CurrentPlanetRadius = @()
            CurrentPlanetSolarPosition = @()
            FuelAllStages = @()
            FuelBattery = @()
            FuelMono = @()
            FuelStage = @()
            InputBrake = @()
            InputPitch = @()
            InputRoll = @()
            InputSlider1 = @()
            InputSlider2 = @()
            InputThrottle = @()
            InputTranslateForward = @()
            InputTranslateRight = @()
            InputTranslateUp = @()
            InputYaw = @()
            MiscGrounded = @()
            MiscSolarRadiation = @()
            NameCraft = @()
            NamePlanet = @()
            NameTargetName = @()
            NameTargetPlanet = @()
            NavAngleOfAttack = @()
            NavBankAngle = @()
            NavCraftPitchAxis = @()
            NavCraftRollAxis = @()
            NavCraftYawAxis = @()
            NavEast = @()
            NavHeading = @()
            NavNorth = @()
            NavPitch = @()
            NavPosition = @()
            NavSideSlip = @()
            NavTargetPosition = @()
            OrbitApoapsis = @()
            OrbitEccentricity = @()
            OrbitInclination = @()
            OrbitPeriapsis = @()
            OrbitPeriod = @()
            OrbitTimeToApoapsis = @()
            OrbitTimeToPeriapsis = @()
            PerformanceCurrentISP = @()
            PerformanceEngineThrust = @()
            PerformanceMass = @()
            PerformanceMaxEngineThrust = @()
            PerformanceStageBurnTime = @()
            PerformanceStageDeltaV = @()
            PerformanceTWR = @()
            TimeFrameDeltaTime = @()
            TimeTimeSinceLaunch = @()
            TimeTotalTime = @()
            VelocityAcceleration = @()
            VelocityAngular = @()
            VelocityGravity = @()
            VelocityLateral = @()
            VelocityMachNumber = @()
            VelocityOrbit = @()
            VelocitySurface = @()
            VelocityTarget = @()
            VelocityVertical = @()
        }

        #TODO: "setInterval" - run updateState and generateTelemetry methods on an interval
        ### this is a timed update/grab from synchronized hashtable
        #### TOREVIEW as this is likely already done with the sr2logger ps script
    }

    updateState () {
        #TODO: runspace data grab here.
    }

    generateTelemetry () {
        #get $timestamp in unix time
        $utctimestamp = [int][double]::Parse((Get-Date -UFormat %s))
        #Must substitute for game time value in future
        ForEach ($key in $this.state.Keys) {
            [hashtable]$tmphash = @{
                timestamp = $utctimestamp
                value = $this.state.$key.value
                id = $key
            }
            $this.notify($tmphash)
            $this.history.$key = $this.history.$key + $tmphash
        }
    }

    notify ($point) {
        ForEach ($listener in $this.listeners) { 
            #$listener($point)
            #need to explore other classes further
        }
    }

    listen ($listener) {
        $this.listeners = $this.listeners + $listener
        #TODO: some weird return
    }

}
$spacecrafttemp = [Spacecraft]::new()
$spacecrafttemp.state = [hashtable]::Synchronized($spacecrafttemp.state)
$spacecrafttemp.history = [hashtable]::Synchronized($spacecrafttemp.history)

return $spacecrafttemp