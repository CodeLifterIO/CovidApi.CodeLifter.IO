using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApi.Settings
{
    public class ConnectionStringManager
    {
        private DatabaseSettings _settings;

        public ConnectionStringManager(DatabaseSettings settings)
        {
            _settings = settings;
        }

        public string GetConnectionString()
        {
            string dataSource = _settings.DataSource;
            string catalog = _settings.Catalog;
            string userId = _settings.UserId;
            string password = _settings.Password;

            return $"Data Source={dataSource};Initial Catalog={catalog};trusted_connection=False;User Id={userId};Password={password}";
        }
    }
}
