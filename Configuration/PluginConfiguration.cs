using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace RevertPlayerHeightDetection.Configuration
{
    internal class PluginConfiguration
    {
        public static PluginConfiguration Instance { get; set; }

        public virtual bool Enabled { get; set; } = true;

        public virtual void OnReload()
        {
        }

        public virtual void Changed()
        {
        }

        public virtual void CopyFrom(PluginConfiguration other)
        {
            Enabled = other.Enabled;
        }
    }
}
