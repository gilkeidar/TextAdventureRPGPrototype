using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Characters;
using RPG.Blueprints.Items;

namespace RPG.Factories
{
	public static class MonsterFactory
	{
		static List<Monster> monsters = new List<Monster>();

		public static void CreateMonsters()
		{
			//create monsters and put them into the monsters list --> Monsters ID should be equal to their index in the list (so you don't have to loop to find the monster/item/etc.)
			monsters.Add(new Monster("Kobold",0, 1, 10, 10, "A short humanoid creature that wields a dagger.", new List<Item> { ItemFactory.GetItem(1) }, new List<int> { 100 }, 10, 5, 10, 80, 10, false));
			monsters.Add(new Monster("Boss Kobold", 1, 5, 50, 50, "A taller kobold than most, that wields a short sword.", new List<Item> { ItemFactory.GetItem(0) }, new List<int> { 100 }, 100, 50, 20, 80, 5, false));
			monsters.Add(new Monster("Shadow Kobold", 2, 6, 100, 100, "A kobold that's stronger than most, and twice as fast.", new List<Item> { ItemFactory.GetItem(2) }, new List<int> { 100 }, 500, 100, 25, 90, 20, true));
		}

		public static Monster GetMonster(int id)
		{
			if(id >= 0 && id < monsters.Count)
			{
				return monsters[id];
			}
			return null;
		}
	}
}
