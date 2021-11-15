1.0.6

Changed:
- Improved spawning speed of trees by only recalculating spawn points when needed.
- Adding a new tree and closing the prefab picker will now simply create an empty tree (in case you want to drag in a specific prefab)

Fixed:
- Not being able to drag terrains into the terrain list
- Seed for tree spawn chances had them spawn in rows
- Undo/redo on tree parameters having no effect if "Auto respawn trees" was disabled in the settings

1.0.5

Added:
- Vegetation items now have an "enabled" checkbox. If disabled, it will not be spawned.
- Vegetation items now have a name label, used to more easily identify them in the inspector. (these will be automatically filled when updating)
- Grass patch size can now be configured under the settings tab. This controls how many batches the terrain internally creates.

Changed:
- A warning is now displayed if a tree prefab does not have a LOD Group component. In this case the terrain can't apply random rotation/scale.
- API now supports (re)spawning trees/grass on a specific terrain
- Improved progress bar for grass respawning

Fixed:
- Tree not respawning when modifying the minimum strength for a terrain layer mask

1.0.4

Added:
- Grass items now have a toggle for camera billboarding if a texture is used
- Noise spread value for grass can now also be configured
- Adding a tree now automatically opens a project prefab picker
- Settings tab now has the option to only respawn all grass or trees
- Added option to set the detail map resolution, since this control the minimum possible distance between grass items

Changed:
- UI improvements for usability
- Renamed namespace to "sc.terrain.vegetationspawner" for consistency between other terrain tools
- Secondary grass color value is now kept, even if linked to main color
- Removed some terrain settings, the component should only concern itself with vegetation related settings (added more of those!)
- (C#) SpawnerBase.GrassType.Billboard was renamed to SpawnerBase.GrassType.Texture. Since a grass item isn't always a billboard

Fixed:
- Sink amount for trees not having any effect
- Inspector window being forced to a fixed width under certain circumstances in the grass tab
- Layer mask selection not selecting the correct item in some cases

1.0.3

Added:
- Callback delegate for when a grass/tree item respawns (see documentation for example).
- Terrain extension function for getting a tree instances of a specific prefab.
- Water level is now visualized in the scene view when the Settings tab is active.
- Option to disable automatic respawning of trees when a spawn rule is modified.

Changed:
- Renamed "Opacity threshold" for terrain layer masks to "Minimum strength", for clarity.
- Height range maximum value is now based on the height of the largest terrain (instead of being fixed at 2000).
- Collision detection now works for non-square terrains

Fixed:
- UI error when switching to a scene with fewer grass items than the last

1.0.2

Fixed:
- Trees appearing black when using Tree Creator shaders.

1.0.1

Added:
- Grass items can now be duplicated.
- Exposed seed value in settings tab (global, added to each item's own seed).

Fixed:
- Last selected tree item respawning when changing grass spawn rules.

1.0.0
Initial release