# Taskbar Folders
This program is used to create miniature folders in the system tray. It can hold files, folders, and programs.

## How to use
1. Download the latest release from the releases page.
2. Place the EXE in your startup folder for ease of use. (type `shell:startup` in explorer to open the folder)
3. Open the EXE. It will create a default folder in the system tray. Click the folder to open it, and right click to add files, folders, and programs.

## Plugin API
The plugin API is used to add new features to the program. To make a plugin, make a .NET 4.7.2 class library that extends `TaskbarFolders.Extension` and override the methods you need. Then, place the DLL in %appdata%\TaskbarFolderPlugins. The plugin will be loaded the next time the program starts.
### Plugin API Features
- Context menu items
- Modify pin ListViewItems
- Modify folder form
- Add properties pages

## Reporting bugs
If you find a bug, please report it on the issues page.

## Support & Contact
If you need help, check out my Discord server: https://discord.gg/eN2rzhAErd 