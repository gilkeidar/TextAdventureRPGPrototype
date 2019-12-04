using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Characters;
using RPG.Blueprints.Items;

namespace RPG.Blueprints.Skills.PlayerSkills
{
	public class SwordFlurry : PlayerSkill
	{
		//Player skill that lets player attack a monster up to three times with a weapon, or if the player wields two 1h weapons, allows up to 2 times. The amount of hits 
		//increases as the skill level increases.
		//SP cost: 4 SP? (a small-medium amount)
		//HP cost: 0 HP
		int attackTimesTwoHanded = 3;
		int attackTimesOneHanded = 2;

		bool multipleWeapons = false;
		/*string skillUsedTextTwo; //skillUsedText for if player uses two weapons instead of one

		public string SkillUsedTextTwo
		{
			get
			{
				return this.skillUsedTextTwo;
			}
			set
			{
				this.skillUsedTextTwo = value;
			}
		}*/
		public SwordFlurry(string name, string description, string errorText, int spCost, int hpCost, SkillType type, int level, int levelToUse) : base(name, description, errorText, spCost, hpCost, type, level, levelToUse)
		{

		}
		public override int UseSkill(Player player)
		{
			int dmg = 0;
			if (player.Weapons[0] != null && player.Weapons[0].WeaponType == 2)
			{
				for (int i = 0; i < attackTimesTwoHanded; i++)
				{
					dmg += RandomNumberGenerator.GetAttackValue(player.Weapons[0].Attack, player.Weapons[0].Accuracy);
				}
				
			}
			else if (player.Weapons[0] == null)
			{
				if(player.Weapons[1] == null)
				{
					return -1;
				}
				else
				{
					for(int i = 0; i < attackTimesOneHanded; i++)
					{
						dmg += RandomNumberGenerator.GetAttackValue(player.Weapons[1].Attack, player.Weapons[1].Accuracy);
					}
				}
			}
			else if (player.Weapons[1] == null)
			{
				for (int i = 0; i < attackTimesOneHanded; i++)
				{
					dmg += RandomNumberGenerator.GetAttackValue(player.Weapons[0].Attack, player.Weapons[0].Accuracy);
				}
			}
			else
			{
				for (int i = 0; i < attackTimesOneHanded; i++)
				{
					dmg += RandomNumberGenerator.GetAttackValue(player.Weapons[0].Attack, player.Weapons[0].Accuracy);
					dmg += RandomNumberGenerator.GetAttackValue(player.Weapons[1].Attack, player.Weapons[1].Accuracy);
				}
				multipleWeapons = true;
				
			}
			return dmg;
		}

		public override void PrintUseSkill(Player player)
		{
			if (multipleWeapons)
			{
				Console.WriteLine("Raising your " + player.Weapons[0].Name + " and " + player.Weapons[1].Name + ", you charge at your foe and blaze a fury of sword strikes.");
			}
			else
			{
				string weaponName = "";
				if (player.Weapons[0] == null)
				{
					weaponName = player.Weapons[1].Name;
				}
				else
				{
					weaponName = player.Weapons[0].Name;
				}
				Console.WriteLine("Raising your " + weaponName + ", you charge at your foe and blaze a fury of sword strikes.");
			}
		}
	}
}
