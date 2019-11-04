using LikeBusLogistic.DAL.Attributes;
using System;

namespace LikeBusLogistic.DAL.Models
{
    public abstract class BaseEntity
    {
        [Ignore]
        public int Id { get; set; }
        [Ignore]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateModified { get; set; } = DateTime.Now;
        [Ignore]
        public bool IsDeleted { get; set; }
    }
}
