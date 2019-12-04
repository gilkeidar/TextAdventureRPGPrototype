using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Items;
using RPG.Blueprints.Skills;

namespace RPG.Blueprints.Characters
{
	public class Player : Character
	{
		int level;
		int experiencePoints;
		int currentHP;
		int maxHP;
		int currentSP;
		int maxSP;
		int gold;
		int speed = 10;
		int defaultSpeed = 10;
		float itemWeight;
		float itemWeightEffect;
		float itemWeightMultiplier = 0.2f;
		float maxItemWeight;
		int maxSpeed;
		bool canGetItems = true;
		int regenerateValueHP = 5; // amt HP regenerates every non-combat turn
		int regenerateValueSP = 1; // amt SP regenerates every non-combat turn
		//list of items
		//equipped weapon slots (right hand, left hand, pockets (for things like ninja stars or whatever) --> Weapon types
		List<Item> inventory = new List<Item>();
		List<PlayerSkill> playerSkills = new List<PlayerSkill>();

		//three weapon slots
		/*Weapon rightHandWeapon = null;
		Weapon leftHandWeapon = null;
		Weapon pocketWeapon = null;*/
		Weapon[] weapons = new Weapon[3]; //three weapon slot array to replace the previous properties

		//body armor --> may expand later to multiple armor slots
		Armor armor;

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
		public int ExperiencePoints
		{
			get
			{
				return this.experiencePoints;
			}
			set
			{
				this.experiencePoints = value;
				while(this.experiencePoints >= this.EXPToNextLevel)
				{
					this.experiencePoints -= this.EXPToNextLevel;
					this.Level++;
					this.MaxHP = ConfigurableFunctions.CalculateMaxHP(this.level);
					this.CurrentHP = this.MaxHP; //Restoring Player's HP to max
					this.MaxSP = ConfigurableFunctions.CalculateMaxSP(this.level);
					Console.WriteLine("LEVEL UP!! You are now level " + this.Level + ".");
				}
			}
		}
		public int EXPToNextLevel
		{
			get
			{
				return ConfigurableFunctions.CalculateEXPNeeded(this.level);
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
		public int MaxHP
		{
			get
			{
				return this.maxHP;
			}
			set
			{
				this.maxHP = value;
			}
		}
		public int CurrentSP
		{
			get
			{
				return this.currentSP;
			}
			set
			{
				this.currentSP = value;
			}
		}
		public int MaxSP
		{
			get
			{
				return this.maxSP;
			}
			set
			{
				this.maxSP = value;
			}
		}
		public int Gold
		{
			get
			{
				return this.gold;
			}
			set
			{
				this.gold = value;
			}
		}

		public List<Item> Inventory
		{
			get
			{
				return this.inventory;
			}
			set
			{
				this.inventory = value;
				//itemWeight += value.Last().Weight; //will this even work?!! Nope, only works if items are added
			}
		}
		public List<PlayerSkill> PlayerSkills
		{
			get
			{
				return this.playerSkills;
			}
			set
			{
				this.playerSkills = value;
			}
		}
		public Weapon[] Weapons
		{
			get
			{
				return this.weapons;
			}
			set
			{
				this.weapons = value;
			}
		}
		public Armor Armor
		{
			get
			{
				return this.armor;
			}
			set
			{
				this.armor = value;
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
		public float ItemWeight
		{
			get
			{
				return this.itemWeight;
			}
			set
			{
				this.itemWeight = value;
				this.itemWeightEffect = this.itemWeightMultiplier * this.itemWeight;
				this.Speed = (int)(defaultSpeed - (this.itemWeightEffect));
				if(this.Speed < 1)
				{
					this.Speed = 1;
				}
			}
		}

		public float MaxItemWeight
		{
			get
			{
				maxSpeed = defaultSpeed; //plus any weapon/armor/item attributes that may increase speed
				maxItemWeight = (maxSpeed - 1) / this.itemWeightMultiplier;
				return this.maxItemWeight;
			}
		}

		public bool CanGetItems
		{
			get
			{
				maxSpeed = defaultSpeed; //plus any weapon/armor/item attributes that may increase speed
				maxItemWeight = (maxSpeed - 1) / this.itemWeightMultiplier;
				if (this.itemWeight >= maxItemWeight)
				{
					return false;
				}
				else
				{
					return true;
				}
			}	
		}


		public Player(string name, int level, int experiencePoints, int currentHP, int maxHP, int currentSP, int maxSP, int gold) :base(name)
		{
			this.Level = level;
			this.ExperiencePoints = experiencePoints;
			this.CurrentHP = currentHP;
			this.MaxHP = maxHP;
			this.CurrentSP = currentSP;
			this.MaxSP = maxSP;
			this.Gold = gold;
		}

		public int Attack(Weapon attackWeapon)
		{
			return RandomNumberGenerator.GetAttackValue(attackWeapon.Attack, attackWeapon.Accuracy);
		}

		public void RegenerateHP()
		{
			if(this.CurrentHP + regenerateValueHP > this.MaxHP)
			{
				this.CurrentHP = this.MaxHP;
			}
			else
			{
				this.CurrentHP += regenerateValueHP;
			}
		}

		public void RegenerateSP()
		{
			if(this.CurrentSP + regenerateValueSP > this.MaxSP)
			{
				this.CurrentSP = this.MaxSP;
			}
			else
			{
				this.CurrentSP += regenerateValueSP;
			}
		}
	}
}
