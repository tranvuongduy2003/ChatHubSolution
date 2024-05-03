using ChatHubSolution.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChatHubSolution.Data.Entities
{
    public class BaseEntity<TKey> : ICassandraModel<TKey> where TKey : notnull
    {
        [Key]
        public TKey Id { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
