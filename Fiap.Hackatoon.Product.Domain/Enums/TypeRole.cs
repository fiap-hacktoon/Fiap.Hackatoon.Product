using System.ComponentModel;

namespace Fiap.Hackatoon.Product.Domain.Enums;

[Flags]
public enum TypeRole
{   
    [Description("Administrator")]
    Administrator = 1,

    [Description("Cliente")]
    Client = 2,

    [Description("Gerente")]
    Manager = 4,

    [Description("Atendente")]
    Attendant = 8,

    [Description("Cozinha")]
    Kitchen = 16,
}