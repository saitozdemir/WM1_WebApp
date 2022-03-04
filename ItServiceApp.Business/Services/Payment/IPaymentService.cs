using ITServiceApp.Core.Payment;

namespace ITServiceApp.Business.Services.Payment
{
    public interface IPaymentService
    {
        public InstallmentModel CheckInstallments(string binNumber,decimal price);
        public PaymentResponseModel Pay(PaymentModel model);
    }
}
