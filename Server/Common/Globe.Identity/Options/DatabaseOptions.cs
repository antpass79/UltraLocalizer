using System;

namespace Globe.Identity.Options
{
    public class DatabaseOptions
    {
        public DatabaseType DatabaseType { get; set; }
        public string DefaultInMemoryConnection
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        public string DefaultSqlServerConnection { get; set; }
        public string DefaultSqliteConnection { get; set; }
    }
}
