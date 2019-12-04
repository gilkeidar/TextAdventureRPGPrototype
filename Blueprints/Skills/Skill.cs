using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Characters;

namespace RPG.Blueprints.Skills
{
	public abstract class Skill
	{
		string name;
		string description;
		string errorText;
		//string skillUsedText; //prints when skill is used. //Probably better to just put the text in the UseSkill function. Can also access weapon names and other such information to make it more interesting.
		int spCost;
		int hpCost;
		SkillType type;

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
		public int HPCost
		{
			get
			{
				return this.hpCost;
			}
			set
			{
				this.hpCost = value;
			}
		}
		public int SPCost
		{
			get
			{
				return this.spCost;
			}
			set
			{
				this.spCost = value;
			}
		}

		public SkillType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		/*public string SkillUsedText
		{
			get
			{
				return this.skillUsedText;
			}
			set
			{
				this.skillUsedText = value;
			}
		}*/

		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
			set
			{
				this.errorText = value;
			}
		}

		protected Skill(string name, string description, string errorText, int spCost, int hpCost, SkillType type)
		{
			this.Name = name;
			this.Description = description;
			this.ErrorText = errorText;
			this.SPCost = spCost;
			this.HPCost = hpCost;
			this.Type = type;
		}
		public abstract int UseSkill(Player player); //skill code goes here
		public abstract void PrintUseSkill(Player player);


	}
}
