# Difficulty Manager

## Description
The Difficulty Manager set of classes are used for calculating what and when to
spawn enemies in a Godot Project.

There are three variants of the difficulty calculator:
1. [EasyDifficultCalculator](@ref EasyDifficultCalculator)
2. [MediumDifficultCalculator](@ref MediumDifficultCalculator)
3. [HardDifficultCalculator](@ref HardDifficultCalculator)

All three can be acquired using the [DifficultyCalculatorFactory.DifficultyCalculator](@ref DifficultyCalculatorFactory) factory function

## Godot Classes
In Godot there is a specific structure that this scene expects:
1. A [Difficulty Enum](@ref Difficulty)
2. A [DifficultyTable](@ref DifficultyTable)

## Godot Functions
The enemy spawn orders can then be created with the [CalculateSpawnOrder](@ref DifficultyCalculator.CalculateSpawnOrder) function.
This function will return a List of [SpawnOrder](@ref SpawnOrder)

The full API can be viewed [here](https://flightcontrol40.github.io/CS-383-Project/classRoundManager_1_1DifficultyCalculator.html)
