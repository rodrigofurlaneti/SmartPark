using SmartPark.Domain.Primitives;

namespace SmartPark.Domain.Entities
{
    public sealed class Company : AggregateRoot
    {
        public string LegalName { get; private set; } = null!;
        public string TradeName { get; private set; } = null!;
        public string Cnpj { get; private set; } = null!;
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        private Company() : base(Guid.Empty) { }
        private Company(Guid id, string legalName, string tradeName, string cnpj, string? email, string? phone) : base(id)
        {
            LegalName = legalName;
            TradeName = tradeName;
            Cnpj = cnpj;
            Email = email;
            Phone = phone;
        }
        public static Result<Company> Create(string legalName, string tradeName, string cnpj, string? email, string? phone)
        {
            if (string.IsNullOrWhiteSpace(legalName))
                return Result.Failure<Company>(new Error("Company.EmptyLegalName", "LegalName is required."));
            if (string.IsNullOrWhiteSpace(tradeName))
                return Result.Failure<Company>(new Error("Company.EmptyTradeName", "TradeName is required."));
            if (string.IsNullOrWhiteSpace(cnpj))
                return Result.Failure<Company>(new Error("Company.EmptyCnpj", "Cnpj is required."));
            var company = new Company(Guid.NewGuid(), legalName, tradeName, cnpj, email, phone);
            return Result.Success(company);
        }
        public void UpdateDetails(string legalName, string tradeName, string? email, string? phone)
        {
            LegalName = legalName;
            TradeName = tradeName;
            Email = email;
            Phone = phone;
            UpdatedAt = DateTime.Now;
        }
    }
}