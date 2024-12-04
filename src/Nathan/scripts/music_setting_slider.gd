extends HSlider

func update_volume(_value: float):
    SoundManager.set_music_volume(_value / 100)