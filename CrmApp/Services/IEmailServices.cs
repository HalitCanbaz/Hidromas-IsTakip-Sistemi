namespace CrmApp.Services
{
    public interface IEmailServices
    {
        Task SendResetPasswordEmail(string emailLink, string toEmail);

    }
}
