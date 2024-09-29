extends Resource
class_name Level

enum Difficulty {Hard, Medium, Easy}

@export var levelDifficulty = Difficulty.Easy
@export var playerHealth = 100
@export var playerMoney = 100
@export var currentRoundNum = 0
@export var mapScene: PackedScene
var towers: Array[Node2D]
