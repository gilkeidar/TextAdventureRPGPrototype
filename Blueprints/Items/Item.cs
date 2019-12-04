using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Blueprints.Items
{
	public class Item
	{
		string name;
		string description;
		int id;
		float weight;
		bool canBeTaken;

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

		public float Weight
		{
			get
			{
				return this.weight;
			}
			set
			{
				this.weight = value;
			}
		}

		public bool CanBeTaken
		{
			get
			{
				return this.canBeTaken;
			}
			set
			{
				this.canBeTaken = value;
			}
		}

		public Item(string name, string description, int id, float weight, bool canBeTaken)
		{
			this.Name = name;
			this.Description = description;
			this.ID = id;
			this.Weight = weight;
			this.CanBeTaken = canBeTaken;
		}

		public virtual string GetClassType()
		{
			return "Item";
		}
	}
}
