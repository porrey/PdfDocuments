using PdfSharp.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

/// <summary>
/// Represents an image with adjustable opacity for use in PDF documents.
/// </summary>
/// <remarks>Use this class to create images with transparency applied, suitable for embedding in PDF files. The
/// image is managed as a disposable resource and should be disposed when no longer needed to release underlying streams
/// and image data.</remarks>
public class PdfTransparentImage : DisposableObject
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PdfTransparentImage"/> class with the specified image stream and image object.
	/// </summary>
	/// <param name="stream">The memory stream containing the image data. Must not be null and should remain open for the lifetime of the
	/// <see cref="PdfTransparentImage"/> instance.</param>
	/// <param name="image">The XImage object representing the image to be used. Must not be null.</param>
	private PdfTransparentImage(MemoryStream stream, XImage image)
	{
		this.Stream = stream;
		this.XImage = image;
	}

	/// <summary>
	/// Gets or sets the underlying memory stream used for data storage or manipulation.
	/// </summary>
	protected MemoryStream Stream { get; set; }

	/// <summary>
	/// Gets the image associated with this instance.
	/// </summary>
	public XImage XImage { get; }

	/// <summary>
	/// Releases managed resources used by the object during disposal.
	/// </summary>
	/// <remarks>This method is called as part of the disposal pattern to clean up managed resources. It should be
	/// overridden to release any managed objects that require explicit disposal when the object is disposed.</remarks>
	protected override void OnDisposeManagedObjects()
	{
		this.XImage.Dispose();
		this.Stream.Dispose();
	}

	/// <summary>
	/// Creates a new instance of the <see cref="PdfTransparentImage"/> class from the specified image file, applying the given opacity
	/// to the image.
	/// </summary>
	/// <remarks>The method loads the image from the provided file path and applies the specified opacity to all
	/// pixels. The resulting image is suitable for use in PDF generation scenarios where transparency is
	/// required.</remarks>
	/// <param name="imagePath">The file path of the image to load. The file must be a valid image supported by the underlying image processing
	/// library.</param>
	/// <param name="opacity">The opacity level to apply to the image, where 0 represents full transparency and 1 represents full opacity. Values
	/// outside this range are clamped.</param>
	/// <returns>A <see cref="PdfTransparentImage"/> instance containing the loaded image with the specified opacity applied.</returns>
	public static PdfTransparentImage FromFile(string imagePath, float opacity)
	{
		opacity = Math.Clamp(opacity, 0f, 1f);

		using Image<Rgba32> source = Image.Load<Rgba32>(imagePath);

		source.ProcessPixelRows(accessor =>
		{
			for (int y = 0; y < accessor.Height; y++)
			{
				Span<Rgba32> row = accessor.GetRowSpan(y);

				for (int x = 0; x < row.Length; x++)
				{
					row[x].A = (byte)(row[x].A * opacity);
				}
			}
		});

		MemoryStream stream = new MemoryStream();
		source.SaveAsPng(stream);
		stream.Position = 0;

		XImage image = XImage.FromStream(stream);

		return new PdfTransparentImage(stream, image);
	}
}