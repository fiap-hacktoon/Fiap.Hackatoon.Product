using Microsoft.EntityFrameworkCore;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using VIEW = Fiap.Hackatoon.Product.Domain.Views.ElasticSearch;

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

    public async Task<List<VIEW.ProductByType>> GetByType(string nameOrCode)
    {
        return (await FindBy(c => c.Type.Name.Contains(nameOrCode) || c.Type.Code.Contains(nameOrCode))
            .Select(c => new VIEW.ProductByType
            {
                Id = c.Id,
                ProductName = c.Name,
                TypeId = c.Type.Id,
                TypeCode = c.Type.Code,
                TypeName = c.Type.Name,
                TypeDescription = c.Type.Description,
                Price = c.Price,
                StockQuantity = c.StockQuantity,
                Status = (int)c.Status,
                Description = c.Description
            })
            .ToListAsync()) ?? new List<VIEW.ProductByType>();
    }
}