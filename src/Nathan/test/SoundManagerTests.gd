extends GdUnitTestSuite
class_name SoundManagerTests

var manager: soundManager = null

func before_test():
   manager = soundManager.new()
   manager._ready()


func test_music_volume()->void:
    manager.set_music_volume(.5)
    assert_float(manager.music_volume).is_equal(.5)

func test_music_volume_underflow()->void:
    manager.set_music_volume(-1)
    assert_float(manager.music_volume).is_equal(0.0)

func test_music_volume_overflow()->void:
    manager.set_music_volume(10000)
    assert_float(manager.music_volume).is_equal(1.0)

func test_music_startup_volume()->void:
    assert_float(manager.music_volume).is_equal(.25)

func test_music_autoplay()->void:
    assert_bool(manager.music_player.autoplay).is_true()

func test_load_music()->void:
    manager.load_music("test_song", "res://src/Nathan/Assets/Magna Bovinia.mp3")
    assert_dict(manager.music).contains_keys(["test_song"])

func test_stop_music()->void:
    manager.stop_music()
    assert_bool(manager.music_player.playing).is_false()

func test_start_music()->void:
    manager.stop_music()
    manager.load_music("test_song", "res://src/Nathan/Assets/Magna Bovinia.mp3")
    manager.play_music("test_song")
    assert_bool(manager.music_player.playing).is_true()


func test_sfx_volume()->void:
    manager.set_sfx_volume(.5)
    assert_float(manager.sfx_volume).is_equal(.5)

func test_sfx_volume_underflow()->void:
    manager.set_sfx_volume(-1)
    assert_float(manager.sfx_volume).is_equal(0.0)

func test_sfx_volume_overflow()->void:
    manager.set_sfx_volume(10000)
    assert_float(manager.sfx_volume).is_equal(1.0)

func test_sfx_startup_volume()->void:
    assert_float(manager.sfx_volume).is_equal(.25)

func test_sfx_autoplay()->void:
    assert_bool(manager.sfx_player.autoplay).is_false()

func test_load_sfx()->void:
    manager.load_sfx("test_sfx", "res://src/Nathan/Assets/Magna Bovinia.mp3")
    assert_dict(manager.sfx).contains_keys(["test_sfx"])

func test_stop_sfx()->void:
    manager.stop_sfx()
    assert_bool(manager.sfx_player.playing).is_false()

func test_start_sfx()->void:
    manager.stop_sfx()
    manager.load_sfx("test", "res://src/Nathan/Assets/Magna Bovinia.mp3")
    manager.play_sfx("test")
    assert_bool(manager.sfx_player.playing).is_true()

func after_test():
    manager.free()
