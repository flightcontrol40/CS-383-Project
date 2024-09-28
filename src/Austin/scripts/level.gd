extends Node

enum Difficulty {Hard, Medium, Easy}

@export var level_difficulty = Difficulty.Hard
@export var player_health:int
@export var player_money:int
@export var current_round_num:int
@export var map:PackedScene

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
