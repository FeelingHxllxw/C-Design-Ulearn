using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public double Numerator;
        public double Denominator;
        public bool IsNan => double.IsNaN(Numerator) || double.IsNaN(Denominator);
        public Rational(double numerator, double denominator = 1)
        {
            Numerator = numerator;
            Denominator = denominator;
            SignChange();
            ReduceFraction();
        }
        public static Rational operator +(Rational first, Rational second)
        {
            Rational result = new Rational(first.Numerator * second.Denominator + second.Numerator * first.Denominator, first.Denominator * second.Denominator);
            return result;
        }

        public static Rational operator -(Rational first, Rational second)
        {
            Rational result = new Rational(first.Numerator * second.Denominator - second.Numerator * first.Denominator, first.Denominator * second.Denominator);
            return result;
        }

        public static Rational operator *(Rational first, Rational second)
        {
            Rational result = new Rational(first.Numerator * second.Numerator, first.Denominator * second.Denominator);
            return result;
        }

        public static Rational operator /(Rational first, Rational second)
        {
            Rational result = new Rational( first.Numerator * second.Denominator, first.Denominator * second.Numerator);
            return result;
        }

        public static explicit operator int(Rational number)
        {
            if (number.Numerator % number.Denominator != 0)
                throw new ArgumentException();
            int result = (int)(number.Numerator / number.Denominator);
            return result;
        }

        public static implicit operator Rational(int i)
        {
            Rational result = new Rational(i);
            return result;
        }

        public static implicit operator double(Rational number)
        {
            if (!number.IsNan)
            {
                double result = number.Numerator / number.Denominator;
                return result;
            }
            else 
            {       
                return double.NaN;
            }
        }

        public void ReduceFraction()
        {
            double new_denom = Math.Abs(Denominator);
            double new_numer = Math.Abs(Numerator);
            while (new_numer % new_denom > 0)
                (new_numer, new_denom) = (new_denom, new_numer % new_denom);
            Numerator /= new_denom;
            Denominator /= new_denom;
        }

        public void SignChange()
        {
            if (Denominator < 0) 
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
        }
    }
}
