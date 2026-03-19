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

namespace PdfDocuments.IronBarcode
{
	/// <summary>
	/// Provides extension methods for configuring IronBarcode licensing within an ASP.NET Core dependency injection
	/// container.
	/// </summary>
	/// <remarks>This class enables the registration and configuration of IronBarcode license keys using the
	/// standard .NET dependency injection pattern. Use these methods to ensure IronBarcode is properly licensed before use
	/// in your application.</remarks>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Configures the IronBarcode library with the specified license key for use within the application's dependency
		/// injection container.
		/// </summary>
		/// <remarks>This method sets the global license key for IronBarcode. It should be called during application
		/// startup before using IronBarcode services.</remarks>
		/// <param name="services">The service collection to which IronBarcode configuration will be applied.</param>
		/// <param name="licenseKey">The license key used to activate IronBarcode features. Cannot be null or empty.</param>
		/// <returns>The same service collection instance, allowing for method chaining.</returns>
		public static IServiceCollection AddIronBarcode(this IServiceCollection services, string licenseKey)
		{
			IronBarCode.License.LicenseKey = licenseKey;
			return services;
		}
	}
}
