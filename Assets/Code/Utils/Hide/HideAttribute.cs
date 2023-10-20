using System;
using UnityEngine;

namespace Code.Utils.Hide
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public class HideAttribute : PropertyAttribute
    {
        public readonly string Flag;

        public HideAttribute(string flag)
        {
            Flag = flag;
        }
    }
}