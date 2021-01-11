using Newtonsoft.Json;
using System;
using System.IO;

namespace OverviewRkiData.Components.UserSettings
{
    internal class UserSettingsLoader : IUserSettingsComponent
    {
        private static UserSettingsLoader _userSettings;
        private readonly string _settingFile = $"{Environment.CurrentDirectory}\\settings.json";

        public static UserSettingsLoader GetInstance() => _userSettings ??= new UserSettingsLoader();

        public SettingsFile Load()
        {
            if (!File.Exists(this._settingFile))
            {
                this.Save(new SettingsFile(true));
            }

            var fileContent = File.ReadAllText(this._settingFile);

            var setting = JsonConvert.DeserializeObject<SettingsFile>(fileContent);
            setting.NoChanged();
            return setting;
        }

        public void Save(SettingsFile firstSetting)
        {
            if (!firstSetting.HasChanged)
            {
                return;
            }

            var toSave = JsonConvert.SerializeObject(firstSetting);
            File.WriteAllText(this._settingFile, toSave);
        }
    }
}
