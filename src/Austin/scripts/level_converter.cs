/*
using DifficultyCalculator;
using RoundManager;
using Godot;
using Castle.Components.DictionaryAdapter;

public abstract class LevelConverter {
    protected Level level;

    public LevelConverter() {
        level = GD.Load<Level>("res://src/Austin/scripts/custom_resources/Level.cs");
    }

    public LevelConverter(Level levelData) {
        level = levelData;
    }

    public abstract void setMap(PackedScene mapScene);

    public abstract void setDifficulty(Difficulty difficulty);

    public Level getLevel() {
        return level;
    }
}

public class SavedResourceConverter : LevelConverter {
    string resourcePath = "";

    public SavedResourceConverter(string path, Level level) {
        resourcePath = path;
        this.level = level;
    }

    public SavedResourceConverter(string path) {
        resourcePath = path;

        level = new Level();
    }

    public override void setDifficulty(Difficulty difficulty) {
        level.setDifficulty(difficulty);
    }

    public override void setMap(PackedScene mapScene) {
        if (mapScene != null) {
            level.mapScene = mapScene;
        }
    }
}

public class ResourceInstanceConverter : LevelConverter {
    public override void setDifficulty(Difficulty difficulty) {
        level.setDifficulty(difficulty);
    }

    public override void setMap(PackedScene mapScene) {
        if (mapScene != null) {
            level.mapScene = mapScene;
        }
    }
}
*/