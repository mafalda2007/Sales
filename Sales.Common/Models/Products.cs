namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Products
    {
        [Key]
        #region Properties
        public int ProductId { get; set; }

        [Required]
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime PublishOn { get; set; }
        #endregion
    }
}
