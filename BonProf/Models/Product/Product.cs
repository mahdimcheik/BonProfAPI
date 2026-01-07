using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BonProf.Models;
using BonProf.Models.Interfaces;

namespace BonProf.Models;

public class Product : BaseModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public Guid CursusId { get; set; }
    public Cursus? Cursus { get; set; }

    public Product() { }

    [SetsRequiredMembers]
    public Product(ProductCreate newProduct)
    {
        Name = newProduct.Name;
        Description = newProduct.Description;
        Price = newProduct.Price;
        CursusId = newProduct.CursusId;
    }
}
