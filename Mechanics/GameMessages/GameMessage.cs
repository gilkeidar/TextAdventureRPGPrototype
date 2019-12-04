using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Mechanics.GameMessages
{
	public class GameMessage
	{
		//If some event occurs, e.g. some condition (player enters a new room for the first time, player levels up to a certain level, player gets a certain item/sees a certain item, player meets a specific monster, etc.)
		//Then change state to States.Message and print the game message
		//Easiest way to do this is to add a GameMessages list to each location and have them run when the player enters the room for the first time; Can add a LocationsBeenTo list to the player so that it doesn't run more than once
		//Another way to do this is to create a Story class that creates all the GameMessages (like a factory) and also holds all the specific conditions
		//Add a bool to Game.cs called MessageFired --> but how to know if an event occurs without knowing that its condition is met?

		//enums for condition type --> NewLocation, NewItem, NewMonster, etc.
		//object that is related to the event --> e.g. the location of the GameMessage, the item that triggers the message, the monster that triggers the game message, and later perhaps the quest item combination/quest item condition that triggers the message
		//The object probably has to be created via inheritance to change object types

		bool condition; // condition that has to be met to print the game message
		string message;

		public bool Condition
		{
			get
			{
				return this.condition;
			}
			set
			{
				this.condition = value;
			}
		}

		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		public GameMessage(string message)
		{
			this.Message = message;
		}

		public virtual string GetClassType()
		{
			return "GameMessage";
		}

	}
}
