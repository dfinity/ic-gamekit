tool
extends EditorPlugin

var ic_settings

func _enter_tree():
	ic_settings = preload("res://addons/ic-gamekit/ic-settings.tscn").instance()
	add_control_to_container(EditorPlugin.CONTAINER_PROJECT_SETTING_TAB_RIGHT, ic_settings)

func _exit_tree():
	remove_control_from_container(EditorPlugin.CONTAINER_PROJECT_SETTING_TAB_RIGHT, ic_settings)
