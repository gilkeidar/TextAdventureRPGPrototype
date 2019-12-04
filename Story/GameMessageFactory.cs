using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Mechanics.GameMessages;
using RPG.Factories;
using RPG.Blueprints.Locations;
using RPG.Blueprints.Items;
using RPG.Blueprints.Characters;
using RPG.Tools;

namespace RPG.Story
{
	public static class GameMessageFactory
	{
		static List<GameMessage> gameMessages = new List<GameMessage>();
		//static List<string> messageScenes = new List<string>(); //the string that has the same index as a gameMessage whose condition was met will be printed

		public static List<GameMessage> GameMessages
		{
			get
			{
				return gameMessages;
			}
			set
			{
				gameMessages = value;
			}
		}

		public static void CreateMessages()
		{
			//create all game messages in the game.
			gameMessages.Add(new NewLocationMessage("Congratulations! You've found the Second Room. Moving in this game is as easy as saying 'north' or 'south'! " +
				"If you know where you're going, that is.", Map.GetLocation(0,1)));
			gameMessages.Add(new NewItemMessage("Ah! I see you just picked up a Short Sword. " +
				"A neat weapon, truly. Since it's one handed, you can wield another weapon at the same time! " +
	"Marvelous, isn't it?", ItemFactory.GetItem(0)));
			gameMessages.Add(new NewMonsterMessage("That, is the fearsome beast of the Kobolds - the Boss Kobold. " +
				"Tread carefully, for it is a Level 5 monster - you heard me! Level 5! " +
	"Not that there aren't STRONGER monsters, but, make no mistake, it's very dangerous if you don't have the right armor. " +
 "However, it's only saving grace is that it is very slow - if you have a throwing weapon of some kind, you may be able to surprise it!", MonsterFactory.GetMonster(1)));
			gameMessages.Add(new DefeatedMonsterMessage("Well done! You've just defeated the Boss Kobold - a strong monster, to be sure. " +
				"You're well on your way to becoming the warrior that this land has long waited for.", MonsterFactory.GetMonster(1)));
			//create messageScenes (like text cut scenes)
			//messageScenes.Add();
		}


		//Each type of event has one type of condition:
		//newLocationMessage: gets called if player enters the newLocation for the first time <-- to deal with this "first time" stuff just set the condition to true and don't check events whose conditions are true. (i.e. only check messages whose conditions are false)
		//newItemMessage: gets called if player gets an item 
		//newMonsterMessage: gets called if player sees a monster for the first time.

		//if gameMessage occurs (i.e. condition was met and message was printed), delete it and its message printout from the gameMessages and messageScenes list to make loops faster.

		public static void CheckLocationMessage(Location currentLocation) //run when entering a new location
		{
			for(int i = 0; i < gameMessages.Count; i++)
			{
				if(gameMessages[i].GetClassType() == "NewLocationMessage")
				{
					if (!gameMessages[i].Condition)
					{
						NewLocationMessage currentMessage = (NewLocationMessage)gameMessages[i];
						if(currentMessage.NewLocation == currentLocation)
						{
							PrintMessage(currentMessage);
							currentMessage.Condition = true;
							break;
						}
					}
				}
			}
		}

		public static void CheckItemMessage(Item itemChecked) //run when getting/equipping a new item
		{
			for(int i = 0; i < gameMessages.Count; i++)
			{
				if(gameMessages[i].GetClassType() == "NewItemMessage")
				{
					if (!gameMessages[i].Condition)
					{
						NewItemMessage currentMessage = (NewItemMessage)gameMessages[i];
						if(currentMessage.NewItem == itemChecked)
						{
							PrintMessage(currentMessage);
							currentMessage.Condition = true;
							break;
						}
					}
				}
			}
		}

		public static void CheckMonsterMessage(Monster monster)
		{
			for(int i = 0; i < gameMessages.Count; i++)
			{
				if (gameMessages[i].GetClassType() == "NewMonsterMessage")
				{
					if (!gameMessages[i].Condition)
					{
						NewMonsterMessage currentMessage = (NewMonsterMessage)gameMessages[i];
						if(currentMessage.NewMonster == monster)
						{
							PrintMessage(currentMessage);
							currentMessage.Condition = true;
							break;
						}
					}
				}
			}
		}

		public static void CheckDefeatedMonsterMessage(Monster monster)
		{
			for (int i = 0; i < gameMessages.Count; i++)
			{
				if (gameMessages[i].GetClassType() == "DefeatedMonsterMessage")
				{
					if (!gameMessages[i].Condition)
					{
						DefeatedMonsterMessage currentMessage = (DefeatedMonsterMessage)gameMessages[i];
						if (currentMessage.DefeatedMonster == monster)
						{
							PrintMessage(currentMessage);
							currentMessage.Condition = true;
							break;
						}
					}
				}
			}
		}

		public static void PrintMessage(GameMessage currentMessage)
		{
			//change state to GameMessage
			ColorPrintouts.StatePrint(currentMessage.Message, States.GameMessage);
			if(currentMessage.GetClassType() == "NewLocationMessage" || currentMessage.GetClassType() == "NewMonsterMessage" || currentMessage.GetClassType() == "DefeatedMonsterMessage")
			{
				Console.WriteLine();
			}
			//change/return state to original state
		}

	}
}
