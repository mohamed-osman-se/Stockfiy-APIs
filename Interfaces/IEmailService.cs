using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string toEmail, string subject, string body);

    }

}