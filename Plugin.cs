using System.Reflection;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using RevertPlayerHeightDetection.Configuration;
using IPALogger = IPA.Logging.Logger;

namespace RevertPlayerHeightDetection
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        private const string _harmonyId = "com.Python.RevertPlayerHeightDetection";

        internal static Harmony Harmony { get; private set; }
        internal static PluginConfiguration Configuration { get; private set; }
        internal static IPALogger Logger { get; private set; }

        [Init]
        public void Init(
            IPALogger logger,
            Config config)
        {
            Harmony = new Harmony(_harmonyId);
            Configuration = config.Generated<PluginConfiguration>();
            Logger = logger;
        }

        [OnEnable]
        public void OnEnable()
            => Harmony.PatchAll(Assembly.GetExecutingAssembly());

        [OnDisable]
        public void OnDisable()
            => Harmony.UnpatchAll(_harmonyId);
    }
}
