![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/porrey/PdfDocuments/.github%2Fworkflows%2Fbuild-and-test.yml?style=for-the-badge&label=Build%20and%20Test) ![GitHub License](https://img.shields.io/github/license/porrey/PdfDocuments?style=for-the-badge) ![.NET](https://img.shields.io/badge/.NET-10-purple?style=for-the-badge)

# PdfDocuments
High level C# library for the easy creation of PDF documents using **PdfSharp**. Use this library to create beautiful and complex PDFs using a structure similar to XAML with model binding. Objects called **PdfSections** are aligned to a grid by stacking or creating a parent/child hierarchy. The rendering engine flows the sections and aligns them to the grid, creating perfect PDF documents every time.

## Table of Contents

- [NuGet Packages](#nuget-packages)
- [Examples](#examples)
- [Quick Start](#quick-start)
  - [One-Time Setup](#one-time-setup)
  - [Define a Data Model](#define-a-data-model)
  - [Create a PDF Generator](#create-a-pdf-generator)
  - [Generate the PDF](#generate-the-pdf)
- [Section Types](#section-types)
- [Fluent Style Builder](#fluent-style-builder)
- [Data Binding](#data-binding)
- [Conditional Rendering](#conditional-rendering)
- [Font Resolvers](#font-resolvers)
- [Dependency Injection](#dependency-injection)
- [Barcode Integration](#barcode-integration)
- [Debug Mode](#debug-mode)

---

## NuGet Packages

| Package | Description |
|---|---|
| **[PdfDocuments](https://www.nuget.org/packages/PdfDocuments)** | Core library for PDF document generation |
| **[PdfDocuments.FontResolver.Windows](https://www.nuget.org/packages/PdfDocuments.FontResolver.Windows)** | Font resolver using installed Windows system fonts |
| **[PdfDocuments.FontResolver.Folder](https://www.nuget.org/packages/PdfDocuments.FontResolver.Folder)** | Font resolver loading fonts from a local folder (Linux/Docker friendly) |
| **[PdfDocuments.IronBarcode](https://www.nuget.org/packages/PdfDocuments.IronBarcode)** | Optional barcode/QR code section powered by IronBarcode |

Install the core package:

```
PM> Install-Package PdfDocuments
```

---

## Examples

### Invoice Example

![Example PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Invoice.png)

[View Actual PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Invoice.pdf)

The library has debug flags to assist in troubleshooting layout issues while developing.

### Invoice Debug Version

![Debug Version](https://github.com/porrey/PdfDocuments/raw/main/Images/Invoice-Debug.png)

[View Actual PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Invoice-Debug.pdf)

---

## Quick Start

The full source for the simple example can be found in the [Examples](https://github.com/porrey/PdfDocuments/tree/main/Src/Examples) folder of the project source code.

### One-Time Setup

Before generating any PDF, register an encoding provider and configure the font resolver. On Windows, use the Windows font resolver. On Linux or Docker, use the folder font resolver and point it at a directory containing `.ttf` / `.otf` files.

```csharp
using System.Text;
using PdfSharp.Fonts;

// Register an encoding provider (required by PdfSharp)
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Windows: use system-installed fonts
GlobalFontSettings.FontResolver = new FontResolver.Windows.FontResolver();

// Set the default fallback font
GlobalPdfDocumentsSettings.DefaultFontName = "Arial";
```

### Define a Data Model

Every PDF document is driven by a data model. The model must implement `IPdfModel`, which requires a single `Id` string property.

Create **[Message.cs](https://github.com/porrey/PdfDocuments/blob/main/Src/Examples/PdfDocuments.Example.Simple/Message.cs)**:

```csharp
public class Message : IPdfModel
{
    public string Id { get; set; }
    public string Text { get; set; }
}
```

### Create a PDF Generator

Create **[HelloWorld.cs](https://github.com/porrey/PdfDocuments/blob/main/Src/Examples/PdfDocuments.Example.Simple/HelloWorld.cs)** by extending `PdfGenerator<TModel>` and overriding two lifecycle methods:

- `OnInitializeStylesAsync` — define named styles
- `OnAddContentAsync` — build and return the root section tree

```csharp
using PdfSharp.Drawing;

public class HelloWorld : PdfGenerator<Message>
{
    public HelloWorld(IPdfStyleManager<Message> styleManager)
        : base(styleManager)
    {
    }

    protected override Task OnInitializeStylesAsync(IPdfStyleManager<Message> styleManager)
    {
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
        return Task.FromResult(Pdf.TextBlockSection<Message>()
            .WithText((g, m) => m.Text)
            .WithStyles("HelloWorld.Text")
            .WithStyleManager(this.StyleManager));
    }
}
```

`OnInitializeStylesAsync` registers a named style. The style name can be anything — it only needs to match the name passed to `.WithStyles(...)` on the section that uses it.

`OnAddContentAsync` returns the root section. Here a single `TextBlockSection` is created whose `Text` property is bound to the `Text` property of the `Message` model at render time.

### Generate the PDF

Open **[Program.cs](https://github.com/porrey/PdfDocuments/blob/main/Src/Examples/PdfDocuments.Example.Simple/Program.cs)** and wire everything together:

```csharp
// Create the style manager and the generator
PdfStyleManager<Message> styleManager = new();
HelloWorld helloWorld = new(styleManager);

// Create the data model
Message model = new() { Id = "12345", Text = "Hello World" };

// Generate the PDF as a byte array — no disk I/O required
(bool success, byte[] pdfData) = await helloWorld.BuildAsync(model);

if (success)
{
    // Save to disk
    string path = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
        $"{model.GetType().Name} [{model.Id}].pdf");

    File.WriteAllBytes(path, pdfData);

    // Open in the default PDF viewer
    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
}
```

`BuildAsync` returns a `(bool success, byte[] pdfData)` tuple. The byte array can be saved to disk, returned from a REST API, or streamed directly — no temporary file is ever required.

> **Shortcut:** Use `SaveAndOpenPdfAsync(model)` to generate, save, and open the PDF in one call (useful during development).

If everything worked, the PDF should look like this:

![Simple PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Message.png)

[View Actual PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Message.pdf)

---

## Section Types

Sections are the composable building blocks of every document. Nest them in vertical and horizontal stacks to create complex layouts.

| Section | Factory Method | Description |
|---|---|---|
| **PdfVerticalStackSection** | `Pdf.VerticalStackSection<M>(children…)` | Arranges child sections top-to-bottom |
| **PdfHorizontalStackSection** | `Pdf.HorizontalStackSection<M>(children…)` | Arranges child sections left-to-right |
| **PdfOverlayStackSection** | `Pdf.OverlayStackSection<M>(children…)` | Overlays sections on top of each other |
| **PdfTextBlockSection** | `Pdf.TextBlockSection<M>()` | Renders a single line of styled text |
| **PdfWrappingTextSection** | `Pdf.WrappingTextSection<M>()` | Renders text with automatic word-wrap |
| **PdfStackedTextSection** | `Pdf.StackedTextSection<M>()` | Renders multiple text lines vertically |
| **PdfImageSection** | `Pdf.ImageSection<M>()` | Renders an image from a file path |
| **PdfPageHeaderSection** | `Pdf.PageHeaderSection<M>()` | Header with optional logo and title |
| **PdfPageFooterSection** | `Pdf.PageFooterSection<M>()` | Footer with four bindable text corners |
| **PdfDataGridSection** | `Pdf.DataGridSection<M, TItem>()` | Tabular data grid with typed columns |
| **PdfDataRowsSection** | `Pdf.DataRowsSection<M, TItem>()` | Repeating data rows |
| **PdfKeyValueSection** | `Pdf.KeyValueSection<M>(items…)` | Key-value pair list |
| **PdfSignatureSection** | `Pdf.SignatureSection<M>()` | Signature area with optional image |
| **PdfContentSection** | `Pdf.ContentSection<M>(child)` | Wrapper for a single child section |
| **PdfHeaderContentSection** | `Pdf.HeaderContentSection<M>()` | Combined header and content area |
| **PdfSectionTemplate** | — | Base template for custom sections |
| **PdfEmptySection** | `Pdf.EmptySection<M>()` | Spacer / placeholder |
| **PdfBarcodeSection** *(add-on)* | `PdfBarcode.BarcodeSection<M>(…)` | Barcode/QR code (requires `PdfDocuments.IronBarcode`) |

### Composing a Layout

```csharp
protected override Task<IPdfSection<Invoice>> OnAddContentAsync()
{
    return Task.FromResult(
        Pdf.VerticalStackSection<Invoice>(
            Pdf.PageHeaderSection<Invoice>()
                .WithLogo((g, m) => "./Images/logo.png")
                .WithTitle((g, m) => "INVOICE")
                .WithStyles("Header")
                .WithStyleManager(this.StyleManager),

            Pdf.HorizontalStackSection<Invoice>(
                Pdf.KeyValueSection<Invoice>(
                    new PdfKeyValueItem<Invoice> { Key = "Invoice #", Value = (g, m) => m.Id },
                    new PdfKeyValueItem<Invoice> { Key = "Date",      Value = (g, m) => m.InvoiceDate.ToShortDateString() }
                ).WithStyles("KeyValue").WithStyleManager(this.StyleManager),

                Pdf.EmptySection<Invoice>()
            ).WithStyles("Row").WithStyleManager(this.StyleManager),

            Pdf.DataGridSection<Invoice, InvoiceItem>()
                .UseItems<Invoice, InvoiceItem>((g, m) => m.Items)
                .AddColumn<Invoice, InvoiceItem, int>(
                    "Qty",     i => i.Quantity,  0.1, "{0}",   "ColHeader", "ColCell")
                .AddColumn<Invoice, InvoiceItem, decimal>(
                    "Price",   i => i.UnitPrice,  0.2, "{0:C}", "ColHeader", "ColCell")
                .AddColumn<Invoice, InvoiceItem, decimal>(
                    "Amount",  i => i.Amount,     0.2, "{0:C}", "ColHeader", "ColCell")
                .WithStyles("DataGrid")
                .WithStyleManager(this.StyleManager),

            Pdf.PageFooterSection<Invoice>()
                .WithBottomLeftText((g, m) => "Thank you for your business.")
                .WithBottomRightText((g, m) => $"Page 1")
                .WithStyles("Footer")
                .WithStyleManager(this.StyleManager)
        )
    );
}
```

---

## Fluent Style Builder

Create named styles using `Style.Create<TModel>()` and the fluent builder chain.

```csharp
styleManager.Add("MyStyle", Style.Create<MyModel>()
    // Font
    .UseFont("Arial", 12, XFontStyleEx.Bold)

    // Colors
    .UseForegroundColor(XColors.White)
    .UseBackgroundColor(XColors.DarkBlue)
    .UseBorderColor(XColors.Black)

    // Borders
    .UseBorderWidth(1)

    // Spacing
    .UseMargin(5, 5, 5, 5)        // left, top, right, bottom
    .UsePadding(5, 5, 5, 5)
    .UseCellPadding(2, 2, 2, 2)   // for data grid cells

    // Sizing
    .UseRelativeHeight(0.1)        // fraction of available height
    .UseRelativeWidths(0.5, 0.5)   // column width fractions

    // Text alignment
    .UseTextAlignment(XStringFormats.Center)
    .UseParagraphAlignment(XParagraphAlignment.Center)

    .Build());
```

**Copy and modify an existing style:**

```csharp
styleManager.Add("MyStyleBold", Style.Copy(baseStyle)
    .UseFont("Arial", 12, XFontStyleEx.Bold)
    .Build());
```

All builder methods also accept a lambda `(PdfGridPage grid, TModel model) => value` for model-driven, dynamic style properties:

```csharp
.UseForegroundColor((g, m) => m.IsOverdue ? XColors.Red : XColors.Black)
.UseFont((g, m) => new XFont("Arial", m.FontSize))
```

---

## Data Binding

Text and style properties accept a `BindProperty<TValue, TModel>` which resolves at render time. Three forms are supported:

```csharp
// 1. Static value (implicit conversion)
BindProperty<string, MyModel> text = "Hello";

// 2. Model-driven lambda
BindProperty<string, MyModel> text = (grid, model) => model.Title;

// 3. Used inline with section methods
Pdf.TextBlockSection<MyModel>()
    .WithText((g, m) => $"Invoice #{m.Id}")
```

---

## Conditional Rendering

Use `.WithRenderCondition(...)` to show or hide a section based on model data:

```csharp
Pdf.TextBlockSection<Invoice>()
    .WithText((g, m) => "PAID")
    .WithRenderCondition((g, m) => m.Paid)
    .WithStyles("PaidStamp")
    .WithStyleManager(this.StyleManager)
```

---

## Font Resolvers

PdfSharp requires a font resolver to be registered before any PDF is generated. Choose the one that fits your deployment environment.

### Windows Font Resolver

Resolves fonts from the Windows system font registry. Ideal for Windows desktop and server applications.

```
PM> Install-Package PdfDocuments.FontResolver.Windows
```

**Manual setup:**

```csharp
GlobalFontSettings.FontResolver = new FontResolver.Windows.FontResolver();
```

**Dependency injection setup:**

```csharp
services.AddWindowsFontResolver();
```

### Folder Font Resolver

Resolves fonts from a local directory containing `.ttf` or `.otf` files. Ideal for Linux, Docker, and cross-platform environments.

```
PM> Install-Package PdfDocuments.FontResolver.Folder
```

**Manual setup:**

```csharp
GlobalFontSettings.FontResolver = new FontResolver.Folder.FontResolver("./Fonts");
```

**Dependency injection setup:**

```csharp
services.AddFolderFontResolver("./Fonts");
```

---

## Dependency Injection

The library integrates cleanly with `Microsoft.Extensions.DependencyInjection`. Register the core services and your generators in `ConfigureServices`:

```csharp
services.AddPdfDocuments()                          // registers IPdfGeneratorFactory
        .AddScoped<IPdfGenerator, InvoicePdf>()     // your custom generator
        .AddPdfStyleManager<Invoice>();             // style manager for Invoice model
```

Inject `IPdfGeneratorFactory` wherever PDF generation is needed. The factory resolves the correct generator from the DI container by matching the `TModel` type parameter to the registered `IPdfGenerator` implementation (e.g. `InvoicePdf` is matched when `TModel` is `Invoice` because `InvoicePdf` extends `PdfGenerator<Invoice>`):

```csharp
public class InvoiceService
{
    private readonly IPdfGeneratorFactory _factory;

    public InvoiceService(IPdfGeneratorFactory factory) => _factory = factory;

    public async Task<byte[]> GenerateAsync(Invoice invoice)
    {
        IPdfGenerator<Invoice> generator = await _factory.GetAsync<Invoice>();
        (bool success, byte[] pdf) = await generator.BuildAsync(invoice);
        return success ? pdf : null;
    }
}
```

See the [Invoice example](https://github.com/porrey/PdfDocuments/tree/main/Src/Examples/PdfDocuments.Example.Invoice) for a full hosted-service integration using Serilog and `Microsoft.Extensions.Hosting`.

---

## Barcode Integration

The optional `PdfDocuments.IronBarcode` package adds barcode and QR code rendering.

```
PM> Install-Package PdfDocuments.IronBarcode
```

Set your IronBarcode license key once at startup (skip for development/trial):

```csharp
PdfBarcode.SetLicense("YOUR-IRONBARCODE-LICENSE-KEY");
```

Add a barcode section anywhere in your document tree:

```csharp
// QR code bound to model data
PdfBarcode.BarcodeSection<MyModel>(
    data: (g, m) => m.TrackingNumber,
    barcodeEncoding: BarcodeEncoding.QRCode)
    .WithStyles("Barcode")
    .WithStyleManager(this.StyleManager)

// Barcode with custom size multipliers
PdfBarcode.BarcodeSection<MyModel>(
    data: (g, m) => m.Sku,
    barcodeEncoding: BarcodeEncoding.Code128,
    heightMultiplier: (g, m) => 0.8,
    widthMultiplier: (g, m) => 1.0)
    .WithStyles("Barcode")
    .WithStyleManager(this.StyleManager)
```

---

## Debug Mode

In more complex documents it can be helpful to visualize the grid and section outlines. Set debug flags on the generator before calling `BuildAsync`:

```csharp
helloWorld.DebugMode = helloWorld.DebugMode
    .SetFlag(DebugMode.RevealGrid, true)       // draw the grid lines
    .SetFlag(DebugMode.RevealLayout, true)     // draw section bounding boxes
    .SetFlag(DebugMode.HideDetails, false)     // show content inside sections
    .SetFlag(DebugMode.RevealFontDetails, true) // overlay font name and size
    .SetFlag(DebugMode.OutlineText, false);    // draw text outlines
```

The available flags are:

| Flag | Effect |
|---|---|
| `RevealGrid` | Draws the underlying grid lines |
| `RevealLayout` | Draws a bounding box around each section |
| `HideDetails` | Hides section content (shows layout only) |
| `RevealFontDetails` | Overlays font name and size on text sections |
| `OutlineText` | Renders text with an outline stroke |

The result with grid and layout revealed:

![Simple Debug PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Message-Debug.png)

[View Actual PDF](https://github.com/porrey/PdfDocuments/raw/main/Images/Message-Debug.pdf)
