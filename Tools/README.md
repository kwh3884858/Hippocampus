# Scene Lookup Generator

## Intro

Find all scene file and generate scene lookup file.

## Usage

mono SceneLookupGenerator.exe -s [your scene fold path] -0 [ your output fold path]

## Argument

-r: set scene root file path.

Generator will find the all scene file in the path.

-o: set output file path

Generated scene lookup file will put in this path.	

-h: show help

## Example 

```shell

mono SceneLookupGenerator.exe -s /Users/User/Documents/UnityProjectFolder/Hippocampus/Assets/Scenes -o /Users/User/Documents/UnityProjectFolder/Hippocampus/Assets/Scenes

```