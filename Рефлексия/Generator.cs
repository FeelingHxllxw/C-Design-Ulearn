using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Reflection.Randomness
{
    public class Generator<T> where T : new()
    {
        private readonly Dictionary<string, Func<Random, object>> _generators;

        public Generator()
        {
            _generators = typeof(T).GetProperties()
                .Where(prop => prop.GetCustomAttributes(typeof(FromDistributionAttribute), true).Length > 0)
                .ToDictionary(prop => prop.Name, prop =>
                {
                    var attribute = (FromDistributionAttribute)prop.GetCustomAttributes(typeof(FromDistributionAttribute), true)[0];
                    var distribution = (IContinuousDistribution)Activator.CreateInstance(attribute.DistributionType, attribute.Arguments);
                    return new Func<Random, object>(rnd => distribution.Generate(rnd));
                });
        }

        public T Generate(Random rnd)
        {
            var instance = new T();
            foreach (var generator in _generators)
            {
                var value = generator.Value(rnd);
                instance.GetType().GetProperty(generator.Key).SetValue(instance, value);
            }
            return instance;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FromDistributionAttribute : Attribute
    {
        public Type DistributionType { get; }
        public object[] Arguments { get; }

        public FromDistributionAttribute(Type distributionType, params object[] arguments)
        {
            if (!typeof(IContinuousDistribution).IsAssignableFrom(distributionType))
            {
                throw new ArgumentException($"Type {distributionType.Name} is not a valid distribution type");
            }
            var constructor = distributionType.GetConstructor(arguments.Select(arg => arg.GetType()).ToArray());
            if (constructor == null)
            {
                throw new ArgumentException($"Constructor for type {distributionType.Name} with specified arguments not found");
            }
            DistributionType = distributionType;
            Arguments = arguments;
        }
    }
}