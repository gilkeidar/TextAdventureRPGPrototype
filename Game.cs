using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Locations;
using RPG.Blueprints.Characters;
using RPG.Blueprints.Items;
using RPG.Factories;
using RPG.Story;
using RPG.Blueprints.Skills;
using RPG.Blueprints.Skills.PlayerSkills;

namespace RPG
{
	public class Game
	{
		//the game runner class
		bool isPlaying = true;
		public Location currentLocation;
		public Location oldLocation = null;
		Player currentPlayer;
		Monster currentMonsterFighting = null;
		States state = States.Peaceful; // 0 = peaceful, 1 = combat, 2 = trade?, 3 = dead

		//combat variables
		int playerAttackCounter = 0;
		bool playerMultipleAttack = false; //if true, then the speed of the player allows them to attack more than once
		bool playerPocketWeaponAttack = false; //if a player's first attack against a monster is with a pocket weapon, then if
											   //the player's speed > monster's speed, then the player will be able to attack again with both main weapons
		bool startedFight = false;

		int immmediateAttackTurnCounter = 0;
		int fleeMonsterCounter = 0; //if player flees a monster, they need to flee to a number of rooms based on the speed difference between the player and the monster

		public void GameLoop()
		{
			//game loop goes here
			while (isPlaying)
			{
				immmediateAttackTurnCounter++;
				if (currentLocation != oldLocation)
				{
					immmediateAttackTurnCounter = 0;

					GameMessageFactory.CheckLocationMessage(currentLocation);
					foreach(Monster monster in currentLocation.MonstersHere) //refactor!!!!!
					{
						if(monster != null)
						{
							GameMessageFactory.CheckMonsterMessage(monster);
						}
					}
					Console.WriteLine(currentLocation.Name);
					if(state == States.Peaceful)
					{
						Console.WriteLine(currentLocation.Description);
					}
				}
				oldLocation = currentLocation;
				if (state == States.Peaceful)
				{
					PrintPossibleDirections();
					//print items in location
					PrintItemsInLocation();
					//print monsters in location?
					PrintMonstersInLocation();

					if(currentPlayer.CurrentHP < currentPlayer.MaxHP)
					{
						currentPlayer.RegenerateHP();
					}
					if(currentPlayer.CurrentSP < currentPlayer.MaxSP)
					{
						currentPlayer.RegenerateSP();
					}
				}
				//if monster attacks player immediately, or maybe have some kind of counter? (1 turn after player enters an area)
				if(immmediateAttackTurnCounter >= 1 && !startedFight)
				{
					foreach(Monster monster in currentLocation.MonstersHere) //refactor!!!!!!!!!!!!!!!!!!!!!!!!!!!
					{
						if (monster.WillAttack)
						{
							currentMonsterFighting = monster;
							startedFight = true;
							state = States.Combat;
							MonsterAttack();
							break;
						}
					}
				}
				Console.WriteLine();
				Console.Write("> ");
				string userInput = Console.ReadLine();
				InputCommands(userInput);

				//Monster combat
				if (state == States.Combat) //if fighting a monster
				{
					if (fleeMonsterCounter >= currentMonsterFighting.Speed / currentPlayer.Speed && fleeMonsterCounter >= 1)
					{
						Console.WriteLine("You manage to flee the " + currentMonsterFighting.Name + "!");
						Console.WriteLine();
						fleeMonsterCounter = 0;
						currentMonsterFighting = null;
						state = States.Peaceful;
						startedFight = false;
					}
					else
					{
						MonsterAttack();
					}
				}
				if(currentMonsterFighting != null && state == States.Peaceful)
				{
					state = States.Combat; //if a player interrupted a fight by say checking inventory/examining item, then this continues the fight
				}
				//Console.WriteLine();
			}
		}

		public Game()
		{
			this.currentLocation = Map.GetLocation(0, 0); //first room
			this.currentPlayer = new Player("Player Name", 1, 0, 100, 100, 10, 10, 200);
		}

		public void MonsterAttack()
		{
			int damage = currentMonsterFighting.DoDamage();
			int lessDamage = damage;
			int netSpeed = currentMonsterFighting.Speed / currentPlayer.Speed;

			if (playerPocketWeaponAttack)
			{
				playerAttackCounter = 1;
				playerPocketWeaponAttack = false;
				//Console.WriteLine("Player attack counter:" + playerAttackCounter);
			}
			else if (netSpeed == 0 && !playerMultipleAttack)
			{
				netSpeed = currentPlayer.Speed / currentMonsterFighting.Speed;
				//Console.WriteLine("Net speed:" + netSpeed);
				playerAttackCounter = netSpeed - 1;
				playerMultipleAttack = true;
			}
			//Console.WriteLine("Player attack counter:" + playerAttackCounter);
			if (playerAttackCounter == 0)
			{
				playerMultipleAttack = false;
				for (int i = 0; i < netSpeed; i++)
				{
					if (currentPlayer.Armor != null)
					{
						lessDamage = damage - currentPlayer.Armor.Defense;
					}
					if (currentPlayer.Armor != null && lessDamage >= currentPlayer.Armor.Defense)
					{
						currentPlayer.CurrentHP -= lessDamage;
						Console.WriteLine("The " + currentMonsterFighting.Name + " attacks you! Your armor absorbed some of the blow! It does " + lessDamage + " damage.");
					}
					else if (currentPlayer.Armor != null && lessDamage < currentPlayer.Armor.Defense)
					{
						Console.WriteLine("The " + currentMonsterFighting.Name + " attacks you! Your armor absorbs the blow!");
					}
					else
					{
						currentPlayer.CurrentHP -= damage;
						Console.WriteLine("The " + currentMonsterFighting.Name + " attacks you! It does " + damage + " damage.");
					}
				}
			}
			if (playerAttackCounter > 0)
			{
				playerAttackCounter--;
			}

			if (currentPlayer.CurrentHP <= 0)
			{
				currentPlayer.CurrentHP = 0;
				state = States.Dead;
				Console.WriteLine("You have died!");
				Console.ReadLine();
				isPlaying = false;
			}
		}
		public void InputCommands(string input)
		{
			//figure out what the command is here, use a switch statement to use correct commands from the given input.

			string lowerCaseInput = input.ToLower();
			string[] lowerCaseCommands = lowerCaseInput.Split(' ');

			Console.WriteLine();
			switch (lowerCaseCommands[0])
			{
				case "n":
				case "north":
					Move('n');
					break;
				case "s":
				case "south":
					Move('s');
					break;
				case "e":
				case "east":
					Move('e');
					break;
				case "w":
				case "west":
					Move('w');
					break;
				case "look":
					Console.WriteLine(currentLocation.Name);
					Console.WriteLine(currentLocation.Description);
					break;
				case "inventory":
					state = States.Peaceful; //this is done so player can interrupt a fight by checking inventory
					PrintInventory();
					Console.WriteLine();
					break;
				case "examine":
					state = States.Peaceful; //this is done so player can interrupt a fight by examining an item/monster
					Examine(lowerCaseCommands);
					Console.WriteLine();
					break;
				case "take":
				case "get":
					GetItem(lowerCaseCommands);
					Console.WriteLine();
					break;
				case "wield":
				case "equip":
					Equip(lowerCaseCommands);
					Console.WriteLine();
					break;
				case "unequip":
					UnEquip(lowerCaseCommands);
					Console.WriteLine();
					break;
				case "drop":
					Drop(lowerCaseCommands);
					Console.WriteLine();
					break;
				case "attack":
				case "fight":
					if (!startedFight) //player needs to specify what monster they want to fight (syntax: > fight [monsterName] with [weapon/skill name] (and [second weapon name]))
						Fight2(lowerCaseCommands);
					else
						Fight3(lowerCaseCommands); //player doesn't need to specify the monster that they want to fight since they're already fighting them! (syntax: > fight with [weapon/skill name] (and [second weapon name]))
					break;
				case "use":
					Use(lowerCaseCommands);
					Console.WriteLine();
					break;
				case "read":
					Read(lowerCaseCommands);
					Console.WriteLine();
					break;
				case "open":
					Open(lowerCaseCommands);
					Console.WriteLine();
					break;
				case "close":
					Close(lowerCaseCommands);
					Console.WriteLine();
					break;
				case "put": //could mean a few things; make different commands and here have an if statement based on the second word or in the Put command itself to get more context as to the user's intent (put an object in a container)
							//or (put an object on another object) etc.
					Put(lowerCaseCommands);
					Console.WriteLine();
					break;
				default:
					Console.WriteLine("I don't understand what you said.");
					break;
			}
		}
		public void Move(char direction)
		{
			Location newLocation = null;
			switch (direction)
			{
				case 'n':
					newLocation = Map.GetLocation(currentLocation.X, currentLocation.Y - 1);
					break;
				case 's':
					newLocation = Map.GetLocation(currentLocation.X, currentLocation.Y + 1);
					break;
				case 'e':
					newLocation = Map.GetLocation(currentLocation.X + 1, currentLocation.Y);
					break;
				case 'w':
					newLocation = Map.GetLocation(currentLocation.X - 1, currentLocation.Y);
					break;
				default:
					break;
			}
			if (newLocation != null)
			{
				currentLocation = newLocation;
				if (currentMonsterFighting != null)
				{
					fleeMonsterCounter++;
				}
			}
			else
			{
				Console.WriteLine("Can't go that way.");
			}
		}
		public void PrintPossibleDirections()
		{
			Console.Write("You can go ");
			for (int i = 0; i < currentLocation.Directions.Count; i++)
			{
				if (currentLocation.Directions.Count == 1)
				{
					Console.WriteLine(currentLocation.Directions[i] + ".");
					break;
				}
				else if (currentLocation.Directions.Count == 2)
				{
					Console.WriteLine(currentLocation.Directions[0] + " and " + currentLocation.Directions[1] + ".");
					break;
				}
				else if (i == currentLocation.Directions.Count - 1)
				{
					Console.WriteLine("and " + currentLocation.Directions[i] + ".");
				}
				else
				{
					Console.Write(currentLocation.Directions[i] + ", ");
				}
			}
		}
		public void PrintItemsInLocation()
		{
			if (currentLocation.ItemsHere.Count >= 1)
			{
				if (currentLocation.ItemsHere.Count >= 1) //if lists are empty, then they have no entries whatsoever(?) (aka, count == 0)
				{
					Console.Write("There is a ");
				}
				for (int i = 0; i < currentLocation.ItemsHere.Count; i++)
				{
					if (currentLocation.ItemsHere.Count == 1)
					{
						Console.WriteLine(currentLocation.ItemsHere[i].Name + " here.");
					}
					else if (i == currentLocation.ItemsHere.Count - 1)
					{
						Console.WriteLine("and a " + currentLocation.ItemsHere[i].Name + " here.");
					}
					else
					{
						Console.Write(currentLocation.ItemsHere[i].Name + ", ");
					}
				}
			}

			//refactor this so that the loop only runs if there are items in the location.


		}
		public void PrintMonstersInLocation()
		{
			if (currentLocation.MonstersHere.Count >= 1)
			{
				Console.Write("There is a ");
				foreach(Monster monster in currentLocation.MonstersHere)
				{
					if(currentLocation.MonstersHere.Count == 1)
					{
						Console.WriteLine(monster.Name + " here.");
					}
					else if (monster == currentLocation.MonstersHere.Last())
					{
						Console.WriteLine("and a " + monster.Name + " here.");
					}
					else
					{
						Console.Write(monster.Name + ", ");
					}
				}
			}
		}
		public void PrintInventory() //Refactor
		{
			Console.WriteLine(currentPlayer.Name + " | HP: " + currentPlayer.CurrentHP + "/" + currentPlayer.MaxHP + " | SP: " + currentPlayer.CurrentSP + "/" + currentPlayer.MaxSP + " | Level: " + currentPlayer.Level + " | EXP: " + currentPlayer.ExperiencePoints + "/" + currentPlayer.EXPToNextLevel + " | Gold: " + currentPlayer.Gold); //EXP to nextlevel calculator
			Console.WriteLine();
			Console.WriteLine("Speed: " + currentPlayer.Speed + " | Weight of items: " + String.Format("{0:00.00}", currentPlayer.ItemWeight) + " kg");
			Console.WriteLine();
			Console.WriteLine("Your inventory: ");
			Console.WriteLine();
			if (currentPlayer.Inventory.Count != 0)
			{
				foreach (Item item in currentPlayer.Inventory)
				{
					Console.WriteLine(item.Name);
				}
			}
			else
			{
				Console.WriteLine("You currently have nothing in your inventory.");
			}
			Console.WriteLine();
			Console.WriteLine("Equipment:");
			Console.WriteLine();
			bool anyEquipped = false;
			if (currentPlayer.Weapons[0] != null && currentPlayer.Weapons[0].WeaponType == 2)
			{
				anyEquipped = true;
				Console.WriteLine("You're currently wielding a " + currentPlayer.Weapons[0].Name + " in both hands.");
			}
			else if (currentPlayer.Weapons[0] != null)
			{
				anyEquipped = true;
				Console.WriteLine("You're currently wielding a " + currentPlayer.Weapons[0].Name + " in your right hand.");
			}
			if (currentPlayer.Weapons[1] != null && currentPlayer.Weapons[1].WeaponType != 2)
			{
				anyEquipped = true;
				Console.WriteLine("You're currently wielding a " + currentPlayer.Weapons[1].Name + " in your left hand.");
			}
			if (currentPlayer.Weapons[2] != null)
			{
				anyEquipped = true;
				Console.WriteLine("You have a " + currentPlayer.Weapons[2].Name + " equipped in your pocket.");
			}
			//Console.WriteLine(anyEquipped);
			//Console.WriteLine(currentPlayer.Weapons[0] != null);
			if (!anyEquipped)
			{
				Console.WriteLine("You don't have any weapon equipped.");
			}

			if (currentPlayer.Armor != null)
			{
				Console.WriteLine("You are wearing " + currentPlayer.Armor.Name + ".");
			}
			else
			{
				Console.WriteLine("You aren't wearing any armor beside your normal clothes.");
			}

			
			//Refactor
			/*if(currentPlayer.RightHandWeapon != null && currentPlayer.RightHandWeapon.WeaponType == 2)
			{
				Console.WriteLine("You're currently wielding a " + currentPlayer.RightHandWeapon.Name + " in both hands.");
			}
			else if (currentPlayer.RightHandWeapon != null)
			{
				Console.WriteLine("You're currently wielding a " + currentPlayer.RightHandWeapon.Name + " in your right hand.");
			}
			if(currentPlayer.LeftHandWeapon != null && currentPlayer.LeftHandWeapon.WeaponType != 2)
			{
				Console.WriteLine("You're currently wielding a " + currentPlayer.LeftHandWeapon.Name + " in your left hand.");
			}
			if(currentPlayer.PocketWeapon != null)
			{
				Console.WriteLine("You have a " + currentPlayer.PocketWeapon.Name + " equipped in your pocket.");
			}
			if(currentPlayer.Armor != null)
			{
				Console.WriteLine("You are wearing " + currentPlayer.Armor.Name + ".");
			}
			if(currentPlayer.PocketWeapon == null && currentPlayer.RightHandWeapon == null && currentPlayer.LeftHandWeapon == null)
			{
				Console.WriteLine("You don't have any weapon equipped.");
			}*/


		}
		public void Examine(string[] lowerCaseInputArray)
		{
			string itemExamined = "";
			if (lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What do you wish to examine?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					sB.Append(lowerCaseInputArray[i]);
					sB.Append(' ');
				}
				string newString = sB.ToString();
				//Console.WriteLine("newString =" + newString);

				/*for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					if (lowerCaseInputArray.Length == 2)
					{
						itemExamined = lowerCaseInputArray[i];
					}
					else if (i == lowerCaseInputArray.Length - 1)
					{
						itemExamined += lowerCaseInputArray[i];
					}
					else
					{
						itemExamined += lowerCaseInputArray[i] + " ";
					}
				}*/
				//Console.WriteLine("itemExamined =" + itemExamined);
				string itemExaminedTrim = newString.Trim();
				//Console.WriteLine("itemExamined.Trim() =" + itemExamined.Trim());
				Item foundItem = null;
				Monster foundMonster = null;
				foreach (Item item in currentLocation.ItemsHere)
				{
					if (item.Name.ToLower() == itemExaminedTrim)
					{
						foundItem = item;
					}
				}
				if (foundItem == null)
				{
					foreach (Item item in currentPlayer.Inventory)
					{
						if (item.Name.ToLower() == itemExaminedTrim)
						{
							foundItem = item;
						}
					}
				}
				if (foundItem == null)
				{
					foreach (Weapon weapon in currentPlayer.Weapons)
					{
						if (weapon != null && weapon.Name.ToLower() == itemExaminedTrim)
						{
							foundItem = weapon;
						}
					}
					if (currentPlayer.Armor != null && currentPlayer.Armor.Name.ToLower() == itemExaminedTrim) //check to see that maybe the player wants to examine armor
					{
						foundItem = currentPlayer.Armor;
					}
				}
				if (foundItem == null)
				{
					foreach (Monster monster in currentLocation.MonstersHere)
					{
						if (monster != null && monster.Name.ToLower() == itemExaminedTrim)
						{
							foundMonster = monster;
						}
					}
				}
				if (foundMonster != null)
				{
					Console.Write(foundMonster.Description);
					Console.Write(" | Level " + foundMonster.Level);
					Console.WriteLine(" | Speed: " + foundMonster.Speed);
				}
				else
				{
					if (foundItem != null)
					{
						Console.Write(foundItem.Description);
						string itemType = foundItem.GetClassType();

						//Console.WriteLine(itemType);
						switch (itemType)
						{
							case "Weapon":
								Console.Write(" | Type: ");
								Console.Write("Weapon | ");
								Weapon weaponItem = (Weapon)foundItem;
								switch (weaponItem.WeaponType)
								{
									case 1:
										Console.Write("1H");
										break;
									case 2:
										Console.Write("2H");
										break;
									case 3:
										Console.Write("Pocket");
										break;
									default:
										break;
								}
								Console.Write(" | ATK: " + weaponItem.Attack);
								break;
							case "Armor":
								Console.Write(" | Type: ");
								Armor armorItem = (Armor)foundItem;
								Console.Write("Armor | DEF: " + armorItem.Defense);
								break;
							case "HealingPotion":
								Console.Write(" | Type: ");
								HealingPotion healingPotion = (HealingPotion)foundItem;
								Console.Write("Healing Potion");
								break;
							case "Container":
								Container containerItem = (Container)foundItem;
								if (containerItem.Open)
								{
									Console.Write(" It is open, ");
									if(containerItem.Items.Count >= 1)
									{
										Console.Write("and inside is a ");
										for(int i = 0; i < containerItem.Items.Count; i++)
										{
											if(containerItem.Items.Count == 1)
											{
												Console.Write(containerItem.Items[0].Name + ".");
											}
											else if (i == containerItem.Items.Count - 1)
											{
												Console.Write("and a " + containerItem.Items[i].Name + ".");
											}
											else
											{
												Console.Write(containerItem.Items[i].Name + ", "); 
											}
										}
									}
									else
									{
										Console.Write("yet there is nothing inside.");
									}
								}
								else
								{
									Console.Write(" It is closed.");
								}
								Console.Write(" | Type: Container");
								break;
							case "Archive":
								Console.Write(" | It can be read.");
								Console.Write(" | Type: Archive");
								break;
							case "Item":
								Console.Write(" | Type: ");
								Console.Write("Item");
								break;
							default:
								break;
						}
						Console.Write(" | Weight: " + String.Format("{0:0.00}", foundItem.Weight) + " kg");
						Console.WriteLine();
					}
					else
					{
						Console.WriteLine("There is no such item here.");
					}
				}

			}
		}
		public void GetItem(string[] lowerCaseInputArray)
		{
			string inputItemGet = "";
			if (lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What do you wish to get?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					sB.Append(lowerCaseInputArray[i]);
					sB.Append(' ');
				}
				string newString = sB.ToString();
				string itemGetTrim = newString.Trim();
				/*for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					if (lowerCaseInputArray.Length == 2)
					{
						inputItemGet = lowerCaseInputArray[i];
					}
					else if (i == lowerCaseInputArray.Length - 1)
					{
						inputItemGet += lowerCaseInputArray[i];
					}
					else
					{
						inputItemGet += lowerCaseInputArray[i] + " ";
					}
				}*/

				bool itemExists = false;
				Item itemReceived = null;
				foreach (Item item in currentLocation.ItemsHere)
				{
					if (item != null && item.Name.ToLower() == itemGetTrim)
					{
						itemReceived = item;
						if (item.CanBeTaken == false)
						{
							itemExists = true;
							Console.WriteLine("The " + itemReceived.Name + " seems to be set firmly in place.");
							break;
						}
						else if (currentPlayer.ItemWeight + item.Weight >= currentPlayer.MaxItemWeight)
						{
							itemExists = true;
							Console.WriteLine("You cannot pick up " + item.Name + " due to its weight.");
							break;
						}
						else
						{
							currentPlayer.Inventory.Add(item);
							itemExists = true;
							currentLocation.ItemsHere.Remove(item);
							currentPlayer.ItemWeight += item.Weight;
							Console.WriteLine("You picked up a " + item.Name + ".");
							GameMessageFactory.CheckItemMessage(item);
							break;
						}
					}
					else if(item != null && item.GetClassType() == "Container")
					{
						Container itemContainer = (Container)item;
						if(itemContainer.Open && itemContainer.Items != null)
						{
							foreach(Item itemInContainer in itemContainer.Items)
							{
								if(itemInContainer.Name.ToLower() == itemGetTrim)
								{
									currentPlayer.Inventory.Add(itemInContainer);
									itemExists = true;
									itemContainer.Items.Remove(itemInContainer);
									itemContainer.Weight -= itemInContainer.Weight;
									currentPlayer.ItemWeight += itemInContainer.Weight;
									Console.WriteLine("You take out a " + itemInContainer.Name + " from the " + itemContainer.Name + ".");
									GameMessageFactory.CheckItemMessage(itemInContainer);
									break;
								}
							}
						}
					}
				}
				if (!itemExists)
				{
					Console.WriteLine("There is no such item here.");
				}
			}
		}
		public void Equip(string[] lowerCaseInputArray) //Refactor
		{
			string inputItemEquip = "";
			bool itemExists = false;
			if (lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What do you wish to equip?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					sB.Append(lowerCaseInputArray[i]);
					sB.Append(' ');
				}
				string newString = sB.ToString();
				string itemGetTrim = newString.Trim();
				/*for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					if (lowerCaseInputArray.Length == 2)
					{
						inputItemEquip = lowerCaseInputArray[i];
					}
					else if (i == lowerCaseInputArray.Length - 1)
					{
						inputItemEquip += lowerCaseInputArray[i];
					}
					else
					{
						inputItemEquip += lowerCaseInputArray[i] + " ";
					}
				}*/

				Item itemEquip = null;
				int sourceItem = 0; //if it's 1, then it's from the CurrentLocation; if it's 2, then it's from the player's inventory
				foreach (Item item in currentLocation.ItemsHere)
				{
					if (item != null && item.Name.ToLower() == itemGetTrim) //&& currentPlayer.RightHandWeapon.Name != inputItemEquip && currentPlayer.LeftHandWeapon.Name != inputItemEquip && currentPlayer.PocketWeapon.Name != inputItemEquip && currentPlayer.Armor.Name != inputItemEquip)
					{
						if(item.CanBeTaken == false)
						{
							itemExists = true;
							Console.WriteLine("You cannot equip the " + item.Name + ", as it seems to be set firmly in place.");
							break;
						}
						else if (currentPlayer.ItemWeight + item.Weight >= currentPlayer.MaxItemWeight)
						{
							itemExists = true;
							Console.WriteLine("You cannot pick up " + item.Name + " due to its weight.");
							break;
						}
						else
						{
							itemExists = true;
							itemEquip = item;
							currentPlayer.ItemWeight += (item.Weight)/2.0f; //equipping item --> only adds 50% of the item's weight to the player's ItemWeight
							currentLocation.ItemsHere.Remove(item);
							sourceItem = 1;
							break;
						}
					}
				}
				if (itemEquip == null)
				{
					foreach (Item item in currentPlayer.Inventory)
					{
						if (item != null && item.Name.ToLower() == itemGetTrim) //&& currentPlayer.RightHandWeapon.Name != inputItemEquip && currentPlayer.LeftHandWeapon.Name != inputItemEquip && currentPlayer.PocketWeapon.Name != inputItemEquip && currentPlayer.Armor.Name != inputItemEquip)
						{
							itemExists = true;
							itemEquip = item;
							currentPlayer.Inventory.Remove(item);
							currentPlayer.ItemWeight -= (item.Weight) / 2.0f; //equipping from inventory --> reduce 50% of item's weight
							sourceItem = 2;
							break;
						}
					}
				}
				bool gotItem = false;
				if (itemEquip != null)
				{
					string itemType = itemEquip.GetClassType();
					switch (itemType)
					{
						case "Armor":
							currentPlayer.Armor = (Armor)itemEquip;
							Console.WriteLine("You equipped " + itemEquip.Name + ".");
							GameMessageFactory.CheckItemMessage(itemEquip);
							gotItem = true;
							break;
						case "Weapon":
							Weapon currentItem = (Weapon)itemEquip;
							switch (currentItem.WeaponType)
							{
								case 1:
									Console.WriteLine("With which hand do you wish to wield this weapon?");
									string userInput = Console.ReadLine();
									switch (userInput)
									{
										case "r":
										case "right":
											gotItem = true;
											if(currentPlayer.Weapons[0] != null) //replacing currently wielded item with the new one and putting the old one in the inventory
											{
												if (currentPlayer.Weapons[0].WeaponType == 2)
												{
													currentPlayer.Weapons[1] = null;
												}
												currentPlayer.Inventory.Add(currentPlayer.Weapons[0]);
											}
											currentPlayer.Weapons[0] = currentItem;
											Console.WriteLine("You are wielding " + currentItem.Name + " in your right hand.");
											GameMessageFactory.CheckItemMessage(currentItem);
											break;
										case "l":
										case "left":
											gotItem = true;
											if (currentPlayer.Weapons[1] != null) //replacing currently wielded item with the new one and putting the old one in the inventory
											{
												if (currentPlayer.Weapons[1].WeaponType == 2)
												{
													currentPlayer.Weapons[0] = null;
												}
												currentPlayer.Inventory.Add(currentPlayer.Weapons[1]);
											}
											currentPlayer.Weapons[1] = currentItem;
											Console.WriteLine("You are wielding " + currentItem.Name + " in your left hand.");
											GameMessageFactory.CheckItemMessage(currentItem);
											break;
										default:
											Console.WriteLine("You have to specify with which hand you wish to wield your weapon.");
											break;
									}
									break;
								case 2:
									gotItem = true;
									if (currentPlayer.Weapons[0] != null) //replacing currently wielded item with the new one and putting the old one in the inventory
									{
										if(currentPlayer.Weapons[0].WeaponType == 2)
										{
											currentPlayer.Weapons[1] = null;
										}
										currentPlayer.Inventory.Add(currentPlayer.Weapons[0]);
									}
									if (currentPlayer.Weapons[1] != null) //replacing currently wielded item with the new one and putting the old one in the inventory
									{
										currentPlayer.Inventory.Add(currentPlayer.Weapons[1]);
									}
									currentPlayer.Weapons[0] = currentItem;
									currentPlayer.Weapons[1] = currentItem;
									Console.WriteLine("You are wielding " + currentItem.Name + " in both hands.");
									GameMessageFactory.CheckItemMessage(currentItem);
									break;
								case 3:
									gotItem = true;
									if (currentPlayer.Weapons[2] != null) //replacing currently wielded item with the new one and putting the old one in the inventory
									{
										currentPlayer.Inventory.Add(currentPlayer.Weapons[2]);
									}
									currentPlayer.Weapons[2] = currentItem;
									Console.WriteLine("You have equipped " + currentItem.Name + " in your pocket.");
									GameMessageFactory.CheckItemMessage(currentItem);
									break;
								default:
									break;

							}
							break;
						default:
							Console.WriteLine("You can't equip this item.");
							break;
					}
					if (!gotItem) //returning the item to where it was if the player doesn't equip it.
					{
						if(sourceItem == 1)
						{
							currentLocation.ItemsHere.Add(itemEquip);
							currentPlayer.ItemWeight -= (itemEquip.Weight) / 2.0f; //undoing weight change
						}
						else if (sourceItem == 2)
						{
							currentPlayer.Inventory.Add(itemEquip);
							currentPlayer.ItemWeight += (itemEquip.Weight) / 2.0f; //undoing weight change
						}
					}
				}
				else if (!itemExists)
				{
					Console.WriteLine("There is no such item here.");
				}
			}
			
		}
		public void UnEquip(string[] lowerCaseInputArray)
		{
			//Unequip code goes here
			if(lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What do you wish to unequip?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					sB.Append(lowerCaseInputArray[i]);
					sB.Append(' ');
				}
				string newString = sB.ToString();
				string itemInputUnEquipTrim = newString.Trim();

				Item itemUnEquip = null;
				for(int i = 0; i < currentPlayer.Weapons.Length; i++)
				{
					if(currentPlayer.Weapons[i] != null && currentPlayer.Weapons[i].Name.ToLower() == itemInputUnEquipTrim)
					{
						if(currentPlayer.Weapons[i].WeaponType == 2) //needs to remove two-handed weapon from other hand as well
						{
							if(i == 1)
							{
								currentPlayer.Weapons[0] = null;
							}
							else
							{
								currentPlayer.Weapons[1] = null;
							}
						}
						itemUnEquip = currentPlayer.Weapons[i];
						currentPlayer.Inventory.Add(currentPlayer.Weapons[i]);
						currentPlayer.Weapons[i] = null;
						currentPlayer.ItemWeight -= (itemUnEquip.Weight) / 2.0f;
						Console.WriteLine("You have unequipped " + itemUnEquip.Name +".");
					}
				}
				if(itemUnEquip == null && currentPlayer.Armor != null && currentPlayer.Armor.Name.ToLower() == itemInputUnEquipTrim)
				{
					itemUnEquip = currentPlayer.Armor;
					currentPlayer.Inventory.Add(currentPlayer.Armor);
					currentPlayer.Armor = null;
					currentPlayer.ItemWeight -= (itemUnEquip.Weight) / 2.0f;
					Console.WriteLine("You have unequipped " + itemUnEquip.Name + ".");
				}
				if(itemUnEquip == null)
				{
					Console.WriteLine("You have no such item equipped.");
				}
			}
		}
		public void Drop(string[] lowerCaseInputArray)
		{
			if(lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What do you wish to drop?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					sB.Append(lowerCaseInputArray[i]);
					sB.Append(' ');
				}
				string newString = sB.ToString();
				string itemInputDropTrim = newString.Trim();

				Item itemDrop = null;
				foreach(Item item in currentPlayer.Inventory) //check inventory
				{
					if(item != null && item.Name.ToLower() == itemInputDropTrim)
					{
						itemDrop = item;
						currentPlayer.Inventory.Remove(item);
						currentLocation.ItemsHere.Add(itemDrop);
						currentPlayer.ItemWeight -= itemDrop.Weight;
						Console.WriteLine("You dropped " + itemDrop.Name + ".");
						break;
					}
				}
				if(itemDrop == null) //check equipped weapons
				{ 
					for(int i = 0; i < currentPlayer.Weapons.Length; i++)
					{
						if(currentPlayer.Weapons[i] != null && currentPlayer.Weapons[i].Name.ToLower() == itemInputDropTrim)
						{
							if(currentPlayer.Weapons[i].WeaponType == 2)
							{
								if(i == 1)
								{
									currentPlayer.Weapons[0] = null;
								}
								else
								{
									currentPlayer.Weapons[1] = null;
								}
							}
							itemDrop = currentPlayer.Weapons[i];
							currentPlayer.Weapons[i] = null;
							currentLocation.ItemsHere.Add(itemDrop);
							currentPlayer.ItemWeight -= (itemDrop.Weight) / 2.0f;
							Console.WriteLine("You dropped " + itemDrop.Name + ".");
							break;
						}
					}
				}
				if(itemDrop == null)
				{
					if (currentPlayer.Armor != null && currentPlayer.Armor.Name.ToLower() == itemInputDropTrim)
					{
						itemDrop = currentPlayer.Armor;
						currentPlayer.Armor = null;
						currentLocation.ItemsHere.Add(itemDrop);
						Console.WriteLine("You dropped " + itemDrop.Name + ".");
					}
				}
				if(itemDrop == null)
				{
					Console.WriteLine("You have no such item to drop.");
				}
			}
		}
		public void Fight(string[] lowerCaseInputArray)
		{
			if(currentMonsterFighting == null)
			{
				if (lowerCaseInputArray.Length == 1)
				{
					Console.WriteLine("Whom do you wish to fight?");
				}
				else
				{
					StringBuilder sB = new StringBuilder();
					for (int i = 1; i < lowerCaseInputArray.Length; i++)
					{
						sB.Append(lowerCaseInputArray[i]);
						sB.Append(' ');
					}
					string newString = sB.ToString();
					string monsterTrim = newString.Trim();

					Monster monsterFight = null;
					foreach (Monster monster in currentLocation.MonstersHere)
					{
						if (monster.Name.ToLower() == monsterTrim)
						{
							monsterFight = monster;
						}
					}
					if (monsterFight != null)
					{
						state = States.Combat;
						currentMonsterFighting = monsterFight;
					}
					else
					{
						Console.WriteLine("There is no such monster here.");
					}
				}
			}
			else
			{
				if (lowerCaseInputArray.Length == 1)
				{
					Console.WriteLine("Attack with what?");
				}
				else
				{
					
					StringBuilder sB = new StringBuilder();
					bool twoWeapons = false; //check to see if player wants to attack with two 1H weapons
					string firstWeapon; //name of first weapon
					string secondWeapon; //name of second weapon
					int wordStop = 0; //index of "and" in input array
					Weapon attackFirstWeapon = null; //if only one weapon, only use attackFirstWeapon
					Weapon attackSecondWeapon = null;
					for (int i = 1; i < lowerCaseInputArray.Length; i++)
					{
						if (lowerCaseInputArray[i] == "and")
						{
							twoWeapons = true;
							wordStop = i;
							break;
						}
					}
					if (twoWeapons)
					{
						for(int i = 1; i < wordStop; i++)
						{
							sB.Append(lowerCaseInputArray[i]);
							sB.Append(' ');
						}
						firstWeapon = sB.ToString().Trim();
						sB.Clear();
						Console.WriteLine("first weapon: " + firstWeapon);

						for(int i = 0; i < currentPlayer.Weapons.Length; i++)
						{
							if(currentPlayer.Weapons[i] != null && currentPlayer.Weapons[i].Name.ToLower() == firstWeapon)
							{
								if (currentPlayer.Weapons[i].WeaponType == 2)
								{
									Console.WriteLine("You can't attack with multiple weapons if one of the weapons you're using is a two handed weapon!");
									attackFirstWeapon = currentPlayer.Weapons[i];
									attackSecondWeapon = null;
								}
								else
								{
									attackFirstWeapon = currentPlayer.Weapons[i];
								}
								break;
							}
						}

						for(int i = wordStop + 1; i < lowerCaseInputArray.Length; i++)
						{
							sB.Append(lowerCaseInputArray[i]);
							sB.Append(' ');
						}
						secondWeapon = sB.ToString().Trim();
						Console.WriteLine("second weapon: " + secondWeapon);

						for (int i = 0; i < currentPlayer.Weapons.Length; i++)
						{
							if (currentPlayer.Weapons[i] != null && currentPlayer.Weapons[i].Name.ToLower() == secondWeapon)
							{
								if (currentPlayer.Weapons[i].WeaponType == 2)
								{
									Console.WriteLine("You can't attack with multiple weapons if one of the weapons you're using is a two handed weapon!");
									attackFirstWeapon = currentPlayer.Weapons[i];
									attackSecondWeapon = null;
								}
								else if (attackFirstWeapon.WeaponType ==2)
								{
									attackSecondWeapon = null;	
								}
								else
								{
									attackSecondWeapon = currentPlayer.Weapons[i];
								}
								break;
							}
						}

					}
					else
					{
						for (int i = 1; i < lowerCaseInputArray.Length; i++)
						{
							sB.Append(lowerCaseInputArray[i]);
							sB.Append(' ');
						}
						firstWeapon = sB.ToString().Trim();
						for (int i = 0; i < currentPlayer.Weapons.Length; i++)
						{
							if (currentPlayer.Weapons[i] != null && currentPlayer.Weapons[i].Name.ToLower() == firstWeapon)
							{
								attackFirstWeapon = currentPlayer.Weapons[i];
							}
						}
					}
					if (attackFirstWeapon != null)
					{
						int playerDamage = 0;
						if(attackSecondWeapon == null)
						{
							playerDamage = currentPlayer.Attack(attackFirstWeapon);
							Console.WriteLine("You attack " + currentMonsterFighting.Name + " with " + attackFirstWeapon.Name + ". You do " + playerDamage + " damage!");
						}
						else
						{
							playerDamage = currentPlayer.Attack(attackFirstWeapon) + currentPlayer.Attack(attackSecondWeapon); //maybe change this formula later to balance the game with two handed weapons
							Console.WriteLine("You attack " + currentMonsterFighting.Name + " with " + attackFirstWeapon.Name + " and " + attackSecondWeapon.Name + ". You do " + playerDamage + " damage!");
						}
						
						currentMonsterFighting.CurrentHP -= playerDamage;
						if (currentMonsterFighting.CurrentHP <= 0)
						{
							Console.WriteLine("You felled the " + currentMonsterFighting.Name + "!");
							//Get loot
							if (currentMonsterFighting.Loot.Count >= 1)
							{
								for (int i = 0; i < currentMonsterFighting.Loot.Count; i++)
								{
									bool getItem = RandomNumberGenerator.GetDrop(currentMonsterFighting.PercentLoot[i]);
									if (getItem)
									{
										Console.WriteLine("The " + currentMonsterFighting.Name + " dropped " + currentMonsterFighting.Loot[i].Name + ".");
										currentLocation.ItemsHere.Add(currentMonsterFighting.Loot[i]);
									}
								}
							}
							if (currentMonsterFighting.GoldReward != 0)
							{
								currentPlayer.Gold += currentMonsterFighting.GoldReward;
								Console.WriteLine("You got " + currentMonsterFighting.GoldReward + " gold.");
							}
							if (currentMonsterFighting.ExperiencePointsReward != 0)
							{
								currentPlayer.ExperiencePoints += currentMonsterFighting.ExperiencePointsReward;
								Console.WriteLine("You got " + currentMonsterFighting.ExperiencePointsReward + " XP.");
							}

							currentLocation.MonstersHere.Remove(currentMonsterFighting);
							currentMonsterFighting = null;
							state = States.Peaceful; //setting the state back to peaceful
						}
					}
					else
					{
						Console.WriteLine("You don't have a weapon by that name!");
					}
				}
			}
		}

		public void Fight2(string[] lowerCaseInputArray)
		{
			if(lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("Whom do you wish to fight?");
			}
			else
			{
				//Rewrite combat system!!
				int withIndex = 0;
				int andIndex = 0;
				int skillIndex = 0;
				bool fightingWithSkill = false;
				bool twoWeapons = false;
				for(int i = 0; i < lowerCaseInputArray.Length; i++)
				{
					if (lowerCaseInputArray[i] == "with")
					{
						withIndex = i;
					}
					else if (lowerCaseInputArray[i] == "and")
					{
						andIndex = i;
						twoWeapons = true;
					}
					else if (lowerCaseInputArray[i] == "skill")
					{
						fightingWithSkill = true;
						skillIndex = i;
					}
				}
				if(withIndex == 0)
				{
					Console.WriteLine("You need to attack using a weapon or a skill!");
				}
				else
				{
					StringBuilder sB = new StringBuilder();
					string monster = "";
					Monster monsterToFight = null;
					for(int i = 1; i < withIndex; i++)
					{
						sB.Append(lowerCaseInputArray[i]);
						sB.Append(" ");
					}
					monster = sB.ToString().Trim(); //monster name that player wants to fight
					sB.Clear();

					foreach(Monster monsterFight in currentLocation.MonstersHere)
					{
						if(monsterFight.Name.ToLower() == monster)
						{
							if (currentMonsterFighting != null)
							{
								if(monster != currentMonsterFighting.Name.ToLower())
								{
									Console.WriteLine("You can't attack multiple monsters at once! What is this, World of Warcraft?");
									state = States.Peaceful; //not penalizing player for trying to switch monsters
									monsterToFight = currentMonsterFighting;
								}
								else
								{
									monsterToFight = currentMonsterFighting;
								}
							}
							else
							{
								monsterToFight = monsterFight;
								currentMonsterFighting = monsterFight;
								state = States.Combat; //combat state
							}
						}
					}
					//Console.WriteLine("Current Monster Fightng: " + currentMonsterFighting.Name);
					if(monsterToFight == null)
					{
						Console.WriteLine("There is no such monster here.");
						if(state == States.Combat)
						{
							state = States.Peaceful; //if player is currently fighting, don't penalize player for misspelling monster name
							//Console.WriteLine("Monster: " + currentMonsterFighting.Name);
						}
					}
					else
					{
						int playerDamage = 0;
						if (!fightingWithSkill) //fighting with base weapon damage
						{
							Weapon firstWeapon = null;
							Weapon secondWeapon = null;
							string firstWeaponChoice = "";
							string secondWeaponChoice = "";
							if (twoWeapons)
							{
								for (int i = withIndex + 1; i < andIndex; i++)
								{
									sB.Append(lowerCaseInputArray[i]);
									sB.Append(" ");
								}
								firstWeaponChoice = sB.ToString().Trim(); //weapon name that player wants to fight with
								sB.Clear();
							}
							else
							{
								for (int i = withIndex + 1; i < lowerCaseInputArray.Length; i++)
								{
									sB.Append(lowerCaseInputArray[i]);
									sB.Append(" ");
								}
								firstWeaponChoice = sB.ToString().Trim(); //weapon name that player wants to fight with
								sB.Clear();
							}
							foreach (Weapon weapon in currentPlayer.Weapons)
							{
								if (weapon != null && weapon.Name.ToLower() == firstWeaponChoice)
								{
									firstWeapon = weapon;
									if (firstWeapon.WeaponType == 2 && twoWeapons == true)
									{
										Console.WriteLine("You can't attack with two weapons if one of them is a two handed weapon!");
										break;
									}
								}
							}
							if (firstWeapon == null)
							{
								state = States.Peaceful; //pauses the fight so that the player doesn't get a penalty for misspelling weapon names
								if (!startedFight)
								{
									currentMonsterFighting = null; //doesn't start the fight if the player doesn't write the weapon name correctly
								}
								Console.WriteLine("You have no weapon by that name!");
							}

							if (twoWeapons && firstWeapon.WeaponType != 2)
							{
								for (int i = andIndex + 1; i < lowerCaseInputArray.Length; i++)
								{
									sB.Append(lowerCaseInputArray[i]);
									sB.Append(" ");
								}
								secondWeaponChoice = sB.ToString().Trim(); //weapon name of second weapon that player wants to fight with
								sB.Clear();

								foreach (Weapon weapon in currentPlayer.Weapons)
								{
									if (weapon != null && weapon.Name.ToLower() == secondWeaponChoice)
									{
										if (weapon.WeaponType == 2)
										{
											firstWeapon = weapon;
											secondWeapon = null;
											Console.WriteLine("You can't attack with two weapons if one of them is a two handed weapon!");
											break;
										}
										else
										{
											secondWeapon = weapon;
										}
									}
								}
							}

							//Actual fighting:

							if (firstWeapon != null && state == States.Combat)
							{
								playerDamage = 0;
								if (secondWeapon == null)
								{
									playerDamage = currentPlayer.Attack(firstWeapon);
									if (!startedFight && firstWeapon.WeaponType == 3 && currentPlayer.Speed > currentMonsterFighting.Speed)
									{
										playerPocketWeaponAttack = true;
										Console.WriteLine("You surprise attack " + currentMonsterFighting.Name + " with a " + firstWeapon.Name + ". You do " + playerDamage + " damage and stun your enemy!");
									}
									else
									{
										Console.WriteLine("You attack " + currentMonsterFighting.Name + " with " + firstWeapon.Name + ". You do " + playerDamage + " damage!");
									}
								}
								else
								{
									playerDamage = currentPlayer.Attack(firstWeapon) + currentPlayer.Attack(secondWeapon); //maybe change this formula later to balance the game with two handed weapons
									Console.WriteLine("You attack " + currentMonsterFighting.Name + " with " + firstWeapon.Name + " and " + secondWeapon.Name + ". You do " + playerDamage + " damage!");
								}
							}
						}
						else
						{ //fighting with a skill
							string skillName = "";
							PlayerSkill skill = null; //eventually make this a Skill type for when player might use Weapon Skills and other skills
							for (int i = skillIndex + 1; i < lowerCaseInputArray.Length; i++)
							{
								sB.Append(lowerCaseInputArray[i] + " ");
							}
							skillName = sB.ToString().Trim();
							bool skillExists = false;

							foreach (PlayerSkill someSkill in currentPlayer.PlayerSkills)
							{
								if (someSkill != null && someSkill.Name.ToLower() == skillName)
								{
									skill = someSkill;
									skillExists = true;
								}
							}

							if (skill == null)
							{
								Console.WriteLine("You have no skill by that name!");
								state = States.Peaceful; //pauses the fight so that the player doesn't get a penalty for misspelling weapon names
								if (!startedFight)
								{
									currentMonsterFighting = null; //doesn't start the fight if the player doesn't write the skill name correctly
								}
							}
							else if (currentPlayer.CurrentSP < skill.SPCost || currentPlayer.CurrentHP < skill.HPCost)
							{
								state = States.Peaceful; //pauses the fight so that the player doesn't get a penalty for not being able to use a skill
								if (!startedFight)
								{
									currentMonsterFighting = null; //doesn't start the fight if the player can't use a skill
								}
								if (currentPlayer.CurrentSP < skill.SPCost && currentPlayer.CurrentHP < skill.HPCost)
								{
									Console.WriteLine("Your HP and SP are too low to use this skill!");
								}
								else if (currentPlayer.CurrentSP < skill.SPCost)
								{
									Console.WriteLine("Your SP is too low to use this skill!");
								}
								else if(currentPlayer.CurrentHP < skill.HPCost)
								{
									Console.WriteLine("Your HP is too low to use this skill!");
								}
							}
							else
							{
								playerDamage = skill.UseSkill(currentPlayer);
								if(playerDamage == -1) // this occurs if the player doesn't meet the skill's conditions (e.g. for Sword Flurry this would occur if the player isn't wielding any weapons)
								{
									Console.WriteLine(skill.ErrorText); // maybe replace with a more helpful statement that's specific to each skill
									state = States.Peaceful; //pauses the fight so that the player doesn't get a penalty for misspelling weapon names
									if (!startedFight)
									{
										currentMonsterFighting = null; //doesn't start the fight if the player doesn't use the skill correctly 
									}
								}
								else
								{
									Console.WriteLine("You use the skill " + skill.Name + "!!!");
									skill.PrintUseSkill(currentPlayer);
									Console.WriteLine("You do " + playerDamage + " damage!!");
									if (skill.SPCost > 0)
									{
										currentPlayer.CurrentSP -= skill.SPCost;
										Console.WriteLine("Your SP decreases by " + skill.SPCost + " SP!!");
									}
									if (skill.HPCost > 0)
									{
										currentPlayer.CurrentHP -= skill.HPCost;
										Console.WriteLine("Your HP decreases by " + skill.HPCost + " HP!!");
									}
								}
							}
						}

						if (currentMonsterFighting != null)
						{
							startedFight = true;
							currentMonsterFighting.CurrentHP -= playerDamage;
							if (currentMonsterFighting.CurrentHP <= 0)
							{
								Console.WriteLine("You felled the " + currentMonsterFighting.Name + "!");
								//Get loot
								if (currentMonsterFighting.Loot.Count >= 1)
								{
									for (int i = 0; i < currentMonsterFighting.Loot.Count; i++)
									{
										bool getItem = RandomNumberGenerator.GetDrop(currentMonsterFighting.PercentLoot[i]);
										if (getItem)
										{
											Console.WriteLine("The " + currentMonsterFighting.Name + " dropped " + currentMonsterFighting.Loot[i].Name + ".");
											currentLocation.ItemsHere.Add(currentMonsterFighting.Loot[i]);
										}
									}
								}
								if (currentMonsterFighting.GoldReward != 0)
								{
									currentPlayer.Gold += currentMonsterFighting.GoldReward;
									Console.WriteLine("You got " + currentMonsterFighting.GoldReward + " gold.");
								}
								if (currentMonsterFighting.ExperiencePointsReward != 0)
								{
									int XPToAdd = currentMonsterFighting.ExperiencePointsReward;
									if (currentMonsterFighting.Level > currentPlayer.Level) //if monster's level is higher than player's level, give player more XP for defeating it.
									{
										XPToAdd = (int)(currentMonsterFighting.ExperiencePointsReward * (1 + (0.1 * (currentMonsterFighting.Level - currentPlayer.Level))));
									}
									Console.WriteLine("You got " + XPToAdd + " XP.");
									currentPlayer.ExperiencePoints += XPToAdd;
								}
								GameMessageFactory.CheckDefeatedMonsterMessage(currentMonsterFighting); //check to see if there's a game message about this particular monster
								currentLocation.MonstersHere.Remove(currentMonsterFighting);
								currentMonsterFighting = null;
								state = States.Peaceful; //setting the state back to peaceful
								startedFight = false; //reset value of startedFight
							}
						}
					}
				}
			}
		}

		public void Use(string[] lowerCaseInputArray)
		{
			//use item
			if(lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What item do you wish to use?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				string itemName = "";
				Item itemUsed = null;
				for(int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					sB.Append(lowerCaseInputArray[i]);
					sB.Append(" ");
				}
				itemName = sB.ToString().Trim();
				foreach(Item item in currentPlayer.Inventory)
				{
					if(item.Name.ToLower() == itemName)
					{
						itemUsed = item;
					}
				}
				if(itemUsed != null)
				{
					string itemType = itemUsed.GetClassType();
					switch (itemType)
					{
						case "HealingPotion":
							HealingPotion potion = (HealingPotion)itemUsed;
							int newHP = potion.Heal(currentPlayer.CurrentHP, currentPlayer.MaxHP);
							
							if (currentPlayer.CurrentHP == newHP)
							{
								Console.WriteLine("You used " + itemUsed.Name + ", but it had no effect.");
							}
							else
							{
								currentPlayer.CurrentHP = newHP;
								if(currentPlayer.CurrentHP == currentPlayer.MaxHP)
								{
									Console.WriteLine("You used " + itemUsed.Name + " and are now at max health!");
								}
								else
								{
									Console.WriteLine("You used " + itemUsed.Name + " and your health increased to " + currentPlayer.CurrentHP + "/" + currentPlayer.MaxHP + "!");
								}
								
							}
							currentPlayer.Inventory.Remove(itemUsed);
							break;
						default:
							Console.WriteLine("You can't use that item.");
							break;
					}
				}
				else
				{
					Console.WriteLine("You have no such item.");
				}
				
			}
		}

		public void Read(string[] lowerCaseInputArray)
		{
			if(lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What do you want to read?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				string itemName = "";
				Item itemRead = null;
				bool fromLocation = false;
				for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					sB.Append(lowerCaseInputArray[i]);
					sB.Append(" ");
				}
				itemName = sB.ToString().Trim();
				foreach (Item item in currentPlayer.Inventory)
				{
					if (item != null && item.Name.ToLower() == itemName)
					{
						itemRead = item;
						break;
					}
				}
				if(itemRead == null)
				{
					foreach (Item item in currentLocation.ItemsHere)
					{
						if (item != null && item.Name.ToLower() == itemName)
						{
							itemRead = item;
							fromLocation = true;
							break;
						}
					}
				}
				if(itemRead != null)
				{
					if (itemRead is Archive)
					{
						Archive itemArchive = (Archive)itemRead;
						if (currentPlayer.PlayerSkills.Contains(itemArchive.Skill)) //if a player reads an archive of a skill they already know, it levels up the skill.
						{
							Console.WriteLine("You open the Archive, and read:");
							Console.WriteLine();
							Console.WriteLine('"' + itemArchive.Message + '"');
							Console.WriteLine();
							Console.WriteLine("The pages begin to glow as from them you increase your understanding of your technique...");
							foreach (PlayerSkill skill in currentPlayer.PlayerSkills)
							{
								if (skill.Name == itemArchive.Skill.Name)
								{
									skill.Level++;
								}
							}
							Console.WriteLine(itemArchive.Skill.Name + " has increased a level! It is now Level " + itemArchive.Skill.Level);
							if (!fromLocation)
							{
								currentPlayer.Inventory.Remove(itemArchive);
							}
							else
							{
								currentLocation.ItemsHere.Remove(itemArchive);
							}
							Console.WriteLine("The Archive, having been read, is consumed into dust.");
						}
						else if (currentPlayer.Level >= itemArchive.Skill.LevelToUse)
						{
							Console.WriteLine("You open the Archive, and read:");
							Console.WriteLine();
							Console.WriteLine('"' + itemArchive.Message + '"');
							Console.WriteLine();
							Console.WriteLine("The pages begin to glow as from them you gain the knowledge of the ancients...");
							currentPlayer.PlayerSkills.Add(itemArchive.Skill);
							Console.WriteLine("You learned " + itemArchive.Skill.Name + "!!");
							if (!fromLocation)
							{
								currentPlayer.Inventory.Remove(itemArchive);
							}
							else
							{
								currentLocation.ItemsHere.Remove(itemArchive);
							}
							Console.WriteLine("The Archive, having been read, is consumed into dust.");

						}
						else
						{
							Console.WriteLine("Your level is not sufficient to read this Archive!");
						}
					}
					else if (itemRead.GetClassType() == "ReadableItem")
					{
						ReadableItem itemReading = (ReadableItem)itemRead;
						Console.WriteLine("You look closely at the " + itemReading.Name + ". It reads:");
						Console.WriteLine();
						Console.WriteLine('"' + itemReading.Message + '"');
					}
					else
					{
						Console.WriteLine("You cannot read this item.");
					}
				}
				else
				{
					Console.WriteLine("There is no such item here.");
				}
			}
		}

		public void Open(string[] lowerCaseInputArray)
		{
			if (lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What do you want to open?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				string itemName = "";
				Item itemOpen = null;
				for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					sB.Append(lowerCaseInputArray[i]);
					sB.Append(" ");
				}
				itemName = sB.ToString().Trim();
				foreach (Item item in currentPlayer.Inventory)
				{
					if (item != null && item.Name.ToLower() == itemName)
					{
						itemOpen = item;
					}
				}
				if (itemOpen == null)
				{
					foreach (Item item in currentLocation.ItemsHere)
					{
						if (item != null && item.Name.ToLower() == itemName)
						{
							itemOpen = item;
						}
					}
				}

				if(itemOpen != null)
				{
					if(itemOpen.GetClassType() == "Container")
					{
						Container itemContainer = (Container)itemOpen;
						itemContainer.Open = true;
						Console.WriteLine("You open the " + itemContainer.Name + ".");
					}
					else
					{
						Console.WriteLine("You can't open this item.");
					}
				}
				else
				{
					Console.WriteLine("There is no such item here.");
				}
			}
		}

		public void Close(string[] lowerCaseInputArray)
		{
			if (lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What do you want to close?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				string itemName = "";
				Item itemClose = null;
				for (int i = 1; i < lowerCaseInputArray.Length; i++)
				{
					sB.Append(lowerCaseInputArray[i]);
					sB.Append(" ");
				}
				itemName = sB.ToString().Trim();
				foreach (Item item in currentPlayer.Inventory)
				{
					if (item != null && item.Name.ToLower() == itemName)
					{
						itemClose = item;
					}
				}
				if (itemClose == null)
				{
					foreach (Item item in currentLocation.ItemsHere)
					{
						if (item != null && item.Name.ToLower() == itemName)
						{
							itemClose = item;
						}
					}
				}

				if (itemClose != null)
				{
					if (itemClose.GetClassType() == "Container")
					{
						Container itemContainer = (Container)itemClose;
						itemContainer.Open = false;
						Console.WriteLine("You close the " + itemContainer.Name + ".");
					}
					else
					{
						Console.WriteLine("You can't close this item.");
					}
				}
				else
				{
					Console.WriteLine("There is no such item here.");
				}
			}
		}

		public void Put(string[] lowerCaseInputArray)
		{
			if (lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("Put what?");
			}
			else
			{
				StringBuilder sB = new StringBuilder();
				int inIndex = 0;
				for(int i = 0; i < lowerCaseInputArray.Length; i++)
				{
					if(lowerCaseInputArray[i] == "in")
					{
						inIndex = i;
						break;
					}
				}
				if(inIndex == 0)
				{
					Console.WriteLine("You can't put something in thin air!");
				}
				else
				{
					string itemName = "";
					string containerName = "";
					Item itemPut = null;
					char itemSource = '0';
					Container itemContainer = null;
					for (int i = 1; i < inIndex; i++)
					{
						sB.Append(lowerCaseInputArray[i]);
						sB.Append(" ");
					}
					itemName = sB.ToString().Trim();
					sB.Clear();

					foreach(Item item in currentLocation.ItemsHere) //find item that player wants to put into the container in the current location & in the player's inventory
					{
						if(item != null && item.Name.ToLower() == itemName)
						{
							itemPut = item;
							itemSource = 'l'; //l = location
							break;
						}
					}
					if(itemPut == null)
					{
						foreach(Item item in currentPlayer.Inventory)
						{
							if(item != null && item.Name.ToLower() == itemName)
							{
								itemPut = item;
								itemSource = 'i'; //i = inventory
								break;
							}
						}
					}

					if (itemPut != null) //item found
					{
						for (int i = inIndex + 1; i < lowerCaseInputArray.Length; i++)
						{
							sB.Append(lowerCaseInputArray[i]);
							sB.Append(" ");
						}
						containerName = sB.ToString().Trim();

						foreach (Item container in currentLocation.ItemsHere) //find container that player wants to put the item into in the current location & in the player's inventory
						{
							if (container != null && container.Name.ToLower() == containerName && container.GetClassType() == "Container")
							{
								itemContainer = (Container)container;
								break;
							}
						}
						if (itemContainer == null)
						{
							foreach (Item container in currentPlayer.Inventory)
							{
								if (container != null && container.Name.ToLower() == containerName && container.GetClassType() == "Container")
								{
									itemContainer = (Container)container;
									break;
								}

							}
						}
						if (itemContainer != null)
						{
							if (itemContainer.Open)
							{
								itemContainer.Items.Add(itemPut);
								itemContainer.Weight += itemPut.Weight;
								if(itemSource == 'l')
								{
									currentLocation.ItemsHere.Remove(itemPut);
								}
								else if (itemSource == 'i')
								{
									currentPlayer.Inventory.Remove(itemPut);
								}
								Console.WriteLine("You put the " + itemPut.Name + " in the " + itemContainer.Name + ".");
							}
							else
							{
								Console.WriteLine("You need to open the " + itemContainer.Name + " first.");
							}
						}
						else
						{
							Console.WriteLine("There is no such container item here.");
						}
					}
					else
					{
						Console.WriteLine("There is no such item here.");
					}
				}
			}
		}

		public void Fight3(string[] lowerCaseInputArray)
		{
			if (lowerCaseInputArray.Length == 1)
			{
				Console.WriteLine("What do you want to fight with?");
			}
			else
			{
				//Rewrite combat system!!
				int withIndex = 0;
				int andIndex = 0;
				int skillIndex = 0;
				bool fightingWithSkill = false;
				bool twoWeapons = false;
				for (int i = 0; i < lowerCaseInputArray.Length; i++)
				{
					if (lowerCaseInputArray[i] == "with")
					{
						withIndex = i;
					}
					else if (lowerCaseInputArray[i] == "and")
					{
						andIndex = i;
						twoWeapons = true;
					}
					else if (lowerCaseInputArray[i] == "skill")
					{
						fightingWithSkill = true;
						skillIndex = i;
					}
				}
				if (withIndex == 0)
				{
					Console.WriteLine("You need to attack using a weapon or a skill!");
				}
				else
				{
					StringBuilder sB = new StringBuilder();
					int playerDamage = 0;
					if (!fightingWithSkill) //fighting with base weapon damage
					{
						Weapon firstWeapon = null;
						Weapon secondWeapon = null;
						string firstWeaponChoice = "";
						string secondWeaponChoice = "";
						if (twoWeapons)
						{
							for (int i = withIndex + 1; i < andIndex; i++)
							{
								sB.Append(lowerCaseInputArray[i]);
								sB.Append(" ");
							}
							firstWeaponChoice = sB.ToString().Trim(); //weapon name that player wants to fight with
							sB.Clear();
						}
						else
						{
							for (int i = withIndex + 1; i < lowerCaseInputArray.Length; i++)
							{
								sB.Append(lowerCaseInputArray[i]);
								sB.Append(" ");
							}
							firstWeaponChoice = sB.ToString().Trim(); //weapon name that player wants to fight with
							sB.Clear();
						}
						foreach (Weapon weapon in currentPlayer.Weapons)
						{
							if (weapon != null && weapon.Name.ToLower() == firstWeaponChoice)
							{
								firstWeapon = weapon;
								if (firstWeapon.WeaponType == 2 && twoWeapons == true)
								{
									Console.WriteLine("You can't attack with two weapons if one of them is a two handed weapon!");
									break;
								}
							}
						}
						if (firstWeapon == null)
						{
							state = States.Peaceful; //pauses the fight so that the player doesn't get a penalty for misspelling weapon names
							if (!startedFight)
							{
								currentMonsterFighting = null; //doesn't start the fight if the player doesn't write the weapon name correctly
							}
							Console.WriteLine("You have no weapon by that name!");
						}

						if (twoWeapons && firstWeapon.WeaponType != 2)
						{
							for (int i = andIndex + 1; i < lowerCaseInputArray.Length; i++)
							{
								sB.Append(lowerCaseInputArray[i]);
								sB.Append(" ");
							}
							secondWeaponChoice = sB.ToString().Trim(); //weapon name of second weapon that player wants to fight with
							sB.Clear();

							foreach (Weapon weapon in currentPlayer.Weapons)
							{
								if (weapon != null && weapon.Name.ToLower() == secondWeaponChoice)
								{
									if (weapon.WeaponType == 2)
									{
										firstWeapon = weapon;
										secondWeapon = null;
										Console.WriteLine("You can't attack with two weapons if one of them is a two handed weapon!");
										break;
									}
									else
									{
										secondWeapon = weapon;
									}
								}
							}
						}

						//Actual fighting:

						if (firstWeapon != null && state == States.Combat)
						{
							playerDamage = 0;
							if (secondWeapon == null)
							{
								playerDamage = currentPlayer.Attack(firstWeapon);
								if (!startedFight && firstWeapon.WeaponType == 3 && currentPlayer.Speed > currentMonsterFighting.Speed)
								{
									playerPocketWeaponAttack = true;
									Console.WriteLine("You surprise attack " + currentMonsterFighting.Name + " with a " + firstWeapon.Name + ". You do " + playerDamage + " damage and stun your enemy!");
								}
								else
								{
									Console.WriteLine("You attack " + currentMonsterFighting.Name + " with " + firstWeapon.Name + ". You do " + playerDamage + " damage!");
								}
							}
							else
							{
								playerDamage = currentPlayer.Attack(firstWeapon) + currentPlayer.Attack(secondWeapon); //maybe change this formula later to balance the game with two handed weapons
								Console.WriteLine("You attack " + currentMonsterFighting.Name + " with " + firstWeapon.Name + " and " + secondWeapon.Name + ". You do " + playerDamage + " damage!");
							}
						}
					}
					else
					{ //fighting with a skill
						string skillName = "";
						PlayerSkill skill = null; //eventually make this a Skill type for when player might use Weapon Skills and other skills
						for (int i = skillIndex + 1; i < lowerCaseInputArray.Length; i++)
						{
							sB.Append(lowerCaseInputArray[i] + " ");
						}
						skillName = sB.ToString().Trim();
						bool skillExists = false;

						foreach (PlayerSkill someSkill in currentPlayer.PlayerSkills)
						{
							if (someSkill != null && someSkill.Name.ToLower() == skillName)
							{
								skill = someSkill;
								skillExists = true;
							}
						}

						if (skill == null)
						{
							Console.WriteLine("You have no skill by that name!");
							state = States.Peaceful; //pauses the fight so that the player doesn't get a penalty for misspelling weapon names
							if (!startedFight)
							{
								currentMonsterFighting = null; //doesn't start the fight if the player doesn't write the skill name correctly
							}
						}
						else if (currentPlayer.CurrentSP < skill.SPCost || currentPlayer.CurrentHP < skill.HPCost)
						{
							state = States.Peaceful; //pauses the fight so that the player doesn't get a penalty for not being able to use a skill
							if (!startedFight)
							{
								currentMonsterFighting = null; //doesn't start the fight if the player can't use a skill
							}
							if (currentPlayer.CurrentSP < skill.SPCost && currentPlayer.CurrentHP < skill.HPCost)
							{
								Console.WriteLine("Your HP and SP are too low to use this skill!");
							}
							else if (currentPlayer.CurrentSP < skill.SPCost)
							{
								Console.WriteLine("Your SP is too low to use this skill!");
							}
							else if (currentPlayer.CurrentHP < skill.HPCost)
							{
								Console.WriteLine("Your HP is too low to use this skill!");
							}
						}
						else
						{
							playerDamage = skill.UseSkill(currentPlayer);
							if (playerDamage == -1) // this occurs if the player doesn't meet the skill's conditions (e.g. for Sword Flurry this would occur if the player isn't wielding any weapons)
							{
								Console.WriteLine(skill.ErrorText); // maybe replace with a more helpful statement that's specific to each skill
								state = States.Peaceful; //pauses the fight so that the player doesn't get a penalty for misspelling weapon names
								if (!startedFight)
								{
									currentMonsterFighting = null; //doesn't start the fight if the player doesn't use the skill correctly 
								}
							}
							else
							{
								Console.WriteLine("You use the skill " + skill.Name + "!!!");
								skill.PrintUseSkill(currentPlayer);
								Console.WriteLine("You do " + playerDamage + " damage!!");
								if (skill.SPCost > 0)
								{
									currentPlayer.CurrentSP -= skill.SPCost;
									Console.WriteLine("Your SP decreases by " + skill.SPCost + " SP!!");
								}
								if (skill.HPCost > 0)
								{
									currentPlayer.CurrentHP -= skill.HPCost;
									Console.WriteLine("Your HP decreases by " + skill.HPCost + " HP!!");
								}
							}
						}
					}

					if (currentMonsterFighting != null)
					{
						startedFight = true;
						currentMonsterFighting.CurrentHP -= playerDamage;
						if (currentMonsterFighting.CurrentHP <= 0)
						{
							Console.WriteLine("You felled the " + currentMonsterFighting.Name + "!");
							//Get loot
							if (currentMonsterFighting.Loot.Count >= 1)
							{
								for (int i = 0; i < currentMonsterFighting.Loot.Count; i++)
								{
									bool getItem = RandomNumberGenerator.GetDrop(currentMonsterFighting.PercentLoot[i]);
									if (getItem)
									{
										Console.WriteLine("The " + currentMonsterFighting.Name + " dropped " + currentMonsterFighting.Loot[i].Name + ".");
										currentLocation.ItemsHere.Add(currentMonsterFighting.Loot[i]);
									}
								}
							}
							if (currentMonsterFighting.GoldReward != 0)
							{
								currentPlayer.Gold += currentMonsterFighting.GoldReward;
								Console.WriteLine("You got " + currentMonsterFighting.GoldReward + " gold.");
							}
							if (currentMonsterFighting.ExperiencePointsReward != 0)
							{
								int XPToAdd = currentMonsterFighting.ExperiencePointsReward;
								if (currentMonsterFighting.Level > currentPlayer.Level) //if monster's level is higher than player's level, give player more XP for defeating it.
								{
									XPToAdd = (int)(currentMonsterFighting.ExperiencePointsReward * (1 + (0.1 * (currentMonsterFighting.Level - currentPlayer.Level))));
								}
								Console.WriteLine("You got " + XPToAdd + " XP.");
								currentPlayer.ExperiencePoints += XPToAdd;
							}
							GameMessageFactory.CheckDefeatedMonsterMessage(currentMonsterFighting); //check to see if there's a game message about this particular monster
							currentLocation.MonstersHere.Remove(currentMonsterFighting);
							currentMonsterFighting = null;
							state = States.Peaceful; //setting the state back to peaceful
							startedFight = false; //reset value of startedFight
						}
					}
					
				}
			}
		}
	}
}
