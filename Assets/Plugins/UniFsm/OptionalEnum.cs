using System;

namespace UniFsm
{
    public readonly struct OptionalEnum<TEnum> where TEnum : Enum
    {
        public TEnum Value { get; }
        public bool HasValue { get; }

        private OptionalEnum(TEnum value, bool hasValue)
        {
            Value = value;
            HasValue = hasValue;
        }

        public static implicit operator OptionalEnum<TEnum>(TEnum value) => new OptionalEnum<TEnum>(value, true);
        public static OptionalEnum<TEnum> None => new OptionalEnum<TEnum>(default, false);
        public static OptionalEnum<TEnum> Create(TEnum value) => new OptionalEnum<TEnum>(value, true);
    }
}