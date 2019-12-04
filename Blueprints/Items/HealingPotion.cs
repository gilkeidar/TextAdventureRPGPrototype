using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Blueprints.Items
{
	public class HealingPotion : Item
	{
		int amountHeal;
		bool hasBeenUsed;

		public int AmountHeal
		{
			get
			{
				return this.amountHeal;
			}
			set
			{
				this.amountHeal = value;
			}
		}
		public bool HasBeenUsed
		{
			get
			{
				return this.hasBeenUsed;
			}
			set
			{
				this.hasBeenUsed = value;
			}
		}

		public HealingPotion(string description, int id, float weight, bool canBeTaken, int amountHeal) : base("",description, id, weight, canBeTaken)
		{
			this.Name = "Healing Potion +" + amountHeal;
			this.AmountHeal = amountHeal;
			this.HasBeenUsed = false;
		}

		public int Heal(int currentHP, int maxHP)
		{
			if(currentHP + amountHeal >= maxHP)
			{
				return maxHP;
			}
			else
			{
				return currentHP + amountHeal;
			}
		}

		public override string GetClassType()
		{
			return "HealingPotion";
		}
	}
}
