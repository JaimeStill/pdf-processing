using Pdf.Cli;
using System.CommandLine;

RootCommand root = Commands.Initialize();
await root.InvokeAsync(args);