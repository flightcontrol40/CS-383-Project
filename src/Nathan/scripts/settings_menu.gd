extends Control

signal return_to_menu

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
    hide_menu()

func unhide_menu():
    visible = true
    mouse_filter = MouseFilter.MOUSE_FILTER_STOP

func hide_menu():
    visible = false
    mouse_filter = MouseFilter.MOUSE_FILTER_IGNORE


func _on_return_button_pressed():
    return_to_menu.emit()


