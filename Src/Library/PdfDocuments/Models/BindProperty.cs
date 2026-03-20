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
	/// Represents a method that binds a property from a PDF model to a grid page and returns a result of the specified
	/// type.
	/// </summary>
	/// <typeparam name="TResult">The type of the result returned by the binding operation.</typeparam>
	/// <typeparam name="TModel">The type of the PDF model to bind. Must implement the IPdfModel interface.</typeparam>
	/// <param name="gp">The PDF grid page to which the property will be bound.</param>
	/// <param name="m">The PDF model instance containing the property to bind.</param>
	/// <returns>The result of the binding operation, of type TResult.</returns>
	public delegate TResult BindPropertyAction<TResult, TModel>(PdfGridPage gp, TModel m)
		where TModel : IPdfModel;

	/// <summary>
	/// Represents a callback that modifies or processes a value of type TProperty using additional state information.
	/// </summary>
	/// <typeparam name="TProperty">The type of the value to be processed or modified by the callback.</typeparam>
	/// <param name="obj">The value to be processed or modified.</param>
	/// <param name="state">An additional state object that can be used to influence the processing of the value.</param>
	/// <returns>The processed or modified value of type TProperty.</returns>
	public delegate TProperty HookAction<TProperty>(TProperty obj, object state);

	/// <summary>
	/// Represents a bindable property for a PDF model, allowing dynamic resolution and optional transformation of property
	/// values within a PDF grid context.
	/// </summary>
	/// <remarks>BindProperty enables flexible property binding and transformation for PDF generation scenarios. The
	/// property value can be resolved based on the current grid page and model, and optionally modified using a hook
	/// action. This class supports both synchronous and asynchronous resolution, as well as implicit conversions for ease
	/// of use. Thread safety is not guaranteed; instances should not be shared across threads without external
	/// synchronization.</remarks>
	/// <typeparam name="TProperty">The type of the property value to be resolved and bound.</typeparam>
	/// <typeparam name="TModel">The type of the PDF model implementing the IPdfModel interface.</typeparam>
	public class BindProperty<TProperty, TModel>
		where TModel : IPdfModel
	{
		/// <summary>
		/// Initializes a new instance of the BindProperty class.
		/// </summary>
		public BindProperty()
		{
		}

		/// <summary>
		/// Initializes a new instance of the BindProperty class with the specified binding action.
		/// </summary>
		/// <param name="action">The binding action to associate with this property. Cannot be null.</param>
		public BindProperty(BindProperty<TProperty, TModel> action)
		{
			this.Action = action;
		}

		/// <summary>
		/// Initializes a new instance of the BindProperty class with the specified binding action.
		/// </summary>
		/// <param name="action">The delegate that defines the binding logic between the property and the model. Cannot be null.</param>
		public BindProperty(BindPropertyAction<TProperty, TModel> action)
		{
			this.Action = action;
		}

		/// <summary>
		/// Gets or sets the action to perform when binding a property to a model.
		/// </summary>
		/// <remarks>Use this property to specify custom logic for handling property binding operations. The action
		/// defines how the property value is transferred or processed during binding. Ensure that the action is compatible
		/// with the types of the property and model.</remarks>
		public BindPropertyAction<TProperty, TModel> Action { get; set; }

		/// <summary>
		/// Gets or sets the hook action to be executed when the property changes.
		/// </summary>
		public virtual HookAction<TProperty> Hook { get; set; }

		/// <summary>
		/// Resolves and returns a property value for the specified PDF grid page and model instance.
		/// </summary>
		/// <param name="g">The PDF grid page for which the property value is to be resolved.</param>
		/// <param name="m">The model instance used to determine the property value.</param>
		/// <param name="state">An optional state object that can be used to provide additional context for the resolution process. May be null.</param>
		/// <returns>The resolved property value for the given PDF grid page and model instance.</returns>
		public virtual TProperty Resolve(PdfGridPage g, TModel m, object state = null)
		{
			return this.OnResolve(g, m, state);
		}

		/// <summary>
		/// Asynchronously resolves the property value for the specified PDF grid page and model.
		/// </summary>
		/// <param name="g">The PDF grid page for which the property value is to be resolved.</param>
		/// <param name="m">The model instance used to resolve the property value.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the resolved property value.</returns>
		public virtual Task<TProperty> ResolveAsync(PdfGridPage g, TModel m)
		{
			return Task.FromResult(this.Resolve(g, m));
		}

		/// <summary>
		/// Sets the action used to bind a property to a model.
		/// </summary>
		/// <param name="action">The delegate that defines how the property is bound to the model. Cannot be null.</param>
		public virtual void SetAction(BindProperty<TProperty, TModel> action)
		{
			this.Action = action;
		}

		/// <summary>
		/// Sets the action to be performed when binding the property to the model.
		/// </summary>
		/// <param name="action">The delegate that defines the operation to execute during property binding. Cannot be null.</param>
		public virtual void SetAction(BindPropertyAction<TProperty, TModel> action)
		{
			this.Action = action;
		}

		/// <summary>
		/// Sets the binding action used to associate a property with a model asynchronously.
		/// </summary>
		/// <param name="action">The binding action that defines how the property of type TProperty is linked to the model of type TModel. Cannot
		/// be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task completes immediately after setting the action.</returns>
		public virtual Task SetActionAsync(BindProperty<TProperty, TModel> action)
		{
			this.Action = action;
			return Task.FromResult(0);
		}

		/// <summary>
		/// Sets the action to be performed when binding a property to a model asynchronously.
		/// </summary>
		/// <param name="action">The delegate that defines the binding action between the property and the model. Cannot be null.</param>
		/// <returns>A task that represents the asynchronous operation. The task completes immediately after the action is set.</returns>
		public virtual Task SetActionAsync(BindPropertyAction<TProperty, TModel> action)
		{
			this.Action = action;
			return Task.FromResult(0);
		}

		/// <summary>
		/// Resolves and returns a property value based on the specified grid page and model, optionally applying a hook for
		/// additional processing.
		/// </summary>
		/// <remarks>Override this method to customize how the property value is resolved or processed. If a hook is
		/// provided, it is invoked with the resolved value and the optional state object.</remarks>
		/// <param name="g">The grid page context used to resolve the property value.</param>
		/// <param name="m">The model instance from which the property value is resolved.</param>
		/// <param name="state">An optional state object passed to the hook for additional processing. May be null.</param>
		/// <returns>The resolved property value of type TProperty. If no action is defined, returns the default value for TProperty.</returns>
		protected virtual TProperty OnResolve(PdfGridPage g, TModel m, object state = null)
		{
			TProperty returnValue = default;

			if (this.Action != null)
			{
				returnValue = this.Action.Invoke(g, m);

				if (this.Hook != null)
				{
					returnValue = this.Hook.Invoke(returnValue, state);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Creates a new instance of BindProperty<![CDATA[<TProperty, TModel>]]> from the specified property value.
		/// </summary>
		/// <remarks>This implicit conversion allows a property value to be used directly as a BindProperty<![CDATA[<TProperty, TModel>]]>
		/// without explicit construction. The resulting BindProperty always returns the provided value, regardless of
		/// the model or property context.</remarks>
		/// <param name="source">The property value to bind. Cannot be null if TProperty is a non-nullable type.</param>
		public static implicit operator BindProperty<TProperty, TModel>(TProperty source)
		{
			return new BindProperty<TProperty, TModel>((gp, m) => source);
		}

		/// <summary>
		/// Converts a specified action delegate to a BindProperty instance, enabling property binding with the provided
		/// action.
		/// </summary>
		/// <remarks>Use this operator to simplify the creation of BindProperty instances from action delegates. This
		/// conversion allows for more concise property binding syntax.</remarks>
		/// <param name="model">The action delegate that defines how the property should be bound to the model. Cannot be null.</param>
		public static implicit operator BindProperty<TProperty, TModel>(BindPropertyAction<TProperty, TModel> model)
		{
			return new BindProperty<TProperty, TModel>()
			{
				Action = model
			};
		}

		/// <summary>
		/// Defines an implicit conversion from a BindProperty instance to a BindPropertyAction instance, enabling assignment
		/// without explicit casting.
		/// </summary>
		/// <remarks>This operator allows seamless usage of BindProperty objects in contexts where a
		/// BindPropertyAction is expected. If the source instance is null, the resulting BindPropertyAction will have a null
		/// Action property.</remarks>
		/// <param name="model">The BindProperty instance to convert. Cannot be null.</param>
		public static implicit operator BindPropertyAction<TProperty, TModel>(BindProperty<TProperty, TModel> model)
		{
			return new BindProperty<TProperty, TModel>()
			{
				Action = model
			};
		}
	}
}