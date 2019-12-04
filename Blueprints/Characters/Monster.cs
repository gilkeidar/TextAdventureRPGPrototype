using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Items;

namespace RPG.Blueprints.Characters
{
	public class Monster : Character
	{
		int id;
		int currentHP;
		int maximumHP;
		string description;
		List<Item> loot = new List<Item>(); //loot items
		List<int> percentLoot = new List<int>(); //same length as loot list, contains int percentages for each drop that correspond to the items in the loot list with the same index
		int experiencePointsReward;
		int goldReward;
		int attack;
		int accuracy;
		int speed;
		int level;
		bool willAttack;

		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		public int CurrentHP
		{
			get
			{
				return this.currentHP;
			}
			set
			{
				this.currentHP = value;
			}
		}
		public int MaximumHP
		{
			get
			{
				return this.maximumHP;
			}
			set
			{
				this.maximumHP = value;
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
		public List<Item> Loot
		{
			get
			{
				return this.loot;
			}
			set
			{
				this.loot = value;
			}
		}
		public List<int> PercentLoot
		{
			get
			{
				return this.percentLoot;
			}
			set
			{
				this.percentLoot = value;
			}
		}
		public int ExperiencePointsReward
		{
			get
			{
				return this.experiencePointsReward;
			}
			set
			{
				this.experiencePointsReward = value;
			}
		}
		public int GoldReward
		{
			get
			{
				return this.goldReward;
			}
			set
			{
				this.goldReward = value;
			}
		}
		public int Attack
		{
			get
			{
				return this.attack;
			}
			set
			{
				this.attack = value;
			}
		}
		public int Accuracy
		{
			get
			{
				return this.accuracy;
			}
			set
			{
				this.accuracy = value;
			}
		}
		public int Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

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

		public bool WillAttack
		{
			get
			{
				return this.willAttack;
			}
			set
			{
				this.willAttack = value;
			}
		}

		public Monster(string name, int id, int level, int currentHP, int maximumHP, string description, List<Item> loot, List<int> percentLoot, int experiencePointsReward, int goldReward, int attack, int accuracy, int speed, bool willAttack) : base(name)
		{
			this.ID = id;
			this.Level = level;
			this.CurrentHP = currentHP;
			this.MaximumHP = maximumHP;
			this.Description = description;
			this.Loot = loot;
			this.PercentLoot = percentLoot;
			this.ExperiencePointsReward = experiencePointsReward;
			this.GoldReward = goldReward;
			this.Attack = attack;
			this.Accuracy = accuracy;
			this.Speed = speed;
			this.WillAttack = willAttack;
		}

		public int DoDamage()
		{
			return RandomNumberGenerator.GetAttackValue(this.Attack, this.Accuracy);
		}
	}
}
