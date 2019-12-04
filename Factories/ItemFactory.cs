using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Items;
using RPG.Blueprints.Skills;

namespace RPG.Factories
{
	public static class ItemFactory
	{
		static List<Item> itemList = new List<Item>();
		//Maybe name of archives should be like this: Tome of <<Sword Flurry>>? or Archive of the Sword Flurry? or Sword Flurry Archive (and for really high level skills be a cool title like "The Archive of the Flurried Sword"? or "The Archive of the Valiant Sword"?
		public static void CreateItems()
		{
			itemList.Add(new Weapon("Short Sword", "This is a short sword.", 0, 2.0f, true, 1, 10, 85));
			itemList.Add(new Weapon("Rusty Sword", "This is a rusty sword.", 1, 2.5f,  true , 1, 5, 70));
			itemList.Add(new Weapon("Giant Sword", "This is a very large sword.", 2, 4.0f, true, 2, 15, 80));
			itemList.Add(new Weapon("Ninja Star", "This is a ninja star.", 3, 0.5f, true, 3, 5, 90));
			itemList.Add(new Armor("Chainmail", "This is armor made from chainmail.", 4, 6.0f, true, 10));
			itemList.Add(new HealingPotion("A potion that returns some of your health to you.", 5, 0.5f, true, 10));
			itemList.Add(new ReadableItem("Sign on the Road", "A wooden sign firmly implanted in the ground", 6, 10.0f, false, "Welcome to the game!"));
			itemList.Add(new Weapon("Powerful Sword", "A sword that's more powerful than other swords. Look at the name for reference.", 7, 2.0f, true, 1, 20, 90));
			itemList.Add(new Item("Dragon Tooth", "Legend tells that a mighty white dragon will fly you to the Dragon's Desolation if you show them this tooth...", 8, 0.5f, true));
			itemList.Add(new Container("Leather Bag", "You know, like the one from Forbidden Castle.", 9, 1.0f, true, new List<Item> { GetItem(8) }, false));
			itemList.Add(new Archive("Archive of the Sword Flurry", "An old volume that describes the sword fighting form of the valiant.", 10, 1.5f, true, "Read this, and the power of the flurried blade shall be yours...", SkillFactory.GetPlayerSkill(0), false));
		}
		public static Item GetItem(int id)
		{
			if(id >= 0 && id < itemList.Count)
			{
				return itemList[id];
			}
			return null;
		}
	}
}
