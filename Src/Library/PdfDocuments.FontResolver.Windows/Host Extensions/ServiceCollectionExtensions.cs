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
using Microsoft.Extensions.DependencyInjection;
using PdfSharp.Fonts;

namespace PdfDocuments.FontResolver.Windows
{
	/// <summary>
	/// Provides extension methods for configuring font resolution services in a Windows environment.
	/// </summary>
	/// <remarks>This class contains static methods that extend the functionality of the IServiceCollection
	/// interface, enabling the registration of Windows-specific font resolver implementations. Use these methods to
	/// integrate font resolution capabilities into dependency injection setups.</remarks>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Registers a Windows-specific font resolver implementation in the dependency injection container.
		/// </summary>
		/// <remarks>Use this method when running on Windows to enable font resolution using system fonts. The
		/// registered font resolver will be used for font-related operations throughout the application.</remarks>
		/// <param name="services">The service collection to which the font resolver will be added. Cannot be null.</param>
		/// <returns>The updated service collection with the Windows font resolver registered.</returns>
		public static IServiceCollection AddWindowsFontResolver(this IServiceCollection services)
		{
			services.AddSingleton<IFontResolver, FontResolver>();
			return services;
		}
	}
}
