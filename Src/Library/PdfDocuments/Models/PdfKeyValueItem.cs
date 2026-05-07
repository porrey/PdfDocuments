/*
 *	MIT License
 *
 *	Copyright (c) 2021-2026 Daniel Porrey
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */
namespace PdfDocuments
{
	/// <summary>
	/// Represents a key-value pair for PDF data binding, where the value can be a static string or a bindable property
	/// associated with a model.
	/// </summary>
	/// <remarks>Use this class to define key-value pairs for PDF generation scenarios, supporting both static and
	/// dynamic values. The value can be bound to a property or action on the model, enabling flexible data extraction for
	/// PDF fields.</remarks>
	/// <typeparam name="TModel">The type of model associated with the value binding. Must implement the IPdfModel interface.</typeparam>
	public class PdfKeyValueItem<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the PdfKeyValueItem class.
		/// </summary>
		public PdfKeyValueItem()
		{
		}

		/// <summary>
		/// Initializes a new instance of the PdfKeyValueItem class with the specified key and value.
		/// </summary>
		/// <param name="key">The key associated with the item. Cannot be null.</param>
		/// <param name="value">The value associated with the key. Cannot be null.</param>
		public PdfKeyValueItem(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		/// <summary>
		/// Initializes a new instance of the PdfKeyValueItem class with the specified key and value binding.
		/// </summary>
		/// <param name="key">The key associated with the value. Cannot be null.</param>
		/// <param name="value">A binding that provides the value associated with the key. Cannot be null.</param>
		public PdfKeyValueItem(BindPropertyAction<string, TModel> key, BindProperty<string, TModel> value)
		{
			this.Key = key;
			this.Value = value;
		}

		/// <summary>
		/// Initializes a new instance of the PdfKeyValueItem class with the specified key and value binding action.
		/// </summary>
		/// <param name="key">The key associated with the item. Cannot be null.</param>
		/// <param name="value">A delegate that defines how to bind the value for the specified model. Cannot be null.</param>
		public PdfKeyValueItem(string key, BindPropertyAction<string, TModel> value)
		{
			this.Key = key;
			this.Value = value;
		}

		/// <summary>
		/// Initializes a new instance of the PdfKeyValueItem class with the specified key binding and value.
		/// </summary>
		/// <param name="key">A delegate that binds a property representing the key from the model. Cannot be null.</param>
		/// <param name="value">The value associated with the key. Can be null or empty.</param>
		public PdfKeyValueItem(BindPropertyAction<string, TModel> key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		/// <summary>
		/// Gets or sets the key value associated with the model.
		/// </summary>
		/// <remarks>The key is typically used to uniquely identify the model instance within a collection or data
		/// store. Ensure that the key value is unique within its context to avoid conflicts.</remarks>
		public BindProperty<string, TModel> Key { get; set; }

		/// <summary>
		/// Gets or sets the bound value for the property, supporting two-way data binding between the view and the model.
		/// </summary>
		/// <remarks>Use this property to synchronize changes between the UI and the underlying model. The generic
		/// parameter specifies the model type associated with the binding.</remarks>
		public BindProperty<string, TModel> Value { get; set; }
	}
}
