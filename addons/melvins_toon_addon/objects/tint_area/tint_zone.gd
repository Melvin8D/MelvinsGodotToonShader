#Attach Collision shape below.
#Uses layer 32
@tool
extends Area3D

@export var color_priority := 0 #Greater than one # (int,0, 128)
@export var bright_add_color = Color(0.0,0.0,0.0,1.0)
@export var bright_mult_color = Color(1.0,1.0,1.0,1.0)
@export var dark_add_color = Color(0.0,0.0,0.0,1.0)
@export var dark_mult_color = Color(1.0,1.0,1.0,1.0)
@export var specular_add_color = Color(0.0,0.0,0.0,1.0)
@export var specular_mult_color = Color(1.0,1.0,1.0,1.0)
@export var rim_add_color = Color(0.0,0.0,0.0,1.0)
@export var rim_mult_color = Color(1.0,1.0,1.0,1.0)
@export var anisotropy_add_color = Color(0.0,0.0,0.0,1.0)
@export var anisotropy_mult_color = Color(1.0,1.0,1.0,1.0)
@export var visibility_aabb = AABB(Vector3(-1,-1,-1), Vector3(2,2,2))
var editor_use = Engine.is_editor_hint()

func _ready():
	$VisibleOnScreenNotifier3D.aabb = visibility_aabb

func _on_VisibilityNotifier_camera_entered(_camera):
	if !editor_use:
		monitorable = true
		print("See")

func _on_VisibilityNotifier_camera_exited(_camera):
	if !editor_use:
		monitorable = false
