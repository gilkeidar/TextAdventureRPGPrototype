using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
	public static class RandomNumberGenerator
	{
		static Random random = new Random();

		public static bool GetDrop(int percentOfDrop)
		{
			int chance = random.Next(0, 100);
			if(chance <= percentOfDrop)
			{
				return true;
			}
			return false;
		}

		public static int GetAttackValue(int attack, int accuracy)
		{
			int chance = random.Next(0, 100);
			int damage;
			//Console.WriteLine("Chance: " + chance);

			if (chance <= accuracy)
			{
				//return attack;
				if (chance > 40)
				{
					damage = attack;
				}
				else if (chance < 1)
				{
					damage = attack * 2;
				}
				else
				{
					//chance is 10 or lower (between 10 and 0)
					float dividend = 1.0f / chance;
					//Console.WriteLine("1/chance = " + dividend);
					damage = (int)(attack * (1 + (dividend)));
				}
			}
			else
			{
				damage = attack / 2;
			}
			//Console.WriteLine("Damage: " + damage);
			return damage;
		}
	}
}
