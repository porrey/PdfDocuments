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
using Xunit;

namespace PdfDocuments.Tests.Services
{
	[Collection("FontTests")]
	public class ServiceCollectionExtensionsTests(FontFixture _)
	{
		[Fact]
		public void AddPdfDocuments_RegistersIPdfGeneratorFactory()
		{
			ServiceCollection services = new();

			services.AddPdfDocuments();

			ServiceProvider provider = services.BuildServiceProvider();
			IPdfGeneratorFactory factory = provider.GetRequiredService<IPdfGeneratorFactory>();
			Assert.NotNull(factory);
		}

		[Fact]
		public void AddPdfDocuments_ReturnsTheSameServiceCollection()
		{
			ServiceCollection services = new();

			IServiceCollection result = services.AddPdfDocuments();

			Assert.Same(services, result);
		}

		[Fact]
		public void AddPdfStyleManager_RegistersIPdfStyleManagerForModel()
		{
			ServiceCollection services = new();

			services.AddPdfStyleManager<NullModel>();

			ServiceProvider provider = services.BuildServiceProvider();
			IPdfStyleManager<NullModel> styleManager = provider.GetRequiredService<IPdfStyleManager<NullModel>>();
			Assert.NotNull(styleManager);
		}

		[Fact]
		public void AddPdfStyleManager_ReturnsTheSameServiceCollection()
		{
			ServiceCollection services = new();

			IServiceCollection result = services.AddPdfStyleManager<NullModel>();

			Assert.Same(services, result);
		}

		[Fact]
		public void AddPdfStyleManager_WithInstance_RegistersProvidedInstance()
		{
			ServiceCollection services = new();
			PdfStyleManager<NullModel> customManager = new();

			services.AddPdfStyleManager(customManager);

			ServiceProvider provider = services.BuildServiceProvider();
			IPdfStyleManager<NullModel> resolved = provider.GetRequiredService<IPdfStyleManager<NullModel>>();
			Assert.Same(customManager, resolved);
		}

		[Fact]
		public void AddPdfStyleManager_WithInstance_ReturnsTheSameServiceCollection()
		{
			ServiceCollection services = new();
			PdfStyleManager<NullModel> customManager = new();

			IServiceCollection result = services.AddPdfStyleManager(customManager);

			Assert.Same(services, result);
		}

		[Fact]
		public void AddPdfDocuments_FactoryIsTransient_CreatesNewInstanceEachTime()
		{
			ServiceCollection services = new();
			services.AddPdfDocuments();

			ServiceProvider provider = services.BuildServiceProvider();
			IPdfGeneratorFactory factory1 = provider.GetRequiredService<IPdfGeneratorFactory>();
			IPdfGeneratorFactory factory2 = provider.GetRequiredService<IPdfGeneratorFactory>();

			Assert.NotSame(factory1, factory2);
		}

		[Fact]
		public void AddPdfStyleManager_IsTransient_CreatesNewInstanceEachTime()
		{
			ServiceCollection services = new();
			services.AddPdfStyleManager<NullModel>();

			ServiceProvider provider = services.BuildServiceProvider();
			IPdfStyleManager<NullModel> m1 = provider.GetRequiredService<IPdfStyleManager<NullModel>>();
			IPdfStyleManager<NullModel> m2 = provider.GetRequiredService<IPdfStyleManager<NullModel>>();

			Assert.NotSame(m1, m2);
		}
	}
}
