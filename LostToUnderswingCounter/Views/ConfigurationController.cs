using BeatSaberMarkupLanguage.Attributes;
using LostToUnderswingCounter.Configuration;
using System.Collections.Generic;

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

        [UIValue("displayModeOptions")] private List<object> displayModeOptions = new List<object>()
            { PluginConfig.displayType.Difference, PluginConfig.displayType.Added, PluginConfig.displayType.Points };
        
        [UIValue("displayModeValue")]
        private object displayModeValue
        {
            get => PluginConfig.Instance.display;
            set => PluginConfig.Instance.display = (PluginConfig.displayType) value;
        }

        [UIValue("displayStyleValue")]
        private object displayStyleValue
        {
            get => PluginConfig.Instance.style;
            set => PluginConfig.Instance.style = (PluginConfig.styleType) value;
        }

        [UIValue("displayStyleOptions")]
        private List<object> displayStyleOptions = new List<object>() { PluginConfig.styleType.Seperate, PluginConfig.styleType.Unified, PluginConfig.styleType.Both };

        [UIValue("inheritSaberColorsValue")]
        private bool inheritSaberColors
        {
            get => PluginConfig.Instance.inheritHandColors;
            set => PluginConfig.Instance.inheritHandColors = value;
        }

        [UIValue("showCounterLabelValue")]
        private bool showCounterLabelValue
        {
            get => PluginConfig.Instance.showHeaderText;
            set => PluginConfig.Instance.showHeaderText = value;
        }
    }
}
