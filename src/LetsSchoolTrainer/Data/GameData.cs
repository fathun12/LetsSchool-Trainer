using LetsSchoolTrainer.Models;

namespace LetsSchoolTrainer.Data;

public static class GameData
{
    // Shared with the BepInEx plugin (ModGuruJenius). Do not change without changing the plugin.
    public const string CommandFile = @"C:\Users\Public\trainer_ls.txt";
    public const string TeacherListFile = @"C:\Users\Public\trainer_ls_teachers.txt";
    public const string StaffListFile = @"C:\Users\Public\trainer_ls_staff.txt";

    // Label = text shown in the UI, Key = role name the plugin expects.
    public static readonly Choice[] StaffRoles =
    [
        new("Manager Lv1", "Manajer_Lv1"),
        new("Manager Lv2", "Manajer_Lv2"),
        new("Manager Lv3", "Manajer_Lv3"),
        new("Headmaster", "Headmaster"),
        new("Security", "Satpam"),
        new("Doctor", "Dokter"),
        new("Sales", "Sales"),
        new("Chef", "Koki"),
    ];
}