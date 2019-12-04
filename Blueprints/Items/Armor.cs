using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Blueprints.Items
{
	public class Armor : Item
	{
		int defense;

		public int Defense
		{
			get
			{
				return this.defense;
			}
			set
			{
				this.defense = value;
			}
		}

		public Armor(string name, string description, int id, float weight, bool canBeTaken, int defense) : base(name, description, id, weight, canBeTaken)
		{
			this.Defense = defense;
		}

		public override string GetClassType()
		{
			return "Armor";
		}
	}
}
