using Windows.Foundation.Collections;
using Windows.Storage;

namespace JapprendACompter
{
    public static class Config
    {
        private const string MultiplicationMaxSetting = "MultiplicationMax";
        private const string LearningModeSetting = "LearningMode";

        private static IPropertySet SettingValues => ApplicationData.Current.RoamingSettings.Values;

        public static int MultiplicationMax
        {
            get { return (int?)SettingValues[MultiplicationMaxSetting] ?? 5; }
            set { SettingValues[MultiplicationMaxSetting] = value; }
        }

        public static bool LearningMode
        {
            get { return (bool?)SettingValues[LearningModeSetting] ?? false; }
            set { SettingValues[LearningModeSetting] = value; }
        }
    }
}
