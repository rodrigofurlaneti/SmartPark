using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Enums
{
    public enum FormaPagamento 
    { 
        Dinheiro = 1, 
        CartaoDebito = 2, 
        CartaoCredito = 3, 
        Boleto = 4, 
        Pix = 5, 
        Cheque = 6, 
        SemParar = 7 
    }
}
