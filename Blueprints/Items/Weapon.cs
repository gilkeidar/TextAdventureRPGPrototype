using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Blueprints.Items
{
	public class Weapon : Item
	{
		//Weapon Types:
		//1 - single hand
		//2 - two hands --> if wielding two-handed weapon, can't use single hand nor pocket weapon unless it's sheathed.
		//3 - small (stored in pockets)
		int weaponType;
		int attack;
		int accuracy;

		public int WeaponType
		{
			get
			{
				return this.weaponType;
			}
			set
			{
				this.weaponType = value;
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
		public Weapon(string name, string description, int id, float weight, bool canBeTaken, int weaponType, int attack, int accuracy) : base(name, description, id, weight, canBeTaken)
		{
			this.WeaponType = weaponType;
			this.Attack = attack;
			this.Accuracy = accuracy;
		}

		public override string GetClassType()
		{
			return "Weapon";
		}
	}
}
