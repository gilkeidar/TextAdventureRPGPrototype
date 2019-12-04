using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Items;
using RPG.Blueprints.Characters;
using RPG.Factories;

namespace RPG.Blueprints.Locations
{
	public class Location
	{
		string name;
		string description;
		int id;
		int x;
		int y;
		List<Item> itemsHere = new List<Item>();
		List<Monster> monstersHere = new List<Monster>();
		List<string> directions = new List<string>();

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}
		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		public int X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}
		public int Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}
		public List<Item> ItemsHere
		{
			get
			{
				return this.itemsHere;
			}
			set
			{
				this.itemsHere = value;
			}
		}
		public List<Monster> MonstersHere
		{
			get
			{
				return this.monstersHere;
			}
			set
			{
				this.monstersHere = value;
			}
		}
		public List<string> Directions
		{
			get
			{
				return this.directions;
			}
			set
			{
				this.directions = value;
			}
		} 

		public Location(string name, string description, int id, int x, int y)
		{
			this.Name = name;
			this.Description = description;
			this.ID = id;
			this.X = x;
			this.Y = y;
		}

		public void FindDirections()
		{
			if(Map.GetLocation(this.X, this.Y - 1) != null)
			{
				Directions.Add("north");
			}
			if (Map.GetLocation(this.X, this.Y + 1) != null)
			{
				Directions.Add("south");
			}
			if (Map.GetLocation(this.X  + 1, this.Y) != null)
			{
				Directions.Add("east");
			}
			if (Map.GetLocation(this.X - 1, this.Y) != null)
			{
				Directions.Add("west");
			}
		}
	}
}
