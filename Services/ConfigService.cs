
namespace Pdf_Acc_Toolset.Services
{
    internal class ConfigService
    {
        public ConfigService() {}

        // public string GetEntry(ConfigEntry key, string defaultValue)
        // {
        //     return Preferences.Get(key.ToString(), defaultValue);
        // }

        // /// <summary>
        // /// Check that a key exists. If it doesn't, create it.
        // /// </summary>
        // /// <param name="key"></param>
        // /// <returns></returns>
        // public async Task<bool> EnsureCreation(ConfigEntry key)
        // {
        //     // Check if the key exists
        //     if (Preferences.ContainsKey(key.ToString())) {
        //         return true;
        //     }

        //     // It doesn't exist, try to create it
        //     return key switch
        //     {
        //         ConfigEntry.ExportDir => await SetExportDir(),
        //         _ => false,
        //     };
        // }

        // /// <summary>
        // /// Sets an entry
        // /// </summary>
        // /// <param name="key"></param>
        // /// <param name="value"></param>
        // public void SetEntry(string key, string value) {
        //     Preferences.Set(key, value);
        // }

        // public void SetEntry(string key, int value)
        // {
        //     Preferences.Set(key, value);
        // }

        // public void SetEntry(string key, double value)
        // {
        //     Preferences.Set(key, value);
        // }

        // public void SetEntry(string key, bool value)
        // {
        //     Preferences.Set(key, value);
        // }

        // private async Task<bool> SetExportDir()
        // {
        //     try
        //     {
        //         // Request a folder
        //         FolderPickerResult result = await FolderPicker.Default.PickAsync(new CancellationToken());

        //         if (result.IsSuccessful)
        //         {
        //             // Save the folder path
        //             Preferences.Set(ConfigEntry.ExportDir.ToString(), result.Folder.Path);
        //             return true;
        //         } else
        //         {
        //             // Create the export folder in the app data directory
        //             string defaultDir = Path.Combine(FileSystem.AppDataDirectory, "Exports");
        //             Directory.CreateDirectory(defaultDir);
        //             // Save the folder path
        //             Preferences.Set(ConfigEntry.ExportDir.ToString(), defaultDir);
        //             return true;
        //         }
        //     } catch (Exception)
        //     {
        //         // A bunch of stuff failed, probably in read only mode or somethin -_-
        //         return false;
        //     }
        // }

        // public void ResetAll()
        // {
        //     Preferences.Clear();
        // }
    }

    internal enum ConfigEntry
    {
        /// <summary>
        /// The directory which will contain exported files.
        /// </summary>
        ExportDir
    }
}
