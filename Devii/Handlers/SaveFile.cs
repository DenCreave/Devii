using System.Text.Json;
using Devii.Enums;

namespace Devii.Handlers;

public sealed class SaveFile
{
    //todo maybe add stats later
    public LevelUnlock UnlockedLevels { get; set; }
    private string _path => "GameSave.json";

    public SaveFile()
    {
        UnlockedLevels = LevelUnlock.Level1;
    }
    
    

    public void UnlockLevel(LevelUnlock level)
    {
        if (level > UnlockedLevels)
        {
            UnlockedLevels = level;
        }
    }

    public bool Save()
    {
        var appDir = AppContext.BaseDirectory;
                 

        if (File.Exists(Path.Combine(appDir,_path)))
        {
            var check = JsonSerializer.Deserialize<SaveFile>(File.ReadAllText(Path.Combine(appDir, _path)));
            
            if (check is not null && UnlockedLevels <= check.UnlockedLevels)
            {
                return false;
            }
        }

        string serializeMe = JsonSerializer.Serialize(this);
        File.WriteAllText(Path.Combine(appDir, _path), serializeMe);
        return true;
    }

    public bool Load()
    {
        var appDir = AppContext.BaseDirectory;
        if (File.Exists(Path.Combine(appDir,_path)))
        {
            var check = JsonSerializer.Deserialize<SaveFile>(File.ReadAllText(Path.Combine(appDir, _path)));
            if (check is not null)
            {
                UnlockedLevels = check.UnlockedLevels;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

}