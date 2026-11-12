using System.IO;
using LetsSchoolTrainer.Data;

namespace LetsSchoolTrainer.Services;

public enum SendResult
{
    WriteFailed,
    Delivered,
    NotPickedUp,
}

public static class GameLink
{
    // The plugin checks the command file about twice a second, reads it, then deletes it.
    // "Delivered" means the plugin took the file. It does not prove the command succeeded in the game.
    public static async Task<SendResult> SendAsync(string line)
    {
        try
        {
            File.AppendAllText(GameData.CommandFile, line + "\n");
        }
        catch (Exception)
        {
            return SendResult.WriteFailed;
        }

        for (int i = 0; i < 50; i++)
        {
            await Task.Delay(100);
            if (!File.Exists(GameData.CommandFile))
                return SendResult.Delivered;
        }
        return SendResult.NotPickedUp;
    }

    // Asks the plugin to write a list file and reads it. Returns null when the game did not answer.
    public static async Task<string[]?> RequestListAsync(string command, string listFile)
    {
        try
        {
            if (File.Exists(listFile))
                File.Delete(listFile);
        }
        catch (Exception)
        {
            return null;
        }

        if (await SendAsync(command) != SendResult.Delivered)
            return null;

        for (int i = 0; i < 30; i++)
        {
            try
            {
                if (File.Exists(listFile))
                    return File.ReadAllLines(listFile);
            }
            catch (IOException)
            {
                // The plugin may still be writing; try again.
            }
            await Task.Delay(100);
        }
        return null;
    }
}