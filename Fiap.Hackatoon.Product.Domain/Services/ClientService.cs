using System.ComponentModel.DataAnnotations;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

namespace Fiap.Hackatoon.Product.Domain.Services;

public class ClientService(IClientRepository clientRepository) : BaseService<DO.Client>(clientRepository), IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<DO.Client> GetById(Guid id, bool include, bool tracking)
    {
        var entity = await _clientRepository.GetById(id, include, tracking);

        if (entity == null)
            throw new ValidationException("O cliente não existe.");

        return entity;
    }

    public override async Task<DO.Client> Add(DO.Client entity)
    {
        var product = await _clientRepository.GetById(entity.Id);

        if (product != null)
            throw new ValidationException("O cliente já existe.");
        
        return await base.Add(entity);
    }

    public override async Task<DO.Client> Update(DO.Client entity)
    {
        return await base.Update(entity);
    }

    public async Task Remove(Guid id)
    {
        var entity = await _clientRepository.GetById(id, false, true);
        if (entity == null)
            throw new Exception("O cliente não existe.");

        await base.Remove(entity);
    }
}