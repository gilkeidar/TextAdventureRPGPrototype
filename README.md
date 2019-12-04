# Welcome to the grand and wonderful world of...?

Sorry, no story yet, just a bare-bones prototype. Implemented basic combat and interaction functionalities. A player can pick up items, store them in inventory, examine them, equip them, use them, walk between rooms, and fight enemies. There are RPG elements as well, such as levels, HP, skills, and the like.

## To run the game

Go to the path: bin\Release and run the RPG.exe folder.

## Command List

Since there's no "help" command in the game, here is a list of all of the commands the player can use:

Command  | Usage
------------- | -------------
north or n  | The player moves one room on the map to the north.
south or s  | The player moves one room on the map to the south.
east or e   | The player moves one room on the map to the east.
west or w   | The player moves one room on the map to the west.
look | Prints the location name and description.
inventory | Pauses combat and prints the player's inventory, including player stats such as **level** and **HP**.
examine | Pauses combat and prints the name, description, and relevant information of an item or monster.
take or get | Lets a player pick up an item from the location and put it into their inventory.
wield or equip | Lets a player equip an item from their inventory or from the location.
unequip | Removes an item from an equipment slot and places it in the player's inventory.
drop | Removes an item from an equipment slot or from the player's inventory and places it in the location.
attack or fight | Needs to be followed by the name of a monster and the weapons used, e.g. "fight kobold with short sword and rusty sword" **To use a skill** you'd instead need to write, "fight kobold with skill sword flurry" (fight monsterName with skill skillName)
use | Uses an item if it can be used and administers its effects on the player or environment. Most likely, this action will consume the item.
read | The player can read an item if it is readible; current examples include Archives and the Sign on the Road in the first room of the game.
open | Opens a container item, and changes its examine description to include the items inside it.
close | Closes a container item, and changes its examine description such that it doesn't include the items inside of it.
put | Needs to be followed by the name of an item and the name of a container, and puts the item in the container item. (e.g. "put short sword in leather bag")
