@tool
extends Control

const settings_path = "res://addons/ic-gamekit/ic-settings.json"

var utilities_script

# Called when the node enters the scene tree for the first time.
func _ready():
	utilities_script = load("res://addons/ic-gamekit/ic-settings-utilities.gd")
	init_settings()


func init_settings():
	var settings_data = utilities_script.load_settings(settings_path)
	$HBoxContainer/RightColumn/CanisterNameInput.text = settings_data["CanisterName"]
	$HBoxContainer/RightColumn/EnableCheckBox.button_pressed = settings_data["ICConnectorEnabled"]
	
	print("IC Settings initialized.")


func _on_SaveSettingsBtn_pressed():
	save_settings();


func save_settings():
	var settings_data = {
		"CanisterName" : $HBoxContainer/RightColumn/CanisterNameInput.text,
		"ICConnectorEnabled" : $HBoxContainer/RightColumn/EnableCheckBox.button_pressed
	}
	utilities_script.save_settings(settings_path, settings_data)
	
	print("IC Settings saved.")
