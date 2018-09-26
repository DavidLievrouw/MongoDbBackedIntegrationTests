using System.Configuration;
using System.IO;

namespace DavidLievrouw.Api
{
    public static class ConfigLoaderForIntegrationTests
    {
        public static Configuration LoadConfig()
        {
            var unitTestAssembly = typeof(ConfigLoaderForIntegrationTests).Assembly;
            var directoryName = Path.GetDirectoryName(unitTestAssembly.Location);
            var assemblyPath = directoryName.Replace(@"file:\", string.Empty);
            var configFile = new FileInfo(Path.Combine(assemblyPath, unitTestAssembly.GetName().Name + ".dll.config"));
            var fileMap = new ExeConfigurationFileMap {ExeConfigFilename = configFile.FullName};
            return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }
    }
}