namespace NexCart.Application.Common.Interfaces;

public interface IEmailService
{
  
    Task SendEmailAsync(
        string to,
        string subject,
        string body,
        bool isHtml = true,
        CancellationToken cancellationToken = default);

 
    Task SendEmailAsync(
        IEnumerable<string> to,
        string subject,
        string body,
        bool isHtml = true,
        CancellationToken cancellationToken = default);


    Task SendTemplatedEmailAsync<T>(
        string to,
        string templateName,
        T model,
        CancellationToken cancellationToken = default);
}