# PdfDocuments — Architecture Guide

## Overview

**PdfDocuments** is a high-level C# library that simplifies professional PDF creation using [PdfSharp](https://docs.pdfsharp.net/) as the underlying rendering engine. Its core philosophy is a **declarative, model-driven approach inspired by XAML** — you describe *what* a document should look like using composable, hierarchical "sections," and the library handles layout, alignment, spacing, and multi-page flow automatically.

> Create beautiful and complex PDFs using a structure similar to XAML with model binding. Objects called `PdfSections` stack and create parent/child hierarchies, with a rendering engine that flows sections through a grid, creating perfect layouts every time.

---

## Repository Structure

```
PdfDocuments/
├── Images/                                      # README screenshots and diagrams
├── Src/
│   ├── Library/                                 # Core library projects
│   │   ├── PdfDocuments/                        # Main library (~63 source files)
│   │   ├── PdfDocuments.FontResolver.Windows/   # Windows system font resolver
│   │   ├── PdfDocuments.FontResolver.Folder/    # Custom folder-based font resolver
│   │   └── PdfDocuments.IronBarcode/            # Optional barcode integration add-on
│   ├── Examples/                                # Sample applications
│   │   ├── PdfDocuments.Example.Simple/         # Minimal HelloWorld example
│   │   ├── PdfDocuments.Example.Invoice/        # Full professional invoice example
│   │   └── PdfDocuments.Example.Shared/         # Shared example utilities
│   └── Tests/
│       └── PdfDocuments.Tests/                  # 16 unit test files (xUnit)
├── README.md
└── LICENSE
```

---

## Key Technologies

| Technology | Version | Role |
|---|---|---|
| **.NET** | 10.0 (`net10.0`) | Target framework |
| **PdfSharp** | 6.2.4 | Low-level PDF rendering engine |
| **Microsoft.Extensions.DependencyInjection** | 10.0.5 | Dependency injection support |
| **IronBarcode** | 2026.3.6 | Barcode generation (optional add-on) |
| **System.Drawing.Common** | 10.0.5 | Font handling on Windows |
| **xUnit** | 2.9.3 | Unit testing framework |
| **Coverlet** | 6.0.4 | Code coverage |

---

## Main Library Code Organization

The main library (`Src/Library/PdfDocuments/`) is divided into focused folders:

### `Interfaces/`

Core abstractions that everything else is built upon:

| Interface | Purpose |
|---|---|
| `IPdfModel` | All data models implement this; requires just an `Id` property |
| `IPdfSection<TModel>` | A renderable section; supports hierarchy, styles, and data binding |
| `IPdfGenerator<TModel>` | Abstract contract for a PDF generator |
| `IPdfStyleManager<TModel>` | Manages a named dictionary of `PdfStyle<TModel>` objects |
| `IPdfElement<TModel>` | Low-level rendering element |
| `IStyleBuilder<TModel>` | Fluent builder for creating styles |

### `Generators/`

- **`PdfGenerator<TModel>`** — The abstract base class you extend to create a document. Override these virtual lifecycle methods:
  - `OnInitializeStylesAsync()` — Define named styles here
  - `OnAddContentAsync()` — Build and return the root section tree
  - `OnSetPageGridAsync()` — Configure the grid (rows/columns)
  - `OnGetPageCountAsync()` — Multi-page logic
  - `OnGetDocumentTitleAsync()` — Set the document title
  - Call `BuildAsync(model)` → returns `Task<(bool success, byte[] pdfData)>`

### `Sections/` — 18 Section Types

Sections are the **composable building blocks** of any document:

| Category | Section Types |
|---|---|
| **Layout containers** | `PdfVerticalStackSection`, `PdfHorizontalStackSection`, `PdfOverlayStackSection`, `PdfSectionTemplate` |
| **Content** | `PdfTextBlockSection`, `PdfWrappingTextSection`, `PdfStackedTextSection`, `PdfImageSection`, `PdfKeyValueSection` |
| **Data** | `PdfDataGridSection<TModel, TItem>`, `PdfDataRowsSection` |
| **Page structure** | `PdfPageHeaderSection`, `PdfPageFooterSection`, `PdfHeaderContentSection` |
| **Special** | `PdfSignatureSection`, `PdfEmptySection`, `PdfContentSection` |

### `Models/`

Data structures that carry layout and styling information:

| Class | Purpose |
|---|---|
| `PdfStyle<TModel>` | Typography (font, colors, alignment), spacing (margin, padding), and borders |
| `PdfStyleManager<TModel>` | `Dictionary<string, PdfStyle<TModel>>` with built-in "Default" and "Debug" entries |
| `PdfGrid` / `PdfGridPage` | Grid system: rows, columns, widths, heights, and offsets |
| `PdfBounds`, `PdfSpacing`, `PdfPoint`, `PdfSize` | Geometric value types |
| `BindProperty<T, TModel>` | Enables conditional/data-bound style values |
| `DebugMode` | Flags enum for debug visualization |
| `GlobalPdfDocumentsSettings` | Global default settings (e.g., default font name) |

### `Decorators/`

Fluent API surface for building documents:

- **`Pdf`** — Static facade with 20+ factory methods:
  - `Pdf.TextBlockSection<M>()`
  - `Pdf.VerticalStackSection<M>(...)`
  - `Pdf.DataGridSection<M, TItem>(...)`, etc.
- **`Style`** — Fluent style builder:
  - `Style.Create<M>().UseFont(...).UsePadding(...).Build()`
- **Section extension methods** — `.WithText()`, `.WithStyles()`, `.WithStyleManager()`, etc.

### `Elements/`

- **`PdfTextElement<TModel>`** — Low-level text rendering element used internally by sections.

### `Factories/`

- **`PdfGeneratorFactory`** — Object creation helper for generators.

---

## How to Use the Library

### Step 1 — One-time Setup

```csharp
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
GlobalFontSettings.FontResolver = new FontResolver.Windows.FontResolver();
GlobalPdfDocumentsSettings.DefaultFontName = "Arial";
```

### Step 2 — Define a Data Model

```csharp
public class Invoice : IPdfModel
{
    public string Id { get; set; }
    public string CustomerName { get; set; }
    public List<InvoiceItem> Items { get; set; }
}
```

### Step 3 — Create a PDF Generator

```csharp
public class InvoicePdf : PdfGenerator<Invoice>
{
    public InvoicePdf(IPdfStyleManager<Invoice> styleManager)
        : base(styleManager) { }

    protected override Task OnInitializeStylesAsync(IPdfStyleManager<Invoice> styleManager)
    {
        styleManager.Add("Header", Style.Create<Invoice>()
            .UseFont("Arial", 18, XFontStyleEx.Bold)
            .UseForegroundColor(XColors.DarkBlue)
            .Build());

        return Task.CompletedTask;
    }

    protected override Task<IPdfSection<Invoice>> OnAddContentAsync()
    {
        return Task.FromResult(
            Pdf.VerticalStackSection<Invoice>(
                Pdf.TextBlockSection<Invoice>()
                    .WithText((g, m) => m.CustomerName)
                    .WithStyles("Header")
                    .WithStyleManager(this.StyleManager)
            )
        );
    }
}
```

### Step 4 — Generate the PDF

```csharp
var styleManager = new PdfStyleManager<Invoice>();
var generator = new InvoicePdf(styleManager);
var model = new Invoice { Id = "1", CustomerName = "Acme Corp." };

(bool success, byte[] pdfBytes) = await generator.BuildAsync(model);

if (success)
{
    File.WriteAllBytes("invoice.pdf", pdfBytes);
}
```

### Fluent Style Building

```csharp
Style.Create<MyModel>()
    .UseFont("Arial", 12, XFontStyleEx.Bold)
    .UseForegroundColor(XColors.Blue)
    .UseBackgroundColor(XColors.LightYellow)
    .UseMargin(10, 10, 10, 10)
    .UsePadding(5, 5, 5, 5)
    .UseTextAlignment(XStringFormats.Center)
    .UseBorderWidth(1)
    .UseRelativeHeight(0.1)
    .Build()
```

### Data Binding

Text and style properties accept lambda expressions for fully dynamic, model-driven content:

```csharp
Pdf.TextBlockSection<MyModel>()
    .WithText((grid, model) => model.Title)
```

### Debug Mode

```csharp
generator.DebugMode = generator.DebugMode
    .SetFlag(DebugMode.RevealGrid, true)
    .SetFlag(DebugMode.RevealLayout, true)
    .SetFlag(DebugMode.RevealFontDetails, true);
```

---

## Optional Add-on Libraries

| Library | Purpose |
|---|---|
| **PdfDocuments.FontResolver.Windows** | Uses `System.Drawing` to auto-discover installed Windows system fonts |
| **PdfDocuments.FontResolver.Folder** | Points to a folder of `.ttf`/`.otf` files — ideal for Linux or Docker environments |
| **PdfDocuments.IronBarcode** | Adds barcode-rendering section types powered by IronBarcode |

---

## Example Projects

### `PdfDocuments.Example.Simple`

A minimal HelloWorld example demonstrating the core workflow:

- `Message.cs` — Model implementing `IPdfModel` with `Id` and `Text` properties
- `HelloWorld.cs` — Extends `PdfGenerator<Message>`; overrides `OnInitializeStylesAsync()` and `OnAddContentAsync()`
- `Program.cs` — Entry point; sets up fonts, encoding, style manager, and calls `BuildAsync()`

### `PdfDocuments.Example.Invoice`

A full professional invoice demonstrating advanced features:

- Complex multi-section layouts with nested vertical/horizontal stacks
- Custom fonts (Open Sans, Tinos) loaded from the `Fonts/` folder
- Color palettes and branding via a `ColorPalette` model
- `PdfDataGridSection` for invoice line items
- Header and footer sections repeated across pages
- Structured into `Models/`, `Pdf/`, `Hosted Services/`, `Images/`, and `Fonts/` sub-folders

---

## Testing

- **Framework:** xUnit with Coverlet code coverage
- **Location:** `Src/Tests/PdfDocuments.Tests/`
- **16 test files** organized into:
  - `Models/` — `PdfGridTests`, `PdfSizeTests`, `PdfSpacingTests`, `PdfBoundsTests`, `PdfPointTests`, `HslTests`, `NullModelTests`, `GlobalPdfDocumentsSettingsTests`
  - `Decorators/` — `PartitionExtensionsTests`, `PdfBoundsExtensionsTests`
  - `Services/` — Service-level tests
- Test infrastructure includes `FontFixture.cs` and `TestFontResolver.cs` for font setup

---

## Key Design Patterns

| Pattern | Where Used |
|---|---|
| **Composite** | Sections form a tree (root → child sections → leaf sections); the renderer walks the tree recursively |
| **Template Method** | `PdfGenerator<TModel>` provides the algorithm skeleton; subclasses fill in the virtual methods |
| **Fluent Builder** | `Style.Create<M>().UseFont(...).Build()` and section extension methods like `.WithText(...).WithStyles(...)` |
| **Data Binding** | Text and style properties accept `Func<PdfGrid, TModel, T>` lambdas for dynamic, model-driven content |
| **Grid Layout** | Documents are laid out on a configurable rows × columns grid; each section declares its relative height/width |
| **Strategy** | Font resolvers (`Windows`, `Folder`) are pluggable strategies injected via PdfSharp's `GlobalFontSettings` |
