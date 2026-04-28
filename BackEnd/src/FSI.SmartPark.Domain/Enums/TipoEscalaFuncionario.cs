namespace FSI.SmartPark.Domain.Enums
{
    /// <summary>Escala de trabalho do funcionário.</summary>
    public enum TipoEscalaFuncionario
    {
        Comercial       = 1, // Segunda a sexta horário comercial
        DozePorTrinta   = 2, // 12h trabalhadas x 36h de descanso
        Noturno         = 3, // Turno noturno fixo
        Compensacao     = 4  // Banco de horas / compensação
    }
}
