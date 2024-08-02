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

        [UIValue("compareUnderswinglessToCurrentValue")]
        private bool compareUnderswingLessToCurrentObject
        {
            get => PluginConfig.Instance.showDifference;
            set => PluginConfig.Instance.showDifference = value;
        }

        [UIValue("displayStyleValue")]
        private object displayStyle
        {
            get
            {
                switch (PluginConfig.Instance.style)
                {
                    case PluginConfig.styleType.Seperate:
                        return "Yes";
                    case PluginConfig.styleType.Unified:
                        return "No";
                    default:
                        return "Both";
                }
            }
            set
            {
                switch (value)
                {
                    case "Seperate Hands":
                        PluginConfig.Instance.style = PluginConfig.styleType.Seperate;
                        return;
                    case "Unify Hands":
                        PluginConfig.Instance.style = PluginConfig.styleType.Unified;
                        return;
                    default:
                        PluginConfig.Instance.style = PluginConfig.styleType.Both;
                        return;
                }
            }
        }

        [UIValue("displayStyleOptions")]
        private List<object> displayStyleOptions = new List<object>() { "Seperate Hands", "Unify Hands", "Both" };

        [UIValue("inheritSaberColorsValue")]
        private bool inheritSaberColors
        {
            get => PluginConfig.Instance.inheritHandColors;
            set => PluginConfig.Instance.inheritHandColors = value;
        }
    }
}
