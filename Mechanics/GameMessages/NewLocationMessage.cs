using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Blueprints.Locations;

namespace RPG.Mechanics.GameMessages
{
	class NewLocationMessage : GameMessage
	{
		Location newLocation;

		public Location NewLocation
		{
			get
			{
				return this.newLocation;
			}
			set
			{
				this.newLocation = value;
			}
		}
		
		public NewLocationMessage(string message, Location newLocation) : base(message)
		{
			this.NewLocation = newLocation;
		}

		public override string GetClassType()
		{
			return "NewLocationMessage";
		}
	}
}
