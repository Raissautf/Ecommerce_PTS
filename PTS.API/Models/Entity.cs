using System;

namespace DBR.API.Models
{
	public class Entity
	{
		public Guid Id { get; set; }

		public Entity()
		{
			Id = Guid.NewGuid();
		}
	}
}
