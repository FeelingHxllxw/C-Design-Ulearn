using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        public static IEnumerable<int> GetDigits(this long number)
        {
            return number.GetDigitsInReverseOrder().Reverse();
        }

        public static IEnumerable<int> GetDigitsInReverseOrder(this long number)
        {
            do
            {
                yield return (int)(number % 10);
                number /= 10;
            }
            while (number > 0);
        }

        public static int SumWithSelector<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            return source.Select(selector).Sum();
        }

        public static int SumWithSelector(this IEnumerable<int> source, Func<int, int> selector)
        {
            return source.Select(selector).Sum();
        }
    }

    public static class ControlDigitAlgo
    {
        private static int CalculateControlDigit(long number, int[] factors)
        {
            int sum = number.GetDigits().SumWithSelector(digit => factors.Next() * digit);
            return (10 - sum % 10) % 10;
        }

        private static int[] GetIsbn10Factors()
        {
            return new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        }

        private static int[] GetLuhnFactors()
        {
            return new[] { 2, 1 }.Repeat(numberOfRepetitions: 100);
        }

        public static int Upc(long number)
        {
            int[] factors = { 3, 1 };
            return CalculateControlDigit(number, factors);
        }

        public static int Isbn10(long number)
        {
            int[] factors = GetIsbn10Factors();
            int sum = number.GetDigits().Take(9).SumWithSelector(digit => factors.Next() * digit);
            int controlDigit = CalculateControlDigit(sum, factors);
            return controlDigit == 0 ? '0' : (char)('0' + (10 - controlDigit));
        }

        public static int Luhn(long number)
        {
            int[] factors = GetLuhnFactors();
            int sum = number.GetDigits().SumWithSelector(digit => factors.Next() * digit);
            return (10 - sum % 10) % 10;
        }

        private static int _index;
        private static int Next(this int[] array)
        {
            int index = array.Length - 1 - _index;
            _index++;
            return array[index];
        }

        private static int[] Repeat(this int[] array, int numberOfRepetitions)
        {
            return Enumerable.Range(0, numberOfRepetitions).SelectMany(_ => array).ToArray();
        }
    }
}