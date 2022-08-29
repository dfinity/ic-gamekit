tool
extends Control

const settings_path = "res://addons/ic-gamekit/ic-settings.json"
const default_canister_name = "godot_html5_assets"

# Called when the node enters the scene tree for the first time.
func _ready():
	init_settings()

func init_settings():
	var settings_data = load_settings()
	$HBoxContainer/RightColumn/CanisterNameInput.text = settings_data["CanisterName"]
	$HBoxContainer/RightColumn/EnableCheckBox.pressed = settings_data["ICConnectorEnabled"]
	
	print("IC Settings initialized.")

func load_settings():
	var settings_data = { }
	
	# Load the settings from the json file if it exists.
	var file = File.new()
	if file.file_exists(settings_path):
		file.open(settings_path, file.READ)
		
		var text = file.get_as_text();
		settings_data = parse_json(text)
		
		file.close()
	
	# Make sure the necessary values exist in the dictionary.
	if not settings_data.has("CanisterName") :
		settings_data["CanisterName"] = default_canister_name
	
	if not settings_data.has("ICConnectorEnabled") :
		settings_data["ICConnectorEnabled"] = false
		
	return settings_data

func _on_SaveSettingsBtn_pressed():
	save_settings();

func save_settings():
	# Prepare the settings data.
	var settings_data = {
		"CanisterName" : $HBoxContainer/RightColumn/CanisterNameInput.text,
		"ICConnectorEnabled" : $HBoxContainer/RightColumn/EnableCheckBox.pressed
	}
	
	# Save the settings data as json file.
	var file = File.new();
	file.open(settings_path, File.WRITE)
	var json_string = JSON.print(settings_data, "  ");
	file.store_string(json_string)
	file.close()
	
	print("IC Settings saved.")
