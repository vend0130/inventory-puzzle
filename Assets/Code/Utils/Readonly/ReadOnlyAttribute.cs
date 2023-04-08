using System;
using UnityEngine;

namespace Code.Utils.Readonly
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}