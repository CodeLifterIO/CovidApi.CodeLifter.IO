using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApi.Services
{
    public interface IEnvironmentService
    {
        string GetValue(string name);
        bool IsDebug();
    }


    public class EnvironmentService : IEnvironmentService
    {
        public string GetValue(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

        public bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

    }
}
