extends HSlider


func update_volume(_value: float):
    SoundManager.set_sfx_volume(_value / 100)