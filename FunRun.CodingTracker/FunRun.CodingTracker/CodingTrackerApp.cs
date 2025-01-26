using FunRun.CodingTracker.Data.Model;
using FunRun.CodingTracker.Services.Interfaces;
using Spectre.Console;
using Spectre.Console.Extensions;

namespace FunRun.CodingTracker;

public class CodingTrackerApp
{
    private ISessionCrudService _sessionCrud;
    private IUserInputService _userInput;

    public CodingTrackerApp(ISessionCrudService sessionCrud, IUserInputService userInput)
    {
        _sessionCrud = sessionCrud;
        _userInput = userInput;
    }

    public async Task RunApp()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("CodingTracker").Centered().Color(Color.Blue));

            AnsiConsole.MarkupLine("[blue] Inpired by the [link=https://thecsharpacademy.com/project/13/coding-tracker]C#Acadamy [/][/]");
            AnsiConsole.MarkupLine("");

            var cSessions = _sessionCrud.GetAllSessions();
            cSessions.Add(new CodingSession(0, DateTime.MinValue, DateTime.MinValue));

            var table = new Table().Centered();
            table.Border = TableBorder.Rounded;

            table.AddColumn(" Id").Centered();
            table.AddColumn("StartTime");
            table.AddColumn("EndTime");
            table.AddColumn(new TableColumn("DurationInMin").RightAligned());

            int selectedIndex = 0;
            bool exit = false;
            CodingSession selectedSession = null;

            await AnsiConsole.Live(table)
                .Overflow(VerticalOverflow.Ellipsis)
                .StartAsync(async ctx =>
                {
                    while (!exit)
                    {
                        table.Rows.Clear();
                        table.Title("[[ [green]Coding Session Overview [/]]]");
                        table.Caption("[[[blue] [[Up/Down]] Navigation, [[Enter]] Select, [[ESC]] Escape[/]]]");

                        for (int i = 0; i < cSessions.Count; i++)
                        {
                            var session = cSessions[i];
                            if(cSessions.Count - 1 == i)
                            {
                                if (i == selectedIndex)
                                {
                                    table.AddRow(
                                        $"[blue]>{session.Id}[/]",
                                        $"[blue]> Create new Session <[/]",
                                        $"[blue]> Create new Session <[/]",
                                        $"[blue] [/]"
                                    );
                                }
                                else
                                {
                                    table.AddRow(
                                       $"[dim]{session.Id}[/]",
                                       $"[dim]> Create new Session <[/]",
                                       $"[dim]> Create new Session <[/]",
                                       $"[dim] [/]"
                                   );
                                }

                            }
                            else if (i == selectedIndex)
                            {
                                table.AddRow(
                                    $"[blue]>{session.Id}[/]",
                                    $"[blue]{session.StartTime}[/]",
                                    $"[blue]{session.EndTime}[/]",
                                    $"[blue]{session.Duration.TotalMinutes}[/]"
                                );
                            }
                            else
                            {
                                table.AddRow(
                                    session.Id.ToString(),
                                    session.StartTime.ToString(),
                                    session.EndTime.ToString(),
                                    session.Duration.TotalMinutes.ToString()
                                );
                            }
                        }


                        ctx.Refresh();

                        var key = Console.ReadKey(true).Key;

                        switch (key)
                        {
                            case ConsoleKey.Escape:
                                exit = true;
                                break;

                            case ConsoleKey.UpArrow:
                                selectedIndex--;
                                if (selectedIndex < 0)
                                    selectedIndex = cSessions.Count - 1;
                                break;

                            case ConsoleKey.DownArrow:
                                selectedIndex++;
                                if (selectedIndex >= cSessions.Count)
                                    selectedIndex = 0;
                                break;

                            case ConsoleKey.Enter:

                                selectedSession = cSessions[selectedIndex];
                                exit = true;
                                break;
                        }
                    }
                });


            if (selectedSession != null)
            {
                if (selectedSession.Id == 0)
                {
                    var codSes = _userInput.ValidateUserSessionInput();
                    _sessionCrud.CreateSession(codSes);
                }
                else
                {
                    var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .PageSize(10)
                            .AddChoices(new[] {
                                "Update", "Delete", "Back"
                            }));

                    if (choice == "Update")
                    {
                        var codSes = _userInput.ValidateUserSessionInput(selectedSession);
                  
                        _sessionCrud.UpdateSession(codSes);
                    }
                    if (choice == "Delete")
                    {
                        var confirmation = AnsiConsole.Prompt(
                            new TextPrompt<bool>($"[yellow]Are you sure you want to [red]Delete[/] the Session: [/]")
                                .AddChoice(true)
                                .AddChoice(false)
                                .DefaultValue(false)
                                .WithConverter(choice => choice ? "y" : "n"));

                        if (confirmation)
                        {
                            _sessionCrud.DeleteSession(selectedSession);
                        }
                    }
                }
            }
            else
            {
                var confirmation = AnsiConsole.Prompt(
        new TextPrompt<bool>($"[yellow]Are you sure you want to Close the App[/]")
            .AddChoice(true)
            .AddChoice(false)
            .DefaultValue(false)
            .WithConverter(choice => choice ? "y" : "n"));

                if (confirmation)
                {
                    break;
                }
            }
        }
    }

    


}
