using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Characters;

namespace RPG.Mechanics.GameMessages
{
	class NewMonsterMessage : GameMessage
	{
		Monster newMonster;

		public Monster NewMonster
		{
			get
			{
				return this.newMonster;
			}
			set
			{
				this.newMonster = value;
			}
		}
		
		public NewMonsterMessage(string message, Monster newMonster) : base(message)
		{
			this.NewMonster = newMonster;
		}

		public override string GetClassType()
		{
			return "NewMonsterMessage";
		}
	}
}
