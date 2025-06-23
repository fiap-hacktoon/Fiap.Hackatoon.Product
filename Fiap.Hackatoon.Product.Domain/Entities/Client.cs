using Fiap.Hackatoon.Product.Domain.Enums;

namespace Fiap.Hackatoon.Product.Domain.Entities;

public class Client : User
{
    public DateTime Birth { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    
    public Client() : base() { }

    public Client(string name, string email, string password) : base()
    {
        Name = name;
        Email = email;
        Password = password;
        TypeRole = TypeRole.Client;

        PrepareToInsert();
    }
}
