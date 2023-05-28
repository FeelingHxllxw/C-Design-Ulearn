using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    class Category : IComparable
    {
        public string Product;
        public MessageType MessageType;
        public MessageTopic MessageTopic;

        public Category(string product, MessageType messageType, MessageTopic messageTopic)
        {
            this.Product = product;
            this.MessageType = messageType;
            this.MessageTopic = messageTopic;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Category)) return 1;
            Category category = (Category)obj;
            ValueTuple<string, MessageType, MessageTopic> CatNew = ValueTuple.Create(category.Product,
            category.MessageType, category.MessageTopic);
            ValueTuple<string, MessageType, MessageTopic> cater = ValueTuple.Create(Product, MessageType, MessageTopic);
            return cater.CompareTo(CatNew);
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override string ToString()
        {
            return $"{Product}.{MessageType}.{MessageTopic}";
        }

        public static bool operator <=(Category first, Category second)
        {
            return first.CompareTo(second) <= 0;
        }

        public static bool operator >=(Category first, Category second)
        {
            return first.CompareTo(second) >= 0;
        }

        public static bool operator >(Category first, Category second)
        {
            return first.CompareTo(second) == 1;
        }

        public static bool operator <(Category first, Category second)
        {
            return first.CompareTo(second) == -1;
        }
    }
}
