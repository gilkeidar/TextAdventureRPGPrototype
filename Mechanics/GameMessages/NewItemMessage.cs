using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Items;

namespace RPG.Mechanics.GameMessages
{
	class NewItemMessage :  GameMessage
	{
		Item newItem;

		public Item NewItem
		{
			get
			{
				return this.newItem;
			}
			set
			{
				this.newItem = value;
			}
		}
		
		public NewItemMessage(string message, Item newItem) : base(message)
		{
			this.NewItem = newItem;
		}
		public override string GetClassType()
		{
			return "NewItemMessage";
		}
	}
}
