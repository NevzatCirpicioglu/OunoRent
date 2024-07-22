namespace EntityLayer.Entities;

public class Product
{
    public int Id { get; set; }

    public string Brand { get; set; }

    public string Model { get; set; }

    public int Price { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }

}