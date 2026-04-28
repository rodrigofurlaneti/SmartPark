using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Application.Interfaces.Comercial;
using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.ValueObjects;

namespace FSI.SmartPark.Application.Services.Comercial;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repo;

    public ClienteService(IClienteRepository repo) => _repo = repo;

    public async Task<ClienteResponseDto> Criar(ClienteRequestDto dto)
    {
        Cliente cliente = dto.Documento.Length == 11
            ? new Cliente(dto.Nome, new Cpf(dto.Documento), dto.IsMensalista, dto.EmpresaId)
            : new Cliente(dto.Nome, new Cnpj(dto.Documento), dto.IsMensalista, dto.EmpresaId);

        var id     = await _repo.Add(cliente);
        var criado = await _repo.GetById(id);
        return ToDto(criado!);
    }

    public async Task<ClienteResponseDto> Atualizar(int id, ClienteRequestDto dto)
    {
        var cliente = await _repo.GetById(id)
            ?? throw new KeyNotFoundException($"Cliente {id} não encontrado.");

        cliente.AlterarStatus(true);
        await _repo.Update(cliente);
        return ToDto(cliente);
    }

    public async Task<ClienteResponseDto?> ObterPorId(int id)
    {
        var c = await _repo.GetById(id);
        return c is null ? null : ToDto(c);
    }

    public async Task<ClienteResponseDto?> ObterPorDocumento(string documento)
    {
        var lista = await _repo.GetAll();
        var docLimpo = documento.Replace(".", "").Replace("-", "").Replace("/", "");
        var c = lista.FirstOrDefault(x => x.DocumentoNumero == docLimpo);
        return c is null ? null : ToDto(c);
    }

    public async Task<IEnumerable<ClienteResponseDto>> ListarMensalistas()
    {
        var lista = await _repo.GetAll();
        return lista.Where(c => c.IsMensalista && c.Ativo).Select(ToDto);
    }

    public async Task<IEnumerable<ClienteResponseDto>> ListarTodos()
    {
        var lista = await _repo.GetAll();
        return lista.Select(ToDto);
    }

    public async Task Inativar(int id)
    {
        var c = await _repo.GetById(id)
            ?? throw new KeyNotFoundException($"Cliente {id} não encontrado.");
        c.AlterarStatus(false);
        await _repo.Update(c);
    }

    private static ClienteResponseDto ToDto(Cliente c) => new(
        c.Id, c.Nome, c.DocumentoNumero, c.IsMensalista, c.Ativo, c.Empresa_Id, c.DataInsercao);
}
