using Windows.Foundation.Collections;
using Windows.Storage;

namespace JapprendACompter
{
    public static class Config
    {
        private const string MultiplicationMaxSetting = "MultiplicationMax";

        private static IPropertySet SettingValues { get { return ApplicationData.Current.RoamingSettings.Values; } }

        public static int MultiplicationMax
        {
            get { return (int?)SettingValues[MultiplicationMaxSetting] ?? 5; }
            set { SettingValues[MultiplicationMaxSetting] = value; }
        }
    }
}
