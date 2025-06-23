using Microsoft.EntityFrameworkCore;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;
using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Infrastructure.Data.Repositories;

public class ProductRepository(ApplicationDBContext context) : BaseRepository<DO.Product>(context), IProductRepository
{
    public override async Task<DO.Product> GetById(Guid id, bool include = false, bool tracking = false)
    {
        var query = BaseQuery(tracking);

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<DO.Product>> FindBy(string name)
    {
        return await FindBy(c => c.Name.Contains(name)).ToListAsync();
    }
}