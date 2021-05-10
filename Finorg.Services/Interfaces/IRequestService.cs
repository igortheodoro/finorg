using System;
using System.Collections.Generic;
using System.Text;

namespace Finorg.Services.Interfaces
{
    public interface IRequestService
    {
        T Get<T>(string url, string endpoint);
        List<T> Post<T>(string url, string endpoint, object body);
    }
}
