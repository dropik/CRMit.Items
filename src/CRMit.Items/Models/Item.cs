using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMit.Items.Models
{
    /// <summary>
    /// Represents data of a single item.
    /// </summary>
    public class Item
    {
        public Item() { }

        public Item(ItemInput dto)
        {
            Name = dto.Name;
            Description = dto.Description;
            Price = dto.Price;
        }

        /// <summary>
        /// Item's unique number.
        /// </summary>
        /// <example>123456</example>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        /// <example>Laptop</example>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        /// <example>A brand new laptop.</example>
        public string Description { get; set; }

        /// <summary>
        /// Price.
        /// </summary>
        /// <example>100.0</example>
        [Column(TypeName = "decimal(18,5)")]
        public decimal Price { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Item item &&
                   Id == item.Id &&
                   Name == item.Name &&
                   Description == item.Description &&
                   Price == item.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description, Price);
        }
    }
}
