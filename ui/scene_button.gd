extends Button

export(String) var scene_to_load
# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	connect("pressed", self, "_on_pressed")
	pass # Replace with function body.

func _on_pressed():
	get_tree().change_scene(scene_to_load)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
