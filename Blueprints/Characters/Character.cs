using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Blueprints.Characters
{
	public class Character
	{
		string name;

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

		public Character(string name)
		{
			this.Name = name;
		}
	}
}
