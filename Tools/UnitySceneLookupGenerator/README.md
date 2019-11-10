# Scene Lookup Generator

This tool will read any file that suffix with .unity and make scene name as one member of new lookup class member name.

Mainly avoid Unity forbid code generator execute scripts in editor when project exist compiler error.

You should provide these information:

- project scenes file path

- output file path


Using '-h' for more information.

Example command line:
(Assuming in Terminal/Shell)
```Shell

mono SceneLookupGenerator.exe -s /Users/User/Documents/UnityProjectFolder/Hippocampus/Assets/Scenes -o /Users/User/Documents/UnityProjectFolder/Hippocampus/Assets/Scenes

```
