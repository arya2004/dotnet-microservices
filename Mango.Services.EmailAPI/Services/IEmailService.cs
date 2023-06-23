using Mango.Services.EmailAPI.Models;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task SendEmail(CartDto cart);
        Task RegisterUSerEmailLog(string email);
    }
}
