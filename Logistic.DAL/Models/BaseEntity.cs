using Logistic.DAL.Attributes;
using System;

namespace Logistic.DAL.Models
{
    public abstract class BaseEntity
    {
        [Ignore(true, true)]
        public int Id { get; set; }
        [Ignore(false, true)]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateModified { get; set; } = DateTime.Now;
        [Ignore(true, true)]
        public bool IsDeleted { get; set; }
    }
}
