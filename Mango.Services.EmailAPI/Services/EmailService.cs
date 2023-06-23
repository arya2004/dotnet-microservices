using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Licenses;
using NuGet.Packaging.Signing;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> options)
        {
            this._dbOptions = options;
        }

        public async Task RegisterUSerEmailLog(string email)
        {
            string message = "User reg sucsec. EMail : " + email;
            await LogANdEmail("admin@admin.com", message);
        }

        public async Task SendEmail(CartDto cart)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<br/>Cart Email Requeste");
            message.AppendLine("<br/>Total" + cart.CartHeader.CartTotal);
            message.AppendLine("<br/>");
            message.AppendLine("<ul/>");
            if (cart.CartDetails != null)
            {
                foreach (var item in cart.CartDetails)
                {
                    message.AppendLine("<li/>");
                    message.AppendLine(item.Product.Name + " X " + item.Count);
                    message.AppendLine("<li/>");
                }
            }
            message.AppendLine("<ul/>");
            await LogANdEmail(cart.CartHeader.Email, message.ToString());
        }
        private async Task<bool> LogANdEmail(string email, string message)
        {
            try
            {
                EmailLogger emailLogger = new()
            { Email = email,
            EmailSent = DateTime.Now,
            Message = message
                
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLogger);
                await _db.SaveChangesAsync();


                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
