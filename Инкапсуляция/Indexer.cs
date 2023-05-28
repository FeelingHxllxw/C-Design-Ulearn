using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
	public class Indexer
    {
        private readonly double[] array;
        private readonly int start;
        private readonly int len;
        public int Length { get { return len; } }
        public Indexer(double[] array, int start, int length)
        {
            if (length > 0 && start > 0 && start + length <= array.Length)
            {
                this.array = array;
                this.start = start;
                this.Length = length;
            }
            throw new ArgumentException();
        }
        public double this[int index]
        {
            get
            {
                if (index >= 0 && index + start <= len && index + start <= array.Length - 1)
                    return array[index + start];
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < 0 || index + start> Length)
                    throw new IndexOutOfRangeException();
                else
                {
                    array[index + start] = value;
                }
            }
        }
    }
}
