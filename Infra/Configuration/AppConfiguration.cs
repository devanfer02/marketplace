namespace Marketplace.Infra.Configuration
{
    public class AppConfiguration
    {
        public DatabaseSettings Database { get; set; }
    }

    public class DatabaseSettings
    {
        public string DefaultConnection { get; set; }
    }

}
