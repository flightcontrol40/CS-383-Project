
// This file will not be used I just need it for my pattern...
// I am not going to document it since I am not going to put forth the effort to add it to the project.
using DifficultyCalculator;
using Godot;

namespace LevelConverter {

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

        public virtual Level getLevel() {
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
            ResourceSaver.Save(level, resourcePath);
        }

        public override void setMap(PackedScene mapScene) {
            if (mapScene != null) {
                level.mapScene = mapScene;
                ResourceSaver.Save(level, resourcePath);
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
}