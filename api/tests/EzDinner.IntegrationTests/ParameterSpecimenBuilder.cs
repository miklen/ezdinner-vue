using AutoFixture;
using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EzDinner.IntegrationTests
{
    /// <summary>
    /// From: https://stackoverflow.com/questions/16819470/autofixture-automoq-supply-a-known-value-for-one-constructor-parameter/16954699#16954699
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ParameterNameSpecimenBuilder<T> : ISpecimenBuilder
    {
        private readonly string _name;
        private readonly T _value;

        public ParameterNameSpecimenBuilder(string name, T value)
        {
            // we don't want a null name but we might want a null value
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            _name = name;
            _value = value;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as ParameterInfo;
            if (pi == null)
            {
                return new NoSpecimen();
            }

            if (pi.ParameterType != typeof(T) ||
                !string.Equals(
                    pi.Name,
                    _name,
                    StringComparison.CurrentCultureIgnoreCase))
            {
                return new NoSpecimen();
            }

            return _value;
        }
    }

    public static class FreezeByNameExtension
    {
        public static void FreezeByName<T>(this IFixture fixture, string name, T value)
        {
            fixture.Customizations.Add(new ParameterNameSpecimenBuilder<T>(name, value));
        }
    }
}
