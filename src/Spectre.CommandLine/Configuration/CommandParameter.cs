// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Spectre.CommandLine.Configuration
{
    internal abstract class CommandParameter
    {
        public ParameterInfo Parameter { get; }
        public bool IsRequired { get; set; }

        protected CommandParameter(ParameterInfo info, bool required)
        {
            Parameter = info;
            IsRequired = required;
        }

        public void Assign(object settings, object value)
        {
            Parameter.Property.SetValue(settings, value);
        }
    }
}