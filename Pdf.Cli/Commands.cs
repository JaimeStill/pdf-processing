using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Pdf.Models;
using Pdf.Processor;

namespace Pdf.Cli;
public static class Commands
{
    public static RootCommand Initialize() =>
        BuildCommands()
            .BuildRootCommand();

    static RootCommand BuildRootCommand(this List<Command> commands)
    {
        var root = new RootCommand("PDF Generator");
        commands.ForEach(command => root.AddCommand(command));

        return root;
    }

    static List<Command> BuildCommands() => new()
    {
        BuildIron(),
        BuildIText()
    };

    static Command BuildIron() =>
        BuildCommand(
            "generate-iron",
            "Generate a PDF using IronPdf",
            new Func<string, string, Task>(async (src, dest) =>
            {
                IGenerator generator = PdfGenerator.CreateIronGenerator(src, dest);
                Person person = Person.Generate();
                Record<Person> record = new(person);
                await generator.Generate(record);

                using IManager manager = PdfManager.CreateIronManager(dest);
                manager.ReadFields();                
            }),
            new List<Option>
            {
                new Option<string>(
                    new[] { "--src", "--s" },
                    getDefaultValue: () => "ssn.pdf",
                    description: "PDF template source"
                ),
                new Option<string>(
                    new[] { "--dest", "--d" },
                    getDefaultValue: () => "iron-ssn.pdf",
                    description: "Generated PDF destination"
                )
            }
        );

    static Command BuildIText() =>
        BuildCommand(
            "generate-itext",
            "Generate a PDF using iText",
            new Func<string, string, Task>(async (src, dest) =>
            {
                IGenerator generator = PdfGenerator.CreateITextGenerator(src, dest);
                Person person = Person.Generate();
                Record<Person> record = new(person);
                await generator.Generate(record);

                using IManager manager = PdfManager.CreateITextManager(dest);
                manager.ReadFields();
            }),
            new List<Option>
            {
                new Option<string>(
                    new[] { "--src", "--s" },
                    getDefaultValue: () => "ssn.pdf",
                    description: "PDF template source"
                ),
                new Option<string>(
                    new[] { "--dest", "--d" },
                    getDefaultValue: () => "itext-ssn.pdf",
                    description: "Generated PDF destination"
                )
            }
        );

    static Command BuildCommand(string name, string description, Func<string, string, Task> @delegate, List<Option> options)
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