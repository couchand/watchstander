using System;
using System.Collections.Generic;
using System.Linq;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public class AccumulatingMetric : IMetric
	{
		public string Name { get; }

		protected string description;
		protected Rate rate;
		protected string unit;

		public TaggerRepository TaggerRepository;

		public string Description => description;
		public Rate Rate => rate;
		public string Unit => unit;
		public IReadOnlyList<string> TagKeys => TaggerRepository.GetAllTagKeys().ToList().AsReadOnly();

		public AccumulatingMetric (string Name)
		{
			this.Name = Name;
			this.description = null;
			this.rate = Rate.Unknown;
			this.unit = null;
			this.TaggerRepository = new TaggerRepository();
		}

		public virtual void SetDescription (string description)
		{
			this.description = description;
		}

		public virtual void SetRate (Rate rate)
		{
			if (rate == Rate.Unknown)
				throw new Exception ("Must provide a valid rate!");

			this.rate = rate;
		}

		public virtual void SetUnit (string unit)
		{
			this.unit = unit;
		}
	}

	public class SetOnceMetric : AccumulatingMetric
	{
		public SetOnceMetric (string Name) : base (Name) {}

		public override void SetDescription (string description)
		{
			if (this.description != null)
				throw new Exception ("Can only set metric description once!");

			base.SetDescription (description);
		}

		public override void SetRate (Rate rate)
		{
			if (this.rate != Rate.Unknown)
				throw new Exception ("Can only set metric rate once!");
			
			base.SetRate (rate);
		}

		public override void SetUnit (string unit)
		{
			if (this.unit != null)
				throw new Exception ("Can only set metric unit once!");

			base.SetUnit (unit);
		}
	}
}

