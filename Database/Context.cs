using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicEFWPF
{
	public class TestContext : DbContext
	{
		public DbSet<Strategy> Strategies { get; set; }
		public DbSet<Operator> Operators { get; set; }
		public DbSet<OrderState> OrderStates { get; set; }

		public TestContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
			Database.Initialize(true);
		}

	}

	[Table("Strategy")]
	public class Strategy
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	[Table("Operator")]
	public class Operator
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	[Table("OrderState")]
	public class OrderState
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

}