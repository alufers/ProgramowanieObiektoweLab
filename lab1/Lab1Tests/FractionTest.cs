using Microsoft.VisualStudio.TestTools.UnitTesting;
using lab1;


namespace Lab1Tests
{
    [TestClass]
    public class FractionTest
    {
        [TestMethod]
        public void TestContructors()
        {
            var f1 = new Fraction();
            Assert.AreEqual(f1.numerator, 0, "default value is not zero");
            Assert.AreEqual(f1.denominator, 0, "default value is not zero");

            var f2 = new Fraction(21, 37);
            Assert.AreEqual(f2.numerator, 21, "bad value");
            Assert.AreEqual(f2.denominator, 37, "bad value");

            var f3 = new Fraction(f2);
            Assert.AreEqual(f3.numerator, 21, "bad copied value");
            Assert.AreEqual(f3.denominator, 37, "bad copied value");
        }

        [TestMethod]
        public void TestToString()
        {
            var f = new Fraction(1, 3);
            Assert.AreEqual(f.ToString(), "1/3");
        }

        [TestMethod]
        public void TestPlusOperator()
        {
            var f1 = new Fraction(1, 3);
            var f2 = new Fraction(2, 3);
            Assert.AreEqual((f1 + f2).Equals(new Fraction(9, 9)), true, "bad calculation");
        }

        [TestMethod]
        public void TestMinusOperator()
        {
            var f1 = new Fraction(1, 3);
            var f2 = new Fraction(2, 3);
            Assert.AreEqual((f1 - f2).Equals(new Fraction(-3, 9)), true, "bad calculation");
        }

        [TestMethod]
        public void TestMultiplicationOperator()
        {
            var f1 = new Fraction(1, 3);
            var f2 = new Fraction(2, 3);
            Assert.AreEqual((f1 * f2).Equals(new Fraction(2, 9)), true, "bad calculation");
        }

        [TestMethod]
        public void TestDivisionOperator()
        {
            var f1 = new Fraction(1, 3);
            var f2 = new Fraction(2, 3);
            Assert.AreEqual((f1 / f2).Equals(new Fraction(3, 6)), true, "bad calculation");
        }

        [TestMethod]
        public void TestEquals()
        {
            var f1 = new Fraction(1, 3);
            var f2 = new Fraction(2, 3);
            Assert.IsFalse(f1.Equals(f2));
        }

        [TestMethod]
        public void TestComparable()
        {
            var f1 = new Fraction(1, 3);
            var f2 = new Fraction(2, 3);
            Assert.AreEqual(f1.CompareTo(f2), -3);
        }

        [TestMethod]
        public void TestToIntFloor()
        {
            var f1 = new Fraction(1, 3);
            Assert.AreEqual(f1.ToIntFloor(), 0);
        }

        [TestMethod]
        public void TestToIntCeil()
        {
            var f1 = new Fraction(1, 3);
            Assert.AreEqual(f1.ToIntCeil(), 1);
        }

    }
}
