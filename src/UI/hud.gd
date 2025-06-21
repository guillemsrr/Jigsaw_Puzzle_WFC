extends CanvasLayer

@export var game_manager: Node
@export var level_generator: Node

@export var default_columns: int = 5
@export var default_rows: int = 5

@export var generate_button: Button
@export var solve_button: Button
@export var randomize_button: Button
@export var column_spinbox: SpinBox
@export var row_spinbox: SpinBox
@export var option_button: OptionButton
@export var upload_button: Button
@export var file_dialog: FileDialog

var selected_texture: Texture2D

func _ready():
	column_spinbox.value = default_columns
	row_spinbox.value = default_rows
	
	file_dialog.access = FileDialog.ACCESS_FILESYSTEM
	file_dialog.file_mode = FileDialog.FILE_MODE_OPEN_FILE
	file_dialog.filters = PackedStringArray(["*.png ; PNG Images", "*.jpg ; JPEG Images"])

	solve_button.disabled = true
	solve_button.pressed.connect(_on_solveButton_pressed)
	generate_button.pressed.connect(_on_generate_button_pressed)
	randomize_button.pressed.connect(_on_randomizeButton_pressed)
	upload_button.pressed.connect(_on_upload_button_pressed)

func _on_SelectImageButton_pressed():
	file_dialog.popup_centered()

func _on_FileDialog_file_selected(path: String):
	var image = Image.load_from_file(path)
	if image:
		selected_texture = ImageTexture.create_from_image(image)
		print("Texture loaded from: ", path)
	else:
		push_error("Failed to load image.")

func _on_generate_button_pressed():
	if game_manager == null:
		return

	level_generator.GridDimensions = Vector2i(int(column_spinbox.value), int(row_spinbox.value))
	var generationType: int = option_button.get_selected_id()
	game_manager.call("SetOption", generationType)
	game_manager.call("Generate")
	if generationType != 0:
		solve_button.disabled = false

func _on_solveButton_pressed():
	solve_button.disabled = true
	_disable_ui(true)
	game_manager.call("SolvePuzzleAnimated")

	var delay = game_manager.call("GetSolveAnimationTime") as float
	await get_tree().create_timer(delay).timeout
	_disable_ui(false)
	
func _on_randomizeButton_pressed():
		solve_button.disabled = false
		game_manager.call("RandomizePuzzlePieces")
		
func _on_upload_button_pressed():
	file_dialog.popup_centered()

func _disable_ui(disable: bool):
	generate_button.disabled = disable
	randomize_button.disabled = disable
	option_button.disabled = disable
	column_spinbox.editable = not disable
	row_spinbox.editable = not disable
