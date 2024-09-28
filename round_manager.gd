extends Node2D
class_name TestClass


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var my_csharp_script = load("res://Path/To/MyCSharpNode.cs")
	var my_csharp_node = my_csharp_script.new()
	print(my_csharp_node.myField) # bar
	my_csharp_node.myField = "BAR"
	print(my_csharp_node.myField) # BAR

func shoot_chicken() -> void:
	pass

# THIS IS C# 
myGDScriptNode.Call("print_node_name", this); # my_csharp_node
# myGDScriptNode.Call("print_node_name"); # This line will fail silently and won't error out.

myGDScriptNode.Call("print_n_times", "Hello there!", 2); // Hello there! Hello there!

string[] arr = new string[] { "a", "b", "c" };
myGDScriptNode.Call("print_array", arr); // a, b, c
myGDScriptNode.Call("print_array", new int[] { 1, 2, 3 }); // 1, 2, 3
# Note how the type of each array entry does not matter as long as 