using Fiap.Hackatoon.Product.Domain.Enums;

namespace Fiap.Hackatoon.Product.Domain.Entities;

public class Employee : User
{
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public Employee() : base() { }

    public Employee(string name, string email, string password, int ddd, string phoneNumber) : base()
    {
        Name = name;
        Email = email;
        Password = password;

        PrepareToInsert();
    }
}
