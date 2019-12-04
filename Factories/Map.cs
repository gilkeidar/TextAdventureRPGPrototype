using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Locations;

namespace RPG.Factories
{
	public static class Map
	{
		static Location[][] gameMap;

		//Method that creates locations and instantiates the world map
		public static void CreateMap()
		{
			//create Locations
			Location firstRoom = new Location("First Room", "This is the first room in the game!", 1, 0, 0);
			Location secondRoom = new Location("Second Room", "This is the second room in the game!", 2, 0, 1);
			Location thirdRoom = new Location("Third Room", "This is the third room in the game!", 3, 1, 1);
			
			//create gameMap
			gameMap = new Location[2][];
			gameMap[0] = new Location[1];
			gameMap[1] = new Location[2];

			//place locations in map based on index positions
			gameMap[0][0] = firstRoom;
			gameMap[1][0] = secondRoom;
			gameMap[1][1] = thirdRoom;

			//define location's possible directions:
			firstRoom.FindDirections();
			secondRoom.FindDirections();
			thirdRoom.FindDirections();

			//add items to rooms:
			firstRoom.ItemsHere.Add(ItemFactory.GetItem(0)); //adding a short sword to the first room.
			//firstRoom.ItemsHere.Add(ItemFactory.GetItem(0));
			firstRoom.ItemsHere.Add(ItemFactory.GetItem(1));
			firstRoom.ItemsHere.Add(ItemFactory.GetItem(2));
			firstRoom.ItemsHere.Add(ItemFactory.GetItem(3));
			firstRoom.ItemsHere.Add(ItemFactory.GetItem(5));
			firstRoom.ItemsHere.Add(ItemFactory.GetItem(6));
			secondRoom.ItemsHere.Add(ItemFactory.GetItem(4));
			secondRoom.ItemsHere.Add(ItemFactory.GetItem(10));
			thirdRoom.ItemsHere.Add(ItemFactory.GetItem(7));
			thirdRoom.ItemsHere.Add(ItemFactory.GetItem(9));
			//Console.WriteLine("First Room ItemsHere Count: " + firstRoom.ItemsHere.Count);
			//Console.WriteLine("First Room item: " + firstRoom.ItemsHere[0].Name);

			//add monsters to rooms:
			secondRoom.MonstersHere.Add(MonsterFactory.GetMonster(0));
			secondRoom.MonstersHere.Add(MonsterFactory.GetMonster(1));
			thirdRoom.MonstersHere.Add(MonsterFactory.GetMonster(2));
		}

		public static Location GetLocation(int x, int y)
		{
			if(y >= 0 && y < gameMap.Length && x >= 0 && x < gameMap[y].Length)
			{
				return gameMap[y][x];
			}
			return null;
		}
	}
}
