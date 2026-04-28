using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Notificacao : EntityBase
    {
        public string Mensagem { get; private set; }
        public bool Lida { get; private set; }
        public int? Usuario_Id { get; private set; }

        public void MarcarComoLida() => Lida = true;
    }
}
