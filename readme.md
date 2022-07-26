# PDF Processing

This repository evaluates the ability for [IronPdf](https://ironpdf.com/) and [iText](https://itextpdf.com/) to inject C# object data into PDF form fields following a standardized convention. The PDF is generated from an embedded PDF with fields configured to a common naming convention: `{Type}.{Property}`.

To account for fields that set a property based on a range of values (SSN is a single value, but exposed as 9 numeric form fields), the index of each field is added to the end of the field name: `{Type}.{Property}.{Index}`. The value is set by using `prop.Value.ElementAt(Index)`.

To account for fields that provide several options for a single value (Gender is only Male or Female, but is exposed as two checkbox inputs), the value is added to the end of the field name: `{Type}.{Property}.{Value}`. The match is determined by the value of the property being equal to the value position in the name of the checkbox.

* [Scenario](#scenario)
* [Managers](#managers)
* [Generators](#generators)
* [CLI](#cli)
    * [Debugging](#vs-code-debugging)
    * [Tasks](#vs-code-tasks)

## Scenario
[Back to Top](#pdf-processing)

Available personnel data from a [Person](./Pdf.Models/Person.cs) object should be able to be auto-filled into a [Social Security Administration Application](./Pdf.Processor/files/ssn.pdf).

A generic [Record](./Pdf.Models/Record.cs) class has been created that uses reflection to enumerate through the properties of a class and generate a collection of [RecordProp](./Pdf.Models/RecordProp.cs) objects that facilitate mapping C# object data to PDF form fields.

## Managers
[Back to Top](#pdf-processing)

Before getting into generating PDFs, I wanted to be able to test some standard PDF capabilities using the libraries. This included:

* Initializing a PDF document from a provided path.
* Reading details about the form fields contained in the PDF.
* Updating the names of form fields.

The required functionality has been defined by the [IManager](./Pdf.Models/IManager.cs) interface. Common features between implementations have been encapsulated in the abstract [ManagerBase](./Pdf.Processor/ManagerBase.cs) class, which implements `IManager, IDisposable`.

Concrete implementations for each library are defined in the internal classes [IronManager](./Pdf.Processor/IronManager.cs) and [ITextManager](./Pdf.Processor/ITextManager.cs).

Acccess to the manager classes is exposed through the [ManagerFactory](./Pdf.Processor/ManagerFactory.cs) class.

## Generators
[Back to Top](#pdf-processing)

The intent of a generator is to:

* Initialize an instance of a PDF document from a provided source path at the provided destination path.

* Given a `Record<T>` object, map every instance of `RecordProp.Value` to a field that matches the `RecordProp.Map` name using the conventions established at the beginning of this document.

The required functionality has been defined by the [IGenerator](./Pdf.Models/IGenerator.cs) interface. Common features between implementations have been encapsulated in the abstract [GeneratorBase](./Pdf.Processor/GeneratorBase.cs) class, which implements `IGenerator`.

Concrete implementations for each library are defined in the internal classes [IronGenerator](./Pdf.Processor/IronGenerator.cs) and [ITextGenerator](./Pdf.Processor/ITextGenerator.cs).

Access to the generator classes is exposed through the [GeneratorFactory](./Pdf.Processor/GeneratorFactory.cs) class.

## CLI
[Back to Top](#pdf-processing)

A [System.CommandLine](https://docs.microsoft.com/en-us/dotnet/standard/commandline/) utility has been created in [Pdf.Cli](./Pdf.Cli/Commands.cs) to test the above features.

**Root Command Help**  

![image](https://user-images.githubusercontent.com/14102723/181103643-f353f0fd-50dd-43ce-984b-8d4fe1e3bc8d.png)

**`generate-iron` Help**  

![image](https://user-images.githubusercontent.com/14102723/181103732-a612e402-7887-4f17-b1bf-0040ffcb00ac.png)

**`generate-itext` Help**  

![image](https://user-images.githubusercontent.com/14102723/181103808-d04abe88-2118-4632-a2a9-48e0bd698ce6.png)

**Running `generate-iron`**  

![image](https://user-images.githubusercontent.com/14102723/181104165-9aa10da0-ba0d-4902-ac3c-f774ce7e7253.png)

**PDF Generated by `generate-iron`**  

![image](https://user-images.githubusercontent.com/14102723/181104603-4e40eca0-2c6f-4e21-9128-6c5ef7c26097.png)

**Running `generate-itext`**  

![image](https://user-images.githubusercontent.com/14102723/181104817-7fd93f4a-f94e-4a2b-99c1-6ccc9277038f.png)

**PDF Generated by `generate-itext`**  

![image](https://user-images.githubusercontent.com/14102723/181104992-88f18080-b704-48ba-aca7-c832c5589ff2.png)

### VS Code Debugging
[Back to Top](#pdf-processing)

[Debugging profiles](./.vscode/launch.json) for `generate-iron` and `generate-itext` are available via the Debug menu:

![image](https://user-images.githubusercontent.com/14102723/181105340-52285341-4355-4d22-8024-c754a74eba07.png)

### VS Code Tasks
[Back to Top](#pdf-processing)

[Tasks](./.vscode/tasks.json) for `generate-iron` and `generate-itext` are available.

They can be accessed by:

* Command Palette (<kbd>Ctrl + P</kbd>) and typing `task `:  

    ![image](https://user-images.githubusercontent.com/14102723/181105713-4fb73e24-bd77-4532-8d7f-14768f455fff.png)

* Installing the [Task Explorer](https://marketplace.visualstudio.com/items?itemName=spmeesseman.vscode-taskexplorer) extension:  

    ![image](https://user-images.githubusercontent.com/14102723/181105903-7b8acdd9-bc4f-410a-a67c-218f0e3aa8b9.png)

[Back to Top](#pdf-processing)