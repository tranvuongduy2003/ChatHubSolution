using ChatHubSolution.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChatHubSolution.Data.Entities
{
    public abstract class BaseEntity<TKey> : ICassandraModel<TKey> where TKey : notnull
    {
        [Key]
        public TKey Id { get; set; } = default!;

        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
