using FunRun.CodingTracker.Data.Model;
using FunRun.CodingTracker.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace FunRun.CodingTracker.Services;

public class UserInputService : IUserInputService
{
    private ILogger<UserInputService> _log;

    public UserInputService(ILogger<UserInputService> log)
    {
        _log = log;
    }

    public CodingSession ValidateUserSessionInput(CodingSession? codingSession = null)
    {
        AnsiConsole.MarkupLine("[yellow]Please provide the details for your coding session.[/]");


        DateTime sessionStart = default;
        DateTime sessionEnd = default;
        long Id = -1;

        AnsiConsole.Prompt(
             new TextPrompt<string>("[yellow]Enter the [green]Start Time[/] (format: yyyy-MM-dd HH:mm):[/]")
                 .Validate(input =>
                 {
                     if (DateTime.TryParse(input, out var startDate) is not true)
                         return ValidationResult.Error("[red]Please enter a valid date and time in the format 'yyyy-MM-dd HH:mm'.[/]");

                     sessionStart = startDate;
                     return ValidationResult.Success();
                 }));

        AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]Enter the [green]End Time[/] (format: yyyy-MM-dd HH:mm):[/]")
                .Validate(input =>
                {
                    if (DateTime.TryParse(input, out var endDate) is not true)
                        return ValidationResult.Error("[red]Please enter a valid date and time in the format 'yyyy-MM-dd HH:mm'.[/]");

                    if (endDate <= sessionStart)
                        return ValidationResult.Error("[red]End Time must be after the Start Time![/]");

                    sessionEnd = endDate;
                    return ValidationResult.Success();
                }));

        // If updating an existing session
        if (codingSession != null)
        {
            Id=codingSession.Id;
        }

        var session = new CodingSession(Id, sessionStart, sessionEnd);
        _log.LogInformation("Validated session input: {Session}", session);

        return session;
    }
}
