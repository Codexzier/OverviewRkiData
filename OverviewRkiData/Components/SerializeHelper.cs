using Newtonsoft.Json;
using OverviewRkiData.Components.UserSettings;
using System;

namespace OverviewRkiData.Components
{
    public static class SerializeHelper
    {
        public static Func<CustomSettingsFile, string> Serialize = JsonConvert.SerializeObject;
        public static Func<string, CustomSettingsFile> Deserialize = JsonConvert.DeserializeObject<CustomSettingsFile>;
    }
}
