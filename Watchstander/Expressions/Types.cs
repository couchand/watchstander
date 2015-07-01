using System;

namespace Watchstander.Expressions
{
	public interface IExpression
	{
		string GetExpression ();
	}

	public abstract class SeriesSet : IExpression
	{
		// addition

		public static SeriesSet operator+(SeriesSet series, NumberSet number)
		{
			return new SumSeriesSet (series, number);
		}

		public static SeriesSet operator+(SeriesSet series, Scalar number)
		{
			return new SumSeriesSet (series, number);
		}

		public static SeriesSet operator+(SeriesSet series, double number)
		{
			return series + new Scalar(number);
		}

		// subtraction

		public static SeriesSet operator-(SeriesSet series, NumberSet number)
		{
			return new DifferenceSeriesSet (series, number);
		}

		public static SeriesSet operator-(SeriesSet series, Scalar number)
		{
			return new DifferenceSeriesSet (series, number);
		}

		public static SeriesSet operator-(SeriesSet series, double number)
		{
			return series - new Scalar(number);
		}

		// multiplication

		public static SeriesSet operator*(SeriesSet series, NumberSet number)
		{
			return new ProductSeriesSet (series, number);
		}

		public static SeriesSet operator*(SeriesSet series, Scalar number)
		{
			return new ProductSeriesSet (series, number);
		}

		public static SeriesSet operator*(SeriesSet series, double number)
		{
			return series * new Scalar(number);
		}

		// division

		public static SeriesSet operator/(SeriesSet series, NumberSet number)
		{
			return new QuotientSeriesSet (series, number);
		}

		public static SeriesSet operator/(SeriesSet series, Scalar number)
		{
			return new QuotientSeriesSet (series, number);
		}

		public static SeriesSet operator/(SeriesSet series, double number)
		{
			return series / new Scalar(number);
		}

		// implementation must provide expression

		public abstract string GetExpression();
	}

	public abstract class NumberSet : IExpression
	{
		// addition

		public static SeriesSet operator+ (NumberSet left, SeriesSet right)
		{
			return right + left;
		}

		public static NumberSet operator+ (NumberSet left, NumberSet right)
		{
			return new SumNumberSet (left, right);
		}

		public static NumberSet operator+ (NumberSet left, Scalar right)
		{
			return new SumNumberSet (left, right);
		}

		public static NumberSet operator+ (NumberSet left, double right)
		{
			return left + new Scalar (right);
		}

		// subtraction

		public static SeriesSet operator- (NumberSet left, SeriesSet right)
		{
			return new SubtractorSeriesSet (right, left);
		}

		public static NumberSet operator- (NumberSet left, NumberSet right)
		{
			return new DifferenceNumberSet (left, right);
		}

		public static NumberSet operator- (NumberSet left, Scalar right)
		{
			return new DifferenceNumberSet (left, right);
		}

		public static NumberSet operator- (NumberSet left, double right)
		{
			return left - new Scalar (right);
		}

		// multiplication

		public static SeriesSet operator* (NumberSet left, SeriesSet right)
		{
			return right * left;
		}

		public static NumberSet operator* (NumberSet left, NumberSet right)
		{
			return new ProductNumberSet (left, right);
		}

		public static NumberSet operator* (NumberSet left, Scalar right)
		{
			return new ProductNumberSet (left, right);
		}

		public static NumberSet operator* (NumberSet left, double right)
		{
			return left * new Scalar (right);
		}

		// division

		public static SeriesSet operator/ (NumberSet left, SeriesSet right)
		{
			return new DivisorSeriesSet (right, left);
		}

		public static NumberSet operator/ (NumberSet left, NumberSet right)
		{
			return new QuotientNumberSet (left, right);
		}

		public static NumberSet operator/ (NumberSet left, Scalar right)
		{
			return new QuotientNumberSet (left, right);
		}

		public static NumberSet operator/ (NumberSet left, double right)
		{
			return left / new Scalar (right);
		}

		// implementation must provide expression

		public abstract string GetExpression();
	}

	public class Scalar : IExpression
	{
		internal double value;

		public Scalar (double value)
		{
			this.value = value;
		}

		public static Scalar operator+ (Scalar left, Scalar right)
		{
			return new Scalar(left.value + right.value);
		}

		public static Scalar operator- (Scalar left, Scalar right)
		{
			return new Scalar(left.value - right.value);
		}

		public static Scalar operator* (Scalar left, Scalar right)
		{
			return new Scalar(left.value * right.value);
		}

		public static Scalar operator/ (Scalar left, Scalar right)
		{
			return new Scalar(left.value / right.value);
		}

		public static Scalar operator+ (Scalar left, double right)
		{
			return new Scalar(left.value + right);
		}

		public static Scalar operator- (Scalar left, double right)
		{
			return new Scalar(left.value - right);
		}

		public static Scalar operator* (Scalar left, double right)
		{
			return new Scalar(left.value * right);
		}

		public static Scalar operator/ (Scalar left, double right)
		{
			return new Scalar(left.value / right);
		}

		public static SeriesSet operator+ (Scalar left, SeriesSet right)
		{
			return right + left;
		}

		public static SeriesSet operator- (Scalar left, SeriesSet right)
		{
			return new SubtractorSeriesSet (right, left);
		}

		public static SeriesSet operator* (Scalar left, SeriesSet right)
		{
			return right * left;
		}

		public static SeriesSet operator/ (Scalar left, SeriesSet right)
		{
			return new DivisorSeriesSet (right, left);
		}

		public static NumberSet operator+ (Scalar left, NumberSet right)
		{
			return right + left;
		}

		public static NumberSet operator- (Scalar left, NumberSet right)
		{
			return new SubtractorNumberSet (right, left);
		}

		public static NumberSet operator* (Scalar left, NumberSet right)
		{
			return right * left;
		}

		public static NumberSet operator/ (Scalar left, NumberSet right)
		{
			return new DivisorNumberSet (right, left);
		}

		public virtual string GetExpression()
		{
			return value.ToString ();
		}
	}

	public class SumSeriesSet : SeriesSet
	{
		private SeriesSet left;
		private IExpression right;

		internal SumSeriesSet(SeriesSet left, IExpression right)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}+{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class SumNumberSet : NumberSet
	{
		private NumberSet left;
		private IExpression right;

		internal SumNumberSet(NumberSet left, IExpression right)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}+{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class DifferenceSeriesSet : SeriesSet
	{
		private SeriesSet left;
		private IExpression right;

		internal DifferenceSeriesSet(SeriesSet left, IExpression right)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}-{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class DifferenceNumberSet : NumberSet
	{
		private NumberSet left;
		private IExpression right;

		internal DifferenceNumberSet(NumberSet left, IExpression right)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}-{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class SubtractorSeriesSet : SeriesSet
	{
		private IExpression left;
		private SeriesSet right;

		internal SubtractorSeriesSet(SeriesSet right, IExpression left)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}-{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class SubtractorNumberSet : NumberSet
	{
		private IExpression left;
		private NumberSet right;

		internal SubtractorNumberSet(NumberSet right, IExpression left)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}-{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class ProductSeriesSet : SeriesSet
	{
		private SeriesSet left;
		private IExpression right;

		internal ProductSeriesSet(SeriesSet left, IExpression right)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}*{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class ProductNumberSet : NumberSet
	{
		private NumberSet left;
		private IExpression right;

		internal ProductNumberSet(NumberSet left, IExpression right)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}*{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class QuotientSeriesSet : SeriesSet
	{
		private SeriesSet left;
		private IExpression right;

		internal QuotientSeriesSet(SeriesSet left, IExpression right)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}/{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class QuotientNumberSet : NumberSet
	{
		private NumberSet left;
		private IExpression right;

		internal QuotientNumberSet(NumberSet left, IExpression right)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}/{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class DivisorSeriesSet : SeriesSet
	{
		private IExpression left;
		private SeriesSet right;

		internal DivisorSeriesSet(SeriesSet right, IExpression left)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}/{1}", left.GetExpression(), right.GetExpression());
		}
	}

	public class DivisorNumberSet : NumberSet
	{
		private IExpression left;
		private NumberSet right;

		internal DivisorNumberSet(NumberSet right, IExpression left)
		{
			this.left = left;
			this.right = right;
		}

		public override string GetExpression ()
		{
			return String.Format ("{0}/{1}", left.GetExpression(), right.GetExpression());
		}
	}
}

