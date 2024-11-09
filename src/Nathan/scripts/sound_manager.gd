extends Node2D
class_name soundManager

# Dictionary to hold all loaded sounds
@export var sounds: Dictionary = {}
@export var music: Dictionary = {}
# Nodes for managing background music and sound effects
@onready var music_player: AudioStreamPlayer = $MusicPlayer
@onready var sfx_player: AudioStreamPlayer2D = $SFXPlayer

# Volume controls
@export var music_volume: float = 0.5
@export var sfx_volume: float = 0.5

var current_song: String = ""

func _ready():
    # Add the players to the scene and set initial volumes
    _update_volumes()
    # Play the default theme
    play_music('mainTheme')


func _update_volumes():
    music_player.volume_db = linear_to_db(music_volume)
    sfx_player.volume_db = linear_to_db(sfx_volume)

func _process(_delta: float) -> void:
    if music_player.finished:
        var next_song: String = ''
        var next = false
        for _name in music:
            if next == true:
                next_song = _name
                break
            if _name == current_song:
                next = true
        play_music(next_song);

# Load sound files dynamically
func load_sound(_name: String, path: String):
    var audio_stream = load(path)
    if audio_stream is AudioStream:
        sounds[_name] = audio_stream
    else:
        push_error("Failed to load sound at path: " + path)

# Load music files dynamically
func load_music(_name: String, path: String):
    var audio_stream = load(path)
    if audio_stream is AudioStream:
        music[_name] = audio_stream
    else:
        push_error("Failed to load music at path: " + path)


# Play background music
func play_music(_name: String, loop: bool = true):
    if sounds.has(_name):
        music_player.stream = sounds[_name]
        music_player.play()
        music_player.loop = loop
        current_song = _name
    else:
        push_error("Music not found: " + _name)

# Stop background music
func stop_music():
    music_player.stop()

# Play sound effect
func play_sfx(_name: String):
    if sounds.has(_name):
        sfx_player.stream = sounds[_name]
        sfx_player.play()
    else:
        push_error("Sound effect not found: " + _name)

# Stop all sound effects
func stop_sfx():
    sfx_player.stop()

# Adjust music volume
func set_music_volume(volume: float):
    music_volume = clamp(volume, 0.0, 1.0)
    _update_volumes()

# Adjust sound effects volume
func set_sfx_volume(volume: float):
    sfx_volume = clamp(volume, 0.0, 1.0)
    _update_volumes()
