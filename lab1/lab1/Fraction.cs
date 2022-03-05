using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class Fraction:IEquatable<Fraction>,IComparable<Fraction>
    {
        private int _numerator;
        private int _denominator;

        public int denominator { get => _denominator; }
        public int numerator { get => _numerator; }
        

        /// <summary>
        /// Creates a fraction initialized with zeros.
        /// </summary>
        public Fraction() { }

 
        /// <summary>
        /// Creates a fraction initialized with the given numerator and denominator
        /// </summary>
        /// <param name="d">The numerator</param>
        /// <param name="r">The denominator</param>
        public Fraction(int d, int r)
        {
            _numerator = d;
            _denominator = r;
        }

        /// <summary>
        /// Creates a copy of the passed fraction.
        /// </summary>
        /// <param name="f">The fraction to copy</param>
        public Fraction(Fraction f)
        {
            _numerator = f._numerator;
            _denominator = f._denominator;
        }

        /// <summary>
        /// Geneartes a string representation of the fraction.
        /// </summary>
        /// <returns>A string representation of the fraction in the form NNN/DDD. For example: 1/3</returns>
        public override string ToString()
        {
            return _numerator.ToString() + "/" + _denominator.ToString();
        }

        /// <summary>
        /// Adds two fractions together. The resulting fraction will be improper.
        /// </summary>
        /// <param name="a">The first fraction to add</param>
        /// <param name="b">The second fraction to add</param>
        /// <returns>The result of the mathematical operation.</returns>
        public static Fraction operator +(Fraction a, Fraction b)
           => new Fraction(a._numerator * b._denominator + b._numerator * a._denominator, a._denominator * b._denominator);

        /// <summary>
        /// Subtracts two fractions from each other. The resulting fraction will be improper.
        /// </summary>
        /// <param name="a">The first fraction to subtract from</param>
        /// <param name="b">The second fraction being subtracted</param>
        /// <returns>The result of the mathematical operation.</returns>
        public static Fraction operator -(Fraction a, Fraction b)
           => a + (-b);


        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Fraction operator +(Fraction a) => a;

        /// <summary>
        /// Negates the fraction by changing the sign of it's numerator.
        /// </summary>
        /// <param name="a">The fraction to negate.</param>
        /// <returns>The negated fraction</returns>
        public static Fraction operator -(Fraction a) => new Fraction(-a._numerator, a._denominator);

        /// <summary>
        /// Multiplies two fractions
        /// </summary>
        /// <param name="a">The first fraction to multiply</param>
        /// <param name="b">The second fraction to multiply</param>
        /// <returns>The result of the mathematical operation.</returns>
        public static Fraction operator *(Fraction a, Fraction b)
            => new Fraction(a._numerator * b._numerator, a._denominator * b._denominator);

        /// <summary>
        /// Divides two fractions.
        /// </summary>
        /// <param name="a">The first fraction to divide</param>
        /// <param name="b">The second fraction to divide by</param>
        /// <returns>The result of the mathematical operation.</returns>
        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new Fraction(a._numerator * b._denominator, a._denominator * b._numerator);
        }

        /// <summary>
        /// CHecks whether the other fraction is equal to this one.
        /// Does not account for improper fractions.
        /// </summary>
        /// <param name="other">The other fraction to check</param>
        /// <returns>True or false</returns>
        public bool Equals(Fraction other)
        {
            return _denominator == other._denominator && _numerator == other._numerator;
        }


        public int CompareTo(Fraction other)
        {
            return (this - other).numerator;
        }

        /// <summary>
        /// Rounds the fraction to a lower int.
        /// </summary>
        /// <returns>The rounded fraction.</returns>
        public int ToIntFloor()
        {
            return (int)(_numerator / _denominator);
        }

        /// <summary>
        /// Rounds the fraction to the next int.
        /// </summary>
        /// <returns>The rounded fraction.</returns>
        public int ToIntCeil()
        {
            return (int)Math.Ceiling((double)_numerator / (double)_denominator);
        }
    }
}
