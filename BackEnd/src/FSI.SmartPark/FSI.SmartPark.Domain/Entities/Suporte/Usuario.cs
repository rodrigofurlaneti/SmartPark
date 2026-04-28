using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Usuario : EntityBase
    {
        public string Login { get; private set; }
        public string Senha { get; private set; }
        public bool Ativo { get; private set; }
        public int? Funcionario_Id { get; private set; }
        public int? Unidade_Id { get; private set; }
        public Usuario() { }
        public Usuario(string login, string senha, int? unidadeId = null)
        {
            if (string.IsNullOrWhiteSpace(login)) throw new Exception("Login obrigatório");
            Login = login;
            Senha = senha; // Aqui no futuro usaremos Hash
            Ativo = true;
            Unidade_Id = unidadeId;
        }

        public void Bloquear() => Ativo = false;
    }
}
