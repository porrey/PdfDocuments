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

namespace PdfDocuments.FontResolver.Folder
{
	/// <summary>
	/// Provides extension methods for configuring font resolvers in an IServiceCollection.
	/// </summary>
	/// <remarks>These extension methods enable registration of custom font resolvers for dependency injection
	/// scenarios. Use these methods to add font resolution capabilities to services in your application's DI
	/// container.</remarks>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Registers a font resolver that loads fonts from the specified folder into the service collection.
		/// </summary>
		/// <remarks>Use this method to enable font resolution from a custom folder when rendering documents or
		/// graphics. The registered font resolver will provide fonts from the specified directory for dependent
		/// services.</remarks>
		/// <param name="services">The service collection to which the font resolver will be added.</param>
		/// <param name="path">The file system path to the folder containing font files. Cannot be null or empty.</param>
		/// <returns>The updated service collection with the folder font resolver registered.</returns>
		public static IServiceCollection AddFolderFontResolver(this IServiceCollection services, string path)
		{
			services.AddSingleton<IFontResolver>(new FontResolver(path));
			return services;
		}
	}
}
