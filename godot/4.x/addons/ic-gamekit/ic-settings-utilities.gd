extends Node

const default_canister_name = "godot_html5_assets"


static func load_settings(settings_path):
	var settings_data = { }
	
	# Load the settings from the json file if it exists.
	var file = File.new()
	if file.file_exists(settings_path):
		file.open(settings_path, file.READ)
		
		var text = file.get_as_text();
		settings_data = JSON.parse_string(text)
		
		file.close()
	
	# Make sure the necessary values exist in the dictionary.
	if not settings_data.has("CanisterName") :
		settings_data["CanisterName"] = default_canister_name
	
	if not settings_data.has("ICConnectorEnabled") :
		settings_data["ICConnectorEnabled"] = false
		
	return settings_data


static func save_settings(settings_path, settings_data):
	# Save the settings data as a json file.
	var file = File.new();
	
	file.open(settings_path, File.WRITE)
	var json_string = JSON.stringify(settings_data, "  ");
	file.store_string(json_string)
	
	file.close()
