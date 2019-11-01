using System;

namespace LikeBusLogistic.DAL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class IgnoreAttribute : Attribute
    {
        public bool WhenInsert { get; set; }
        public bool WhenUpdate { get; set; }

        public IgnoreAttribute(bool whenInsert = true, bool whenUpdate = false)
        {
            WhenInsert = whenInsert;
            WhenUpdate = whenUpdate;
        }
    }
}
