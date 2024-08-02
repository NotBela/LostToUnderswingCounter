using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace LostToUnderswingCounter.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public virtual styleType style { get; set; } = styleType.Both;
        public virtual int decimalPrecision { get; set; } = 2;
        public virtual bool inheritHandColors { get; set; } = true;
        public virtual bool showDifference { get; set; } = true;

        public enum styleType
        {
            Seperate,
            Unified,
            Both
        }
    }
}