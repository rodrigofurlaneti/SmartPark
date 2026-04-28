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
        Cliente cliente;
        if (dto.Documento.Length == 11)
            cliente = new Cliente(dto.Nome, new Cpf(dto.Documento), dto.IsMensalista);
        else
            cliente = new Cliente(dto.Nome, new Cnpj(dto.Documento), dto.IsMensalista);

        var id = await _repo.Inserir(cliente);
        var criado = await _repo.ObterPorId(id);
        return ToDto(criado!);
    }

    public async Task<ClienteResponseDto> Atualizar(int id, ClienteRequestDto dto)
    {
        var cliente = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Cliente {id} não encontrado.");
        cliente.AlterarStatus(true);
        await _repo.Atualizar(cliente);
        return ToDto(cliente);
    }

    public async Task<ClienteResponseDto?> ObterPorId(int id)
    {
        var c = await _repo.ObterPorId(id);
        return c is null ? null : ToDto(c);
    }

    public async Task<ClienteResponseDto?> ObterPorDocumento(string documento)
    {
        var lista = await _repo.ListarTodos();
        var c = lista.FirstOrDefault(x => x.DocumentoNumero == documento.Replace(".", "").Replace("-", "").Replace("/", ""));
        return c is null ? null : ToDto(c);
    }

    public async Task<IEnumerable<ClienteResponseDto>> ListarMensalistas()
    {
        var lista = await _repo.ListarTodos();
        return lista.Where(c => c.IsMensalista && c.Ativo).Select(ToDto);
    }

    public async Task<IEnumerable<ClienteResponseDto>> ListarTodos()
    {
        var lista = await _repo.ListarTodos();
        return lista.Select(ToDto);
    }

    public async Task Inativar(int id)
    {
        var c = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Cliente {id} não encontrado.");
        c.AlterarStatus(false);
        await _repo.Atualizar(c);
    }

    private static ClienteResponseDto ToDto(Cliente c) => new(
        c.Id, c.Nome, c.DocumentoNumero, c.IsMensalista, c.Ativo, c.DataInsercao);
}
