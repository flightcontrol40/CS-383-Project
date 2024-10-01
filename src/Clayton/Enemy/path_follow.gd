extends PathFollow2D

var speed = .1

func _process(delta: float) -> void:
	progress_ratio += delta * speed
	
	if (progress_ratio >= 1):
		queue_free()
	
