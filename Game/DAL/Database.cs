using System.Configuration;

namespace DAL
{
    internal static class Database
    {
        internal static string ConnectionString
        {
            get
            {
                var configured = ConfigurationManager.ConnectionStrings["GameDatabase"];
                return configured != null
                    ? configured.ConnectionString
                    : "Data Source=.\\SQLEXPRESS;Initial Catalog=GameDatabase;Integrated Security=True";
            }
        }
    }
}
