extends Node

@export var currentLevel: Level
@export var currentMap: Map

func _ready() -> void:
	pass

func _process(delta: float) -> void:
	if Input.is_action_just_pressed("load_map"):
		loadMap()

# Loads the map stored in currentLevel
func loadMap() -> void:
	currentMap = currentLevel.mapScene.instantiate()
	currentMap.show()

func addTower(tower: Node2D) -> void:
	currentLevel.towers.append(tower)

func removeTower(tower: Node2D) -> void:
	currentLevel.towers.erase(tower)
	tower.queue_free()
