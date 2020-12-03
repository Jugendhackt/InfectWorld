extends Button

export(String) var open_url
# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	connect("pressed", self, "_on_pressed")
	pass # Replace with function body.

func _on_pressed():
	OS.shell_open(open_url)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
