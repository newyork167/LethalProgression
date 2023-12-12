thunderstore_plugins = "$Env:USERPROFILE\AppData\Roaming\Thunderstore Mod Manager\DataFolder\LethalCompany\profiles\Discord\BepInEx\plugins"
Write-Host "Thunderstore folder: $thunderstore_plugins"
lethal_progression_plugin_folder = "$thunderstore_plugins\lethal-progression"
Write-Host "Copying to $lethal_progression_plugin_folder"

New-Item -ItemType Directory -Force -Path $lethal_progression_plugin_folder
Copy-Item $dll_build -Destination "$lethal_progression_plugin_folder"