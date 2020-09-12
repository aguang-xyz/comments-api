using System;

namespace CommentsApi.Entities
{
    public abstract class EntityBase
    {
        // Primary key.
        public Guid Id { get; set; }

        // Created time.
        public DateTime CreatedAt { get; set; }

        // Updated time.
        public DateTime UpdatedAt { get; set; }
    }
}
