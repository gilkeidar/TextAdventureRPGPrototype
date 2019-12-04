using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Skills;
using RPG.Blueprints.Skills.PlayerSkills;

namespace RPG.Factories
{
	public static class SkillFactory
	{
		static List<PlayerSkill> playerSkills = new List<PlayerSkill>();
		//add a list for weapon skills (or sword skills) and monster skills

		public static void CreateSkills()
		{
			//create all the skills in the game.

			//Player Skills:
			playerSkills.Add(new SwordFlurry("Sword Flurry", "A blaze of sword strikes descending on one's foes.", "You need to wield a weapon to use this skill!", 5, 0, SkillType.Attack, 1, 1));
			//Weapon Skills:

			//Monster Skills:

		}
		public static PlayerSkill GetPlayerSkill(int id)
		{
			return playerSkills[id];
		}
	}
}
