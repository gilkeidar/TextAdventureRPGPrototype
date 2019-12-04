using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Factories;
using RPG.Story;

namespace RPG
{
	class Program
	{
		static void Main(string[] args)
		{
			SkillFactory.CreateSkills();
			ItemFactory.CreateItems();
			MonsterFactory.CreateMonsters();
			Map.CreateMap();
			GameMessageFactory.CreateMessages();
			Game game = new Game();
			game.GameLoop();
		}
	}
}
