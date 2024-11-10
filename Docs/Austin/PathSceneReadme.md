Author: Austin Walker
Date: 11/7/2024

--- This Readme file is for the Path Scene --- \
# Description
The path scene is used to store and distribute the paths that a map contains.

A great question would be, why not use the Path2D node already provided by Godot?
The reasoning for this is some maps require the use of mutlple paths, and having a class to manage the paths makes it easy to get them.

# Structure in Godot
In Godot there is a specific structure that this scene expects:
1. A map should contain one of these path scenes
2. The path scene should have the paths stored as child Path2D nodes

# Functions
The path scene contains 2 important functions.
1. [_Ready(double detla)](#_Ready)
2. [getPath()](#getPath)

## _Ready
### Summary
Sets up the paths by storing references to the paths. With multiple paths gets this scene ready to return paths.

## getPath
### Summary
Used to get a path from the map.

### Return
Returns a Path2D that the map contains. If there are multple paths returns 1 path in a round robin pattern.

