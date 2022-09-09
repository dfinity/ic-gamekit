tool
extends Control

const settings_path = "res://addons/ic-gamekit/ic-settings.json"

signal ic_conector_enabled(toggled)


# Called when the node enters the scene tree for the first time.
func _ready():
	init_settings()


func init_settings():
	var settings_data = ICSettingsUtilities.load_settings(settings_path)
	$HBoxContainer/RightColumn/CanisterNameInput.text = settings_data["CanisterName"]
	$HBoxContainer/RightColumn/EnableCheckBox.pressed = settings_data["ICConnectorEnabled"]
	
	print("IC Settings initialized.")


func _on_SaveSettingsBtn_pressed():
	save_settings();


func save_settings():
	var node = get_node("LeftColumn")
	print(node)
	
	var settings_data = {
		"CanisterName" : $HBoxContainer/RightColumn/CanisterNameInput.text,
		"ICConnectorEnabled" : $HBoxContainer/RightColumn/EnableCheckBox.pressed
	}
	ICSettingsUtilities.save_settings(settings_path, settings_data)
	
	print("IC Settings saved.")


func _on_ICConnector_toggled(button_pressed):
	emit_signal("ic_conector_enabled", button_pressed)
