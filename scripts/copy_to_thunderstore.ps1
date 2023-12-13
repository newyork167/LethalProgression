$solution_dir = $args[0]
$dll_build = "$solution_dir\LethalProgression\bin\Debug\netstandard2.1\LethalProgression.dll" 

$thunderstore_plugins = "$Env:USERPROFILE\AppData\Roaming\Thunderstore Mod Manager\DataFolder\LethalCompany\profiles\Discord\BepInEx\plugins"
$lethal_progression_plugin_folder = "$thunderstore_plugins\lethal-progression"

Write-Host "Thunderstore folder: $thunderstore_plugins"
Write-Host "Creating folder $lethal_progression_plugin_folder"
New-Item -ItemType Directory -Force -Path "$lethal_progression_plugin_folder"

Write-Host "Copying $dll_build to $lethal_progression_plugin_folder"
Copy-Item -Path "$dll_build" -Destination "$lethal_progression_plugin_folder"