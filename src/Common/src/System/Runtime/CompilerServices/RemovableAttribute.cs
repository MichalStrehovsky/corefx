// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace System.Runtime.CompilerServices
{
    // Instructs IL linkers that the method body decorated with this attribute can be removed.
    // The return value of the method with removed body is replaced with the default value
    // that corresponds to the type (0 for int, null for reference types, etc.).
    [AttributeUsage(AttributeTargets.Method)]
    internal class RemovableAttribute : Attribute
    {
        public string FeatureSwitchName { get; }

        public RemovableAttribute(string featureSwitchName)
        {
            FeatureSwitchName = featureSwitchName;
        }
    }
}
