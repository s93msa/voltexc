namespace VoltigeCore
{
    public static class AppConfig
    {
        public static string ConnectionString { get; set; }
        public static string BasePath { get; set; }

        // Derived paths
        public static string ContentRootPath => BasePath + @"voltexc\WebApplication1\";
        public static string StartlistOutputPath => BasePath + @"voltexc\output\";
        public static string OutputPath => BasePath + @"output\";

        public static int ContestId { get; set; }
        public static bool IsTraHastTavling { get; set; }
        public static float HorsePointTraHastTavling { get; set; }
    }
}
