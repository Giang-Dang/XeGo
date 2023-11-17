using Microsoft.Extensions.Configuration;

namespace XeGo.Shared.Lib.Helpers
{
    public static class DockerHelpers
    {
        public static IConfigurationBuilder AddDockerSecrets(this IConfigurationBuilder builder)
        {
            var secretsPath = "/run/secrets";
            if (Directory.Exists(secretsPath))
            {
                foreach (var file in Directory.GetFiles(secretsPath))
                {
                    var lines = File.ReadAllLines(file);
                    if (lines.Length > 0)
                    {
                        var key = Path.GetFileName(file);
                        var value = lines[0];
                        builder.AddInMemoryCollection(new Dictionary<string, string> { { key, value } });
                    }
                }
            }

            return builder;
        }
    }

}
