using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LetsSchoolTrainer.Data;
using LetsSchoolTrainer.Models;
using LetsSchoolTrainer.Services;

namespace LetsSchoolTrainer.ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        GlobalRows =
        [
            new ValueRowItem("School Funds", "Set the school's money to this amount.",
                "MONEY", true, "999999", Report, SendAsync),
            new ValueRowItem("All Knowledge Points", "Set Arts, Science, Culture and Sports to the same value.",
                "KNOWLEDGE_ALL", true, "9999", Report, SendAsync),
            new ValueRowItem("All Teacher Stats", "Set every ability of every teacher.",
                "TEACHER_ALL", false, "9999", Report, SendAsync),
            new ValueRowItem("Club Strength", "Set club power for every student who is in a club.",
                "CLUB_STRENGTH", false, "9999", Report, SendAsync),
            new ValueRowItem("Staff Mutation", "All employees: set abilities and grant every certificate.",
                "STAFF_MUTATION", false, "9999", Report, SendAsync),
            new ValueRowItem("Support Capability", "Set medical, cooking, sales and security ability for all staff.",
                "SUPPORT_CAPABILITY", false, "9999", Report, SendAsync),
        ];
        SelectedRole = StaffRoles[0];
    }

    public ValueRowItem[] GlobalRows { get; }
    public Choice[] StaffRoles => GameData.StaffRoles;
    public ObservableCollection<Choice> Teachers { get; } = new();
    public ObservableCollection<Choice> Staff { get; } = new();

    // Status line
    [ObservableProperty]
    private string _statusText = "Ready.";

    [ObservableProperty]
    private string _statusKind = "idle";

    // Knowledge by category (blank = unchanged)
    [ObservableProperty]
    private string _artsText = "";

    [ObservableProperty]
    private string _scienceText = "";

    [ObservableProperty]
    private string _cultureText = "";

    [ObservableProperty]
    private string _sportsText = "";

    // Single teacher (blank = unchanged)
    [ObservableProperty]
    private Choice? _selectedTeacher;

    [ObservableProperty]
    private string _teachingText = "";

    [ObservableProperty]
    private string _researchText = "";

    [ObservableProperty]
    private string _managingText = "";

    [ObservableProperty]
    private string _cultivatingText = "";

    // Single staff member
    [ObservableProperty]
    private Choice? _selectedStaff;

    [ObservableProperty]
    private Choice? _selectedRole;

    [ObservableProperty]
    private string _staffSkillText = "9999";

    public void Report(string kind, string text)
    {
        StatusKind = kind;
        StatusText = text;
    }

    public async Task SendAsync(string line, string label)
    {
        Report("idle", label + ": sending...");
        SendResult result = await GameLink.SendAsync(line);
        switch (result)
        {
            case SendResult.Delivered:
                Report("ok", label + ": sent to the game.");
                break;
            case SendResult.NotPickedUp:
                Report("warn", label + ": the game did not pick it up. Is Let's School running with the plugin?");
                break;
            default:
                Report("warn", label + ": could not write the command file.");
                break;
        }
    }

    [RelayCommand]
    private Task UnlockFurnitureAsync() => SendAsync("UNLOCK_ALL_FURNITURE", "Unlock All Furniture");

    [RelayCommand]
    private Task CleanGroundAsync() => SendAsync("CLEAN_INVALID_GROUND", "Clean Invalid Ground");

    [RelayCommand]
    private async Task ApplyKnowledgeAsync()
    {
        if (!InputParser.TryOptional(ArtsText, true, out string a) ||
            !InputParser.TryOptional(ScienceText, true, out string s) ||
            !InputParser.TryOptional(CultureText, true, out string c) ||
            !InputParser.TryOptional(SportsText, true, out string p))
        {
            Report("warn", "Knowledge: use whole numbers, or leave a box blank.");
            return;
        }
        if (a + s + c + p == "")
        {
            Report("warn", "Knowledge: enter at least one value.");
            return;
        }
        await SendAsync($"KNOWLEDGE_ONE:{a};{s};{c};{p}", "Knowledge");
    }

    [RelayCommand]
    private async Task LoadTeachersAsync()
    {
        Report("idle", "Teachers: loading list...");
        string[]? lines = await GameLink.RequestListAsync("LIST_TEACHERS", GameData.TeacherListFile);
        if (lines is null)
        {
            Report("warn", "Teachers: no answer from the game. Is Let's School running with the plugin?");
            return;
        }
        Fill(Teachers, lines, false);
        SelectedTeacher = Teachers.FirstOrDefault();
        Report("ok", $"Teachers: {Teachers.Count} loaded.");
    }

    [RelayCommand]
    private async Task ApplyTeacherAsync()
    {
        if (SelectedTeacher is null)
        {
            Report("warn", "Teacher: load the list and pick a teacher first.");
            return;
        }
        if (!InputParser.TryOptional(TeachingText, false, out string t) ||
            !InputParser.TryOptional(ResearchText, false, out string r) ||
            !InputParser.TryOptional(ManagingText, false, out string m) ||
            !InputParser.TryOptional(CultivatingText, false, out string c))
        {
            Report("warn", "Teacher: use valid numbers, or leave a box blank.");
            return;
        }
        if (t + r + m + c == "")
        {
            Report("warn", "Teacher: enter at least one value.");
            return;
        }
        await SendAsync($"TEACHER_ONE:{SelectedTeacher.Key};{t};{r};{m};{c}", "Teacher");
    }

    [RelayCommand]
    private async Task LoadStaffAsync()
    {
        Report("idle", "Staff: loading list...");
        string[]? lines = await GameLink.RequestListAsync("LIST_STAFF", GameData.StaffListFile);
        if (lines is null)
        {
            Report("warn", "Staff: no answer from the game. Is Let's School running with the plugin?");
            return;
        }
        Fill(Staff, lines, true);
        SelectedStaff = Staff.FirstOrDefault();
        Report("ok", $"Staff: {Staff.Count} loaded.");
    }

    [RelayCommand]
    private async Task ApplyStaffAsync()
    {
        if (SelectedStaff is null || SelectedRole is null)
        {
            Report("warn", "Staff: load the list, then pick a person and a role.");
            return;
        }
        if (!InputParser.TryDecimal(StaffSkillText, out string skill))
        {
            Report("warn", "Staff: skill value must be a valid number.");
            return;
        }
        await SendAsync($"STAFF_TARGET:{SelectedStaff.Key}:{SelectedRole.Key}:{skill}", "Staff");
    }

    // List lines look like "id|name" (teachers) or "id|name|role" (staff).
    private static void Fill(ObservableCollection<Choice> target, string[] lines, bool showRole)
    {
        target.Clear();
        foreach (string line in lines)
        {
            string[] parts = line.Split('|');
            if (parts.Length < 2 || parts[0].Trim() == "")
                continue;
            string name = parts[1].Trim();
            string label = showRole && parts.Length > 2 ? name + " - " + parts[2].Trim() : name;
            target.Add(new Choice(label, parts[0].Trim()));
        }
    }
}