using NUnit;
using NUnit.Framework;

using System;

using Watchstander.Expressions;

namespace WatchstanderTests.Unit.Expressions
{
	[TestFixture]
	public class ArithmeticTests
	{
		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, -1)]
		[TestCase(100, 100)]
		public void TestScalarAddDouble (double left, double right)
		{
			var leftScalar = new Scalar (left);

			var sumScalar = leftScalar + right;

			var expected = left + right;

			Assert.AreEqual (expected.ToString (), sumScalar.GetExpression ());
		}

		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, -1)]
		[TestCase(100, 100)]
		public void TestScalarAddScalar (double left, double right)
		{
			var leftScalar = new Scalar (left);
			var rightScalar = new Scalar (right);

			var sumScalar = leftScalar + rightScalar;

			var expected = left + right;

			Assert.AreEqual (expected.ToString (), sumScalar.GetExpression ());
		}

		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, -1)]
		[TestCase(100, 100)]
		public void TestScalarSubtractDouble (double left, double right)
		{
			var leftScalar = new Scalar (left);

			var sumScalar = leftScalar - right;

			var expected = left - right;

			Assert.AreEqual (expected.ToString (), sumScalar.GetExpression ());
		}

		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, -1)]
		[TestCase(100, 100)]
		public void TestScalarSubtractScalar (double left, double right)
		{
			var leftScalar = new Scalar (left);
			var rightScalar = new Scalar (right);

			var sumScalar = leftScalar - rightScalar;

			var expected = left - right;

			Assert.AreEqual (expected.ToString (), sumScalar.GetExpression ());
		}

		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, -1)]
		[TestCase(100, 100)]
		public void TestScalarMultiplyDouble (double left, double right)
		{
			var leftScalar = new Scalar (left);

			var sumScalar = leftScalar * right;

			var expected = left * right;

			Assert.AreEqual (expected.ToString (), sumScalar.GetExpression ());
		}

		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, -1)]
		[TestCase(100, 100)]
		public void TestScalarMultiplyScalar (double left, double right)
		{
			var leftScalar = new Scalar (left);
			var rightScalar = new Scalar (right);

			var sumScalar = leftScalar * rightScalar;

			var expected = left * right;

			Assert.AreEqual (expected.ToString (), sumScalar.GetExpression ());
		}

		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, -1)]
		[TestCase(100, 100)]
		public void TestScalarDivideDouble (double left, double right)
		{
			var leftScalar = new Scalar (left);

			var sumScalar = leftScalar / right;

			var expected = left / right;

			Assert.AreEqual (expected.ToString (), sumScalar.GetExpression ());
		}

		[Test]
		[TestCase(0, 0)]
		[TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, -1)]
		[TestCase(100, 100)]
		public void TestScalarDivideScalar (double left, double right)
		{
			var leftScalar = new Scalar (left);
			var rightScalar = new Scalar (right);

			var sumScalar = leftScalar / rightScalar;

			var expected = left / right;

			Assert.AreEqual (expected.ToString (), sumScalar.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestScalarAddNumber (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantNumber (constant);

			var sumSeries = scalar + series;

			var expected = String.Format ("{0}+{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestScalarSubtractNumber (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantNumber (constant);

			var sumSeries = scalar - series;

			var expected = String.Format ("{1}-{0}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestScalarMultiplyNumber (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantNumber (constant);

			var sumSeries = scalar * series;

			var expected = String.Format ("{0}*{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestScalarDivideNumber (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantNumber (constant);

			var sumSeries = scalar / series;

			var expected = String.Format ("{1}/{0}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestScalarAddSeries (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantSeries (constant);

			var sumSeries = scalar + series;

			var expected = String.Format ("constant({0})+{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestScalarSubtractSeries (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantSeries (constant);

			var sumSeries = scalar - series;

			var expected = String.Format ("{1}-constant({0})", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestScalarMultiplySeries (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantSeries (constant);

			var sumSeries = scalar * series;

			var expected = String.Format ("constant({0})*{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestScalarDivideSeries (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantSeries (constant);

			var sumSeries = scalar / series;

			var expected = String.Format ("{1}/constant({0})", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberAddDouble (double constant, double value)
		{
			var number = new ConstantNumber (constant);

			var sumNumber = number + value;

			var expected = String.Format ("{0}+{1}", constant, value);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberAddScalar (double constant, double value)
		{
			var scalar = new Scalar (value);

			var number = new ConstantNumber (constant);

			var sumNumber = number + scalar;

			var expected = String.Format ("{0}+{1}", constant, value);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberAddNumber (double left, double right)
		{
			var leftNumber = new ConstantNumber (left);
			var rightNumber = new ConstantNumber (right);

			var sumNumber = leftNumber + rightNumber;

			var expected = String.Format ("{0}+{1}", left, right);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberAddSeries (double left, double right)
		{
			var leftNumber = new ConstantNumber (left);
			var rightSeries = new ConstantSeries (right);

			var sumSeries = leftNumber + rightSeries;

			var expected = String.Format ("constant({1})+{0}", left, right);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberSubtractDouble (double constant, double value)
		{
			var number = new ConstantNumber (constant);

			var sumNumber = number - value;

			var expected = String.Format ("{0}-{1}", constant, value);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberSubtractScalar (double constant, double value)
		{
			var scalar = new Scalar (value);

			var number = new ConstantNumber (constant);

			var sumNumber = number - scalar;

			var expected = String.Format ("{0}-{1}", constant, value);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberSubtractNumber (double left, double right)
		{
			var leftNumber = new ConstantNumber (left);
			var rightNumber = new ConstantNumber (right);

			var sumNumber = leftNumber - rightNumber;

			var expected = String.Format ("{0}-{1}", left, right);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberSubtractSeries (double left, double right)
		{
			var leftNumber = new ConstantNumber (left);
			var rightSeries = new ConstantSeries (right);

			var sumSeries = leftNumber - rightSeries;

			var expected = String.Format ("{0}-constant({1})", left, right);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberMultiplyDouble (double constant, double value)
		{
			var number = new ConstantNumber (constant);

			var sumNumber = number * value;

			var expected = String.Format ("{0}*{1}", constant, value);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberMultiplyScalar (double constant, double value)
		{
			var scalar = new Scalar (value);

			var number = new ConstantNumber (constant);

			var sumNumber = number * scalar;

			var expected = String.Format ("{0}*{1}", constant, value);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberMultiplyNumber (double left, double right)
		{
			var leftNumber = new ConstantNumber (left);
			var rightNumber = new ConstantNumber (right);

			var sumNumber = leftNumber * rightNumber;

			var expected = String.Format ("{0}*{1}", left, right);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberMultiplySeries (double left, double right)
		{
			var leftNumber = new ConstantNumber (left);
			var rightSeries = new ConstantSeries (right);

			var sumSeries = leftNumber * rightSeries;

			var expected = String.Format ("constant({1})*{0}", left, right);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberDivideDouble (double constant, double value)
		{
			var number = new ConstantNumber (constant);

			var sumNumber = number / value;

			var expected = String.Format ("{0}/{1}", constant, value);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberDivideScalar (double constant, double value)
		{
			var scalar = new Scalar (value);

			var number = new ConstantNumber (constant);

			var sumNumber = number / scalar;

			var expected = String.Format ("{0}/{1}", constant, value);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberDivideNumber (double left, double right)
		{
			var leftNumber = new ConstantNumber (left);
			var rightNumber = new ConstantNumber (right);

			var sumNumber = leftNumber / rightNumber;

			var expected = String.Format ("{0}/{1}", left, right);

			Assert.AreEqual (expected, sumNumber.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestNumberDivideSeries (double left, double right)
		{
			var leftNumber = new ConstantNumber (left);
			var rightSeries = new ConstantSeries (right);

			var sumSeries = leftNumber / rightSeries;

			var expected = String.Format ("{0}/constant({1})", left, right);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesAddDouble (double constant, double value)
		{
			var series = new ConstantSeries (constant);

			var sumSeries = series + value;

			var expected = String.Format ("constant({0})+{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesAddScalar (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantSeries (constant);

			var sumSeries = series + scalar;

			var expected = String.Format ("constant({0})+{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesAddNumber (double left, double right)
		{
			var leftSeries = new ConstantSeries (left);
			var rightNumber = new ConstantNumber (right);

			var sumSeries = leftSeries + rightNumber;

			var expected = String.Format ("constant({0})+{1}", left, right);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesSubtractDouble (double constant, double value)
		{
			var series = new ConstantSeries (constant);

			var sumSeries = series - value;

			var expected = String.Format ("constant({0})-{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesSubtractScalar (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantSeries (constant);

			var sumSeries = series - scalar;

			var expected = String.Format ("constant({0})-{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesSubtractNumber (double left, double right)
		{
			var leftSeries = new ConstantSeries (left);
			var rightNumber = new ConstantNumber (right);

			var sumSeries = leftSeries - rightNumber;

			var expected = String.Format ("constant({0})-{1}", left, right);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesMultiplyDouble (double constant, double value)
		{
			var series = new ConstantSeries (constant);

			var sumSeries = series * value;

			var expected = String.Format ("constant({0})*{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesMultiplyScalar (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantSeries (constant);

			var sumSeries = series * scalar;

			var expected = String.Format ("constant({0})*{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesMultiplyNumber (double left, double right)
		{
			var leftSeries = new ConstantSeries (left);
			var rightNumber = new ConstantNumber (right);

			var sumSeries = leftSeries * rightNumber;

			var expected = String.Format ("constant({0})*{1}", left, right);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesDivideDouble (double constant, double value)
		{
			var series = new ConstantSeries (constant);

			var sumSeries = series / value;

			var expected = String.Format ("constant({0})/{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesDivideScalar (double constant, double value)
		{
			var scalar = new Scalar (value);

			var series = new ConstantSeries (constant);

			var sumSeries = series / scalar;

			var expected = String.Format ("constant({0})/{1}", constant, value);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(100, 10)]
		[TestCase(1, 100)]
		[TestCase(42, 5)]
		public void TestSeriesDivideNumber (double left, double right)
		{
			var leftSeries = new ConstantSeries (left);
			var rightNumber = new ConstantNumber (right);

			var sumSeries = leftSeries / rightNumber;

			var expected = String.Format ("constant({0})/{1}", left, right);

			Assert.AreEqual (expected, sumSeries.GetExpression ());
		}
	}
}

