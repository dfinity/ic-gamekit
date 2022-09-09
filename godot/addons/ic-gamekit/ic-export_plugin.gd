tool
extends EditorExportPlugin

var output_path

# Called when the node enters the scene tree for the first time.
func _ready():
	pass

func _export_begin(features, is_debug, path, flags):
	output_path = path
	print("Begin: " + output_path)
	print(features)
	
func _export_end():
	print("End: " + output_path)
	
func _export_file(path, type, features):
	if "addons/ic-gamekit" in path:
		skip()

	print(path)
