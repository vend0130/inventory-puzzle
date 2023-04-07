using System;
using UnityEngine;

namespace Code.Property.Readonly
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}