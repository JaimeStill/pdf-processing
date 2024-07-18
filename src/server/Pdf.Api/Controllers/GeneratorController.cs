using Microsoft.AspNetCore.Mvc;
using Pdf.Models;
using Pdf.Processor.Generator;

namespace Pdf.Api.Controllers;

[Route("api/[controller]")]
public class GeneratorController(IGenerator generator) : ControllerBase
{
    readonly IGenerator generator = generator;

    [HttpGet("[action]")]
    public IActionResult GeneratePerson() =>
        Ok(Person.Generate());

    [HttpPost("[action]")]
    public async Task<FileContentResult> GeneratePdf([FromBody] Person person)
    {
        Record<Person> record = new(person);

        string src = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "files",
            "ssn.pdf"
        );

        string dest = Path.Join(
            AppDomain.CurrentDomain.BaseDirectory,
            $"{Guid.NewGuid()}.pdf"
        );

        await generator.Generate(record, src, dest);

        byte[] bytes = await System.IO.File.ReadAllBytesAsync(dest);
        System.IO.File.Delete(dest);

        return new FileContentResult(bytes, "application/octet")
        {
            FileDownloadName = $"{person.FirstName.ToLower()}-{person.LastName.ToLower()}.pdf"
        };
    }
}