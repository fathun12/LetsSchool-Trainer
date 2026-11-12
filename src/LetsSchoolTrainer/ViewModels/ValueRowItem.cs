using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LetsSchoolTrainer.Services;

namespace LetsSchoolTrainer.ViewModels;

// One row of the FUNDS AND POINTS section: a number box and a Set button.
public sealed partial class ValueRowItem : ObservableObject
{
    private readonly string _command;
    private readonly bool _wholeNumber;
    private readonly Action<string, string> _report;
    private readonly Func<string, string, Task> _send;

    public ValueRowItem(string title, string description, string command, bool wholeNumber,
        string defaultValue, Action<string, string> report, Func<string, string, Task> send)
    {
        Title = title;
        Description = description;
        _command = command;
        _wholeNumber = wholeNumber;
        _inputText = defaultValue;
        _report = report;
        _send = send;
    }

    public string Title { get; }
    public string Description { get; }

    [ObservableProperty]
    private string _inputText;

    [RelayCommand]
    private async Task ApplyAsync()
    {
        if (!InputParser.TryParse(InputText, _wholeNumber, out string value))
        {
            _report("warn", Title + ": enter " + (_wholeNumber ? "a whole number." : "a valid number."));
            return;
        }
        await _send(_command + ":" + value, Title);
    }
}