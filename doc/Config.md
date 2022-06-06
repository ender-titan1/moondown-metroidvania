# Moondown global configuration file 
The Moondown global config file is used to edit 
game values for balancing and playtesting.

## globalConfig JSON
To use the config feature you have to create a new
file in the `C:\\Users\USER\AppData\LocalLow\Moondown Project\Moondown`
directory called `globalConfig.json`. Now the game should read from this file
when started.

## JSON Schema
```
invincible: bool,
playerDashSpeed: float,
playerJumpVelocity: float,
playerWallJumpVelocity: float,
playerPhysics: {
  mass: float,
  gravity: float,
  angularDrag: float,
  linearDrag: float
},
bulletPhysics: {
  mass: float,
  gravity: float,
  angularDrag: float,
  linearDrag: float
}
```
