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
using System.Linq.Expressions;

namespace PdfDocuments
{
	/// <summary>
	/// Represents a column definition for a data grid rendered in a PDF document, supporting binding to model properties
	/// and customizable formatting and styles.
	/// </summary>
	/// <remarks>Use this class to configure the appearance and behavior of individual columns in a PDF data grid,
	/// including header text, cell formatting, and style names. Each property supports binding to values from the model,
	/// enabling dynamic customization based on model data.</remarks>
	/// <typeparam name="TModel">The type of the model associated with the column. Must implement <see cref="IPdfModel"/>.</typeparam>
	public class PdfDataGridColumn<TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Gets or sets the name of the style applied to the header element.
		/// </summary>
		public virtual BindProperty<string, TModel> HeaderStyleName { get; set; }

		/// <summary>
		/// Gets or sets the name of the style applied to the data element for the model.
		/// </summary>
		public virtual BindProperty<string, TModel> DataStyleName { get; set; }

		/// <summary>
		/// Gets or sets the column header text for the bound model property.
		/// </summary>
		public virtual BindProperty<string, TModel> ColumnHeader { get; set; }

		/// <summary>
		/// Gets or sets the relative width of the section as a proportion of its container.
		/// </summary>
		/// <remarks>The value typically ranges from 0.0 to 1.0, where 1.0 represents full width. Setting this
		/// property allows dynamic sizing based on container dimensions.</remarks>
		public virtual BindProperty<double, TModel> RelativeWidth { get; set; }

		/// <summary>
		/// Gets or sets the format string used to display the bound value as text.
		/// </summary>
		/// <remarks>Specify a format string to control how the value is rendered. Common .NET format specifiers can
		/// be used. If not set, the value is displayed using its default string representation.</remarks>
		public virtual BindProperty<string, TModel> StringFormat { get; set; }

		/// <summary>
		/// Gets or sets the expression that represents access to a member, such as a property or field, within an expression
		/// tree.
		/// </summary>
		/// <remarks>Use this property to inspect or modify the member access represented in the expression tree.
		/// Changing this property affects how the expression tree references the underlying member.</remarks>
		public virtual MemberExpression MemberExpression { get; set; }
	}
}
