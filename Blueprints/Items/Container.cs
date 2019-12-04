using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Blueprints.Items
{
	public class Container : Item //an item that can contain other items
	{
		List<Item> items = new List<Item>();
		bool open = false;

		public List<Item> Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
			}
		}

		public bool Open
		{
			get
			{
				return this.open;
			}
			set
			{
				this.open = value;
			}
		}

		public Container(string name, string description, int id, float weight, bool canBeTaken, List<Item> items, bool open) : base(name, description, id, weight, canBeTaken)
		{
			this.Items = items;
			this.Open = open;
		}

		public override string GetClassType()
		{
			return "Container";
		}
	}
}
