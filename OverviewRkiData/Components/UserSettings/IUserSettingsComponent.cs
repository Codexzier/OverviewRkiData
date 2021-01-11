namespace OverviewRkiData.Components.UserSettings
{
    public interface IUserSettingsComponent
    {
        SettingsFile Load();
        void Save(SettingsFile firstSetting);
    }
}