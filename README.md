# PdfDocuments
High level C# library for the easy creation of PDF documents using **PdfSharp**. Use this library to create beautiful and complex PDF's using a structure similar to XAML with model binding. Objects called PdfSections, are aligned to a grid allowing easy alignment of objects, flow and resizing capabilities.

## Examples

### Invoice Example

![Example PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Invoice.png)

[View Actual PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Invoice.pdf)

The library has debug flags to assist in troubleshooting layout issues while developing.

### Invoice Debug Version

![Debug Version](https://github.com/porrey/PdfDocuments/raw/main/Images/Invoice-Debug.png)

[View Actual PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Invoice-Debug.pdf)

## Simple Code Example

The source for the simple example can be in the [Examples](https://github.com/porrey/PdfDocuments/tree/main/Src/PDF-Documents-Solution/Examples) folder of the project source code.

### Add the NuGet package

Open Visual Studio and create a new Console application. Add the NuGet package.

```
PM> Install-Package PdfDocuments
```

### Add the using statement

In Visual Studio, open the **[Program.cs](https://github.com/porrey/PdfDocuments/blob/main/Src/PDF-Documents-Solution/Examples/PdfDocuments.Example.Simple/Program.cs)** file and add the using statement for the library.

```
using PdfDocuments;
```

### Change Main method Signature

This example uses **async/await** statements. This requires the main method signature to be changed as shown below.

```
static async Task<int> Main(string[] args)
```

### Register Encoding

Using the [PdfSharp](http://www.pdfsharp.com/PDFsharp/) library requires an encoding provider to be registered. Add the following code to the Main method in **Program.cs**.

```
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
```

### Create a Data Model

The PDF Documents in this library are based on data models. The PDF document uses binding to retrieve data from the instance of the model passed when building the PDF document.

Create a new class in the solution called **[Message.cs](https://github.com/porrey/PdfDocuments/blob/main/Src/PDF-Documents-Solution/Examples/PdfDocuments.Example.Simple/Message.cs)** and paste the code shown below into the class.

	namespace PdfDocuments.Example.Simple
	{
		public class Message : IPdfModel
		{
			public string Id { get; set; }
			public string Text { get; set; }
		}
	}

### Create PDF Document Generator Class

Create a new class call **[HelloWorld.cs](https://github.com/porrey/PdfDocuments/blob/main/Src/PDF-Documents-Solution/Examples/PdfDocuments.Example.Simple/HelloWorld.cs)** in the solution and copy the code below to the class.


	using PdfSharp.Drawing;
	using System.Threading.Tasks;
	
	namespace PdfDocuments.Example.Simple
	{
		public class HelloWorld : PdfGenerator<Message>
		{
			public HelloWorld(IPdfStyleManager<Message> styleManager)
				: base(styleManager)
			{
			}
	
			protected override Task OnInitializeStylesAsync(IPdfStyleManager<Message> styleManager)
			{
				//
				// Add a style for the text. The style name is referenced in the
				// content creation.
				//
				this.StyleManager.Add("HelloWorld.Text", Style.Create<Message>()
							.UseFont("Arial", 48)
							.UseForegroundColor(XColors.Purple)
							.UseBorderWidth(1)
							.UseTextAlignment(XStringFormats.Center)
							.UsePadding(10, 10, 10, 10)
							.Build());
	
				return Task.CompletedTask;
			}
	
			protected override Task<IPdfSection<Message>> OnAddContentAsync()
			{
				//
				// Add a basic text block using the style that was created.
				//
				return Task.FromResult(Pdf.TextBlockSection<Message>()
										  .WithText((g, m) => m.Text)
										  .WithStyles("HelloWorld.Text"));
			}
		}
	}

The class above overrides `OnInitializeStylesAsync` to create a single style that is used to style the text. The name of the styles does not matter as long as the style name is referenced with the same name in the section referencing the style.

the class also overrides `OnAddContentAsync()`. This method is where all of the content for the PDF is added. In this method, there is one section being added using the style that was defined previously. The **Text** property of the section is bound to the **Text** property of the **Message** model.

### Generate the PDF

Open the **Program.cs** file again and add a line to create a new Style Manager instance as shown below.

```
PdfStyleManager<Message> styleManager = new PdfStyleManager<Message>();
```

Next add a statement to create an instance of the ***HelloWorld*** PDF document as shown below. Notice the Style manager instance is passed into the constructor.

```
HelloWorld helloWorld = new HelloWorld(styleManager);
```

Next, create an instance of the data model.

```
Message model = new Message() { Id = "12345", Text = "Hello World" };
```

Now we can generate a PDF. The **Build()** method will create the PDF into a byte array. This byte array can be saved to disk or returned by a REST API to a client application without the need to write to disk first.

```
(bool result, byte[] fileData) = await generator.Build(model);
```

The Build method return a Tuple where the first element is a Boolean and indicates if the PDF was successfully created or not. If successful, the second element contains the PDF byte array.

Let's write the PDF to disk. The code below will save the PDF to the desktop folder of the current user using the model type name and Id property of the model to name the file.

	string fileName = $@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\{model.GetType().Name} [{model.Id}].pdf";
	File.WriteAllBytes(fileName, fileData);

Last, we can launch the default OS PDF viewer to view the new PDF document.

```
Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
```

If everything worked, the PDF should look like this:

![Simple PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Message.png)

[View Actual PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Message.pdf)

### Troubleshooting the Layout

In more complex documents, it may be necessary to troubleshoot layout and fonts. There are a few debug flags that can be set to reveal the details of how the document was rendered.

	helloWorld.DebugMode = helloWorld.DebugMode
			.SetFlag(DebugMode.RevealGrid, true)
			.SetFlag(DebugMode.RevealLayout, true)
			.SetFlag(DebugMode.HideDetails, false)
			.SetFlag(DebugMode.RevealFontDetails, true)
			.SetFlag(DebugMode.OutlineText, false);


In the above code, the PDF renderer will draw the grid, the section outlines and the fonts used. This result in the PDF shown below.

![Simple Debug PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Message-Debug.png)

[View Actual PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Message-debug.pdf)
