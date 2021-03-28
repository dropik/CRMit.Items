using System.ComponentModel.DataAnnotations;

namespace CRMit.Items.Models
{
    /// <summary>
    /// Item input data.
    /// </summary>
    public class ItemInput
    {
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
        public decimal Price { get; set; }
    }
}
