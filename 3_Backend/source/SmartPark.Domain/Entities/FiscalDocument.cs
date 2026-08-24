using SmartPark.Domain.Primitives;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class FiscalDocument : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid EstablishmentId { get; private set; }
        public Guid? InvoiceId { get; private set; }
        public FiscalDocumentType DocumentType { get; private set; }
        public string? DocumentNumber { get; private set; }
        public string? ExternalId { get; private set; }
        public DateTime? IssueDate { get; private set; }
        public FiscalDocumentStatus Status { get; private set; }
        public string? Payload { get; private set; }
        public string? Response { get; private set; }
        private FiscalDocument() : base(Guid.Empty) { }
        public FiscalDocument(
            Guid id,
            Guid companyId,
            Guid establishmentId,
            Guid? invoiceId,
            FiscalDocumentType documentType) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (establishmentId == Guid.Empty)
                throw new ArgumentException("O identificador do estabelecimento (EstablishmentId) é obrigatório.", nameof(establishmentId));
            CompanyId = companyId;
            EstablishmentId = establishmentId;
            InvoiceId = invoiceId;
            DocumentType = documentType;
            Status = FiscalDocumentStatus.Pending;
        }
        public void StartProcessing(string? payload = null)
        {
            if (Status != FiscalDocumentStatus.Pending && Status != FiscalDocumentStatus.Rejected)
                throw new InvalidOperationException("Apenas documentos pendentes ou rejeitados podem entrar em processamento.");
            Status = FiscalDocumentStatus.Processing;
            if (!string.IsNullOrWhiteSpace(payload))
                Payload = payload;
            
        }
        public void Authorize(string documentNumber, string? externalId = null, string? response = null)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new ArgumentException("O número do documento autorizado é obrigatório.", nameof(documentNumber));
            DocumentNumber = documentNumber;
            ExternalId = externalId;
            Response = response;
            IssueDate = DateTime.UtcNow;
            Status = FiscalDocumentStatus.Authorized;
            
        }
        public void Reject(string? response = null)
        {
            Response = response;
            Status = FiscalDocumentStatus.Rejected;
            UpdatedAt = DateTime.Now;
        }
        public void Cancel(string? response = null)
        {
            if (Status != FiscalDocumentStatus.Authorized)
                throw new InvalidOperationException("Apenas documentos autorizados podem ser cancelados.");
            Response = response;
            Status = FiscalDocumentStatus.Cancelled;
            UpdatedAt = DateTime.Now;
        }
    }
}