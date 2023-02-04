@tool
extends Node3D

var surface_material
var current_tintzones = []
var chosen_color_array = []
var active = false
const TINT_PARAMS = [
	"bright_scene_add_tint",
	"bright_scene_mult_tint",
	"dark_scene_add_tint",
	"dark_scene_mult_tint",
	"specular_scene_add_tint",
	"specular_scene_mult_tint",
	"rim_scene_add_tint",
	"rim_scene_mult_tint",
	"anisotropy_scene_add_tint",
	"anisotropy_scene_mult_tint",
]
const DEFAULT_COLORS = [
	Color("#000000"), Color("#FFFFFF"),
	Color("#000000"), Color("#FFFFFF"),
	Color("#000000"), Color("#FFFFFF"),
	Color("#000000"), Color("#FFFFFF"),
	Color("#000000"), Color("#FFFFFF"),
]

func _ready():
	if !get_parent().is_class("GeometryInstance3D"):
		print("Object Needs to be GeometryInstance3D with material with AnimeDrawingHQ or AnimeDrawingLQ shader depending on which Controller this is")
		if !Engine.is_editor_hint():
			queue_free()
		else:
			set_process(false)
	else:
		surface_material = get_parent().get_surface_override_material(0)
		update_tintzone()
		active = true

func _process(_delta):
	var character_light_vector = vector3_from_angle_yx(rotation.y, rotation.x)
	if active:
		surface_material.set_shader_parameter(
		"character_light", character_light_vector)

func vector3_from_angle_yx(y, x):
	var neg_cosx = -cos(x)
	return Vector3(neg_cosx * sin(y), sin(x), neg_cosx * cos(y))

func tintzone_enter(area):
	if area.is_in_group("Tint Zone"):
		current_tintzones.append(area)
		update_tintzone()

func tintzone_exit(area):
	if area.is_in_group("Tint Zone"):
		current_tintzones.erase(area)
		update_tintzone()

func update_tintzone():
	if get_parent().is_class("MeshInstance3D"):
		var highest_priority = -1
		var chosen_tintzone
		if current_tintzones.size() == 0:
			chosen_color_array = DEFAULT_COLORS
		else:
			for i in current_tintzones.size():
				if current_tintzones[i].color_priority > highest_priority:
					highest_priority = current_tintzones[i].color_priority
					chosen_tintzone = current_tintzones[i]
			chosen_color_array = [
				chosen_tintzone.bright_add_color,
				chosen_tintzone.bright_mult_color,
				chosen_tintzone.dark_add_color,
				chosen_tintzone.dark_mult_color,
				chosen_tintzone.specular_add_color,
				chosen_tintzone.specular_mult_color,
				chosen_tintzone.rim_add_color,
				chosen_tintzone.rim_mult_color,
				chosen_tintzone.anisotropy_add_color,
				chosen_tintzone.anisotropy_mult_color
			]
		assign_color()

func assign_color():
	for i in chosen_color_array.size():
		surface_material.set_shader_parameter(
		TINT_PARAMS[i],  chosen_color_array[i])
