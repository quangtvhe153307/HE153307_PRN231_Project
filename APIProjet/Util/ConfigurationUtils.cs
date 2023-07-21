namespace APIProject.Utils
{
    public class ConfigurationUtils
    {
        private static IConfiguration _config;
        public static IConfiguration Configuration
        {
            get { return _config; }
        }
        public static void Init(IConfiguration config)
        {
            _config = config;
        }
    }
}
