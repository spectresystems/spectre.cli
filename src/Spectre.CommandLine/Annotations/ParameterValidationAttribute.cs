using System;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public abstract class ParameterValidationAttribute : Attribute
    {
        public string Message { get; }

        protected ParameterValidationAttribute(string message)
        {
            Message = message;
        }

        public abstract ValidationResult Validate(object value);
    }
}