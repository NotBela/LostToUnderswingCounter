using BeatSaberMarkupLanguage.Attributes;
using LostToUnderswingCounter.Configuration;


namespace LostToUnderswingCounter.Views
{
    class ConfigurationController
    {
        [UIValue("decimalPrecisionValue")]
        private int decimalPrecision
        {
            get => PluginConfig.Instance.decimalPrecision;
            set => PluginConfig.Instance.decimalPrecision = value;
        }

        [UIValue("seperateHandsValue")]
        private bool seperateHands
        {
            get => PluginConfig.Instance.seperateHands;
            set => PluginConfig.Instance.seperateHands = value;
        }

        [UIValue("inheritSaberColorsValue")]
        private bool inheritSaberColors
        {
            get => PluginConfig.Instance.inheritHandColors;
            set => PluginConfig.Instance.inheritHandColors = value;
        }
    }
}
