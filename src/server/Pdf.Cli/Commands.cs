using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Pdf.Models;
using Pdf.Processor;
using Pdf.Processor.Generator;
using Pdf.Processor.Manager;

namespace Pdf.Cli;
public static class Commands
{
    public static RootCommand Initialize() =>
        BuildCommands()
            .BuildRootCommand();

    static RootCommand BuildRootCommand(this List<Command> commands)
    {
        var root = new RootCommand("PDF Generator");
        commands.ForEach(root.AddCommand);

        return root;
    }

    static List<Command> BuildCommands() =>
    [
        BuildIron(),
        BuildIText()
    ];

    static Command BuildIron() =>
        BuildCommand(
            "generate-iron",
            "Generate a PDF using IronPdf",
            [
                new Option<string>(
                    ["--src", "--s"],
                    getDefaultValue: () => Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "files",
                        "ssn.pdf"
                    ),
                    description: "PDF template source"
                ),
                new Option<string>(
                    ["--dest", "--d"],
                    getDefaultValue: () => "iron-ssn.pdf",
                    description: "Generated PDF destination"
                )
            ],
            new Func<string, string, Task>(async (src, dest) =>
            {
                IronGenerator generator = new();
                Person person = Person.Generate();
                PdfRecord<Person> record = new(person);
                await generator.Generate(record, src, dest);

                using IronManager manager = new(dest);
                manager.ReadFields();                
            })
        );

    static Command BuildIText() =>
        BuildCommand(
            "generate-itext",
            "Generate a PDF using iText",
            [
                new Option<string>(
                    ["--src", "--s"],
                    getDefaultValue: () => Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "files",
                        "ssn.pdf"
                    ),
                    description: "PDF template source"
                ),
                new Option<string>(
                    ["--dest", "--d"],
                    getDefaultValue: () => "itext-ssn.pdf",
                    description: "Generated PDF destination"
                )
            ],
            new Func<string, string, Task>(async (src, dest) =>
            {
                ITextGenerator generator = new();
                Person person = Person.Generate();
                PdfRecord<Person> record = new(person);
                await generator.Generate(record, src, dest);

                using ITextManager manager = new(dest);
                manager.ReadFields();
            })
        );

    static Command BuildCommand(string name, string description, List<Option> options, Func<string, string, Task> @delegate)
    {
        var command = new Command(name, description)
        {
            Handler = CommandHandler.Create(@delegate)
        };

        foreach (var o in options)
            command.AddOption(o);

        return command;
    }
}