using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Skills;

namespace RPG.Blueprints.Items
{
	public class Archive : ReadableItem // a tome that when read, if the player's level is high enough, teaches the player a skill. It is consumed if this occurs.
	{
		PlayerSkill skill;
		bool wasRead = false;

		public PlayerSkill Skill
		{
			get
			{
				return this.skill;
			}
			set
			{
				this.skill = value;
			}
		}
		public bool WasRead
		{
			get
			{
				return this.wasRead;
			}
			set
			{
				this.wasRead = value;
			}
		}

		public Archive(string name, string description, int id, float weight, bool canBeTaken, string message, PlayerSkill skill, bool wasRead) : base(name, description, id, weight, canBeTaken, message)
		{
			this.Skill = skill;
			this.WasRead = wasRead;
		}

		public override string GetClassType()
		{
			return "Archive";
		}
	}
}
