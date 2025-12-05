using ChurchSaaS.Admin.Domain.Entities;
using ChurchSaaS.Client.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Infrastructure.Persistence;
using ChurchSaaS.Client.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChurchSaaS.Admin.Infrastructure.Repositories;

public class EnderecoRepository : IEnderecoRepository
{
    private readonly AdminDbContext _context;

    public EnderecoRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Endereco>> ListByCidadeAsync(int cidadeId, CancellationToken cancellationToken = default)
    {
        return await _context.Enderecos
            .AsNoTracking()
            .Where(e => e.CidadeId == cidadeId)
            .OrderBy(e => e.Logradouro)
            .ThenBy(e => e.Numero)
            .ToListAsync(cancellationToken);
    }
}
