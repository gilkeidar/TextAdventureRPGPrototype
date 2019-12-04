using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Blueprints.Skills
{
	public abstract class PlayerSkill : Skill
	{
		int level; //level of skill? Through use this level increases (via probability)
		int levelToUse; //minimum level player needs to be to use this skill

		public int Level
		{
			get
			{
				return this.level;
			}
			set
			{
				this.level = value;
			}
		}
		public int LevelToUse
		{
			get
			{
				return this.levelToUse;
			}
			set
			{
				this.levelToUse = value;
			}
		}
		public PlayerSkill(string name, string description, string errorText, int spCost, int hpCost, SkillType type, int level, int levelToUse) : base(name, description, errorText, spCost, hpCost, type)
		{
			this.Level = level;
			this.LevelToUse = levelToUse;
		}
	}
}
