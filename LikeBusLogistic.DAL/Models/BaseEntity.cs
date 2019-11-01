using System;

namespace LikeBusLogistic.DAL.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateModified { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
    }
}
