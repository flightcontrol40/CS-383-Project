extends Node2D
class_name Map

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	$TowerZones.monitoring = false


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

# Used to check if a tower can be placed in a certain location
# @param tower The tower that wants to be placed
# @return true if the tower can be placed, false otherwise
func validTowerLocation(tower: Node2D) -> bool:
	# Setup the tower zone to check if a tower can be placed
	var towerZone = $TowerZones
	towerZone.monitoring = true

	# Check if tower overlaps the no zone (will need to fix this once Ankit has made his towers)
	var canPlace_f: bool = false
	if not towerZone.overlaps_body(tower):
		canPlace_f = true

	towerZone.monitorying = false
	return canPlace_f
