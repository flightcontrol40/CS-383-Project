class_name MainMenu
extends Control



@onready var start_button = $VBoxContainer2/Start as Button		#defines start button as button
@onready var quit_button = $VBoxContainer2/Quit as Button		#defines quit button as button
# @onready var start_level = preload("res://Sohan/Level1.tscn") as PackedScene		#defines game as a packed scene used in start_pressed function


func _ready():
	start_button.button_down.connect(on_start_pressed)		#connects start button with start function
	quit_button.button_down.connect(on_quit_pressed)		#connects quit button with quit function
	
	
func on_start_pressed() -> void:
	get_tree().change_scene_to_file("res://src/Austin/scenes/map.tscn")		#starts game
	
func on_quit_pressed() -> void:
	get_tree().quit()		#quits game
