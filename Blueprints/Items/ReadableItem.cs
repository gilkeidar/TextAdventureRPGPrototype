using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Blueprints.Items
{
	public class ReadableItem : Item
	{
		string message; //text in the readable item (such as a scroll, a sign, etc.) that will be printed when the player says > read [itemName]

		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}
		
		public ReadableItem(string name, string description, int id, float weight, bool canBeTaken, string message) : base(name, description, id, weight, canBeTaken)
		{
			this.Message = message;
		}

		public override string GetClassType()
		{
			return "ReadableItem";
		}
	}
}
