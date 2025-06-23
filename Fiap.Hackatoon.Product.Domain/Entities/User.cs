using Fiap.Hackatoon.Product.Domain.Enums;

namespace Fiap.Hackatoon.Product.Domain.Entities;

public class User : BaseEntity
{
    public TypeRole TypeRole { get;set;}
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public User() : base() { }

    public User(Guid id, string name, string email, TypeRole typeRole) : base()
    {
        Id = id;
        Name = name;
        Email = email;
        TypeRole = typeRole;
    }

    public void SetLevelPermission(TypeRole typeRole)
    {
        TypeRole = typeRole;
    }
}