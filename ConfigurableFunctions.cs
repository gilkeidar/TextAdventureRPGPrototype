using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
	public static class ConfigurableFunctions //this class houses all the functions that have some mathematical effect e.g. amount of EXP needed for each level, amount HP increases for every level up, etc.
	{
		public static int CalculateEXPNeeded(int level)
		{
			return level * 100;
		}
		public static int CalculateMaxHP(int level)
		{
			return (int)((100) * Math.Pow(1.2, level - 1)); //100 being the initial HP value of the player
		}

		public static int CalculateMaxSP(int level)
		{
			while (true) //run until the if block runs
			{
				if (level % 5 == 0)
				{
					int amtToAdd = (int)(level / 2.5f); //every 5 levels, player gets +2 SP
					return 10 + amtToAdd;
				}
				else
				{
					level--;
				}
			}
		}
	}
}
