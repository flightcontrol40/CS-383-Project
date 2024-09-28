
## What is needed
All we need is something that looks like a game/the most basic version of our game:
* Single level
* One round
* Enemies that spawn at a regular interval
* A shop to buy towers
* Placeable towers
* Towers that do damage to enemies
* Enemies can hurt the player
* win/lose screen
* Each element can be distinguished from another
	* elements like towers and enemies can be rectangles, but the player needs to be able to distinguish between the two

## Nice to haves:
* A few rounds
* difficulty system
* Enemy spawning interval changes based difficulty system
* Towers that can be upgraded
* Main menu scene
* basic textures


## What TL2 needs from each person (suggestions)
### Austin - Level
* create tower placement system - might need coordination with Ankit (towers)/Sohan (controls)
* Create a level
	* create a map which should include
		* paths for enemies - needs coordination with Clayton (enemies)
		* tower placement zones - might need to coordinate with Ankit (towers)/Sohan (controls)
		* basic textures
* start thinking about level selection system

### Nate - Round
* get enemies to spawn at some rate - might need some coordination with Clayton (enemies)/Austin (start position)
* create win/lose condition and or condition tracking? - might need to coordinate with Sohan (UI)
* start thinking about how to expand to more rounds
* start thinking about difficulty scaling

### Clayton - Enemies
* create a type of enemy which should:
	* be able to deal damager to player
	* be able to pathfind - needs coordination with Austin (implementing enemy path in map?)
	* take damage from towers
	* give player some money when killed

### Sohan - UI/Controls
* create shop interface - might need coordination with Ankit (towers)
* create controls to:
	* place a tower - might need coordination with Ankit (towers)/Austin (tower place zones)
	* select a tower - might need coordination with Ankit (towers)
* create win/lose screen? - might need coordination with Nate (win/lose conition)
* start thinking about the upgrade tower menu

### Ankit - Towers
* create a type of tower which should:
	* be able to damage enemies that come within range - needs coordination with Clayton (Enemies)
	* be placable on map - might need to coordinate with Austin (tower placement zones)/Sohan (controls)
	* start thinking about upgrades
