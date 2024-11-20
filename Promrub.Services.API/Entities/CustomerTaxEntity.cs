using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promrub.Services.API.Entities;

[Table("Customer_Tax")]
public class CustomerTaxEntity
{

    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("tax_id")]
    public string TaxId { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
    [Column("tel")]
    public string Tel { get; set; }
    
    [Column("full_address")]
    public string FullAddress { get; set; }
   
    [Column("post_code")]
    public string PostCode { get; set; }
    
    [Column("is_memo")]
    public bool IsMemo { get; set; }

    public List<PaymentTransactionEntity> TransactionEntities { get; set; } = new List<PaymentTransactionEntity>();
}