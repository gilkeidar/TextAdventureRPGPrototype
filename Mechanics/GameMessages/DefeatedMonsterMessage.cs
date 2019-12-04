using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Characters;

namespace RPG.Mechanics.GameMessages
{
	class DefeatedMonsterMessage : GameMessage
	{
		Monster defeatedMonster;

		public Monster DefeatedMonster
		{
			get
			{
				return this.defeatedMonster;
			}
			set
			{
				this.defeatedMonster = value;
			}
		}
		
		public DefeatedMonsterMessage(string message, Monster defeatedMonster) : base(message)
		{
			this.DefeatedMonster = defeatedMonster;
		}

		public override string GetClassType()
		{
			return "DefeatedMonsterMessage";
		}
	}
}
