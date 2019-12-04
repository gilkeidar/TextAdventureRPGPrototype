using RPG.Blueprints.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Blueprints.Items
{
	public class PlayerArchive : Item
	{
		PlayerSkill[] attackSkills;
		PlayerSkill[] defenseSkills;
		PlayerSkill[] regenerateSkills;

		public PlayerSkill[] AttackSkills
		{
			get
			{
				return this.attackSkills;
			}
			set
			{
				this.attackSkills = value;
			}
		}

		public PlayerSkill[] DefenseSkills
		{
			get
			{
				return this.defenseSkills;
			}
			set
			{
				this.defenseSkills = value;
			}
		}

		public PlayerSkill[] RegenerateSkills
		{
			get
			{
				return this.regenerateSkills;
			}
			set
			{
				this.regenerateSkills = value;
			}
		}

		public PlayerArchive(string name, string description, int id, float weight, bool canBeTaken, PlayerSkill[] attackSkills, PlayerSkill[] defenseSkills, PlayerSkill[] regenerateSkills) : base(name, description, id, weight, canBeTaken)
		{
			this.AttackSkills = attackSkills;
			this.DefenseSkills = defenseSkills;
			this.RegenerateSkills = regenerateSkills;
		}

		public override string GetClassType()
		{
			return "PlayerArchive";
		}
	}
}
