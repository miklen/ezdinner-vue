using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.Shared
{
    public class Tag : IEquatable<Tag>
    {
        public string Value { get; }
        public string Color { get; }

        public Tag(string value, string color)
        {
            Value = value;
            Color = color;
        }


        public bool Equals(Tag? other)
        {
            return Value == other?.Value && Color == other?.Color;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tag);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Color);
        }

        public override string ToString()
        {
            return $"Tag Value: {Value}, Color: {Color}";
        }
    }
}
