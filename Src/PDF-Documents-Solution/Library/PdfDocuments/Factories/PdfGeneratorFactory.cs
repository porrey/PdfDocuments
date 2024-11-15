﻿/*
 *	MIT License
 *
 *	Copyright (c) 2021-2025 Daniel Porrey
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PdfDocuments
{
	public class PdfGeneratorFactory : IPdfGeneratorFactory
	{
		public PdfGeneratorFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		protected virtual IServiceProvider ServiceProvider { get; set; }

		public virtual Task<IPdfGenerator<TModel>> GetAsync<TModel>()
			where TModel : IPdfModel
		{
			IPdfGenerator<TModel> returnValue = null;

			IEnumerable<IPdfGenerator> items = this.ServiceProvider.GetRequiredService<IEnumerable<IPdfGenerator>>();
			IPdfGenerator item = items.Where(t => t.DocumentModelType == typeof(TModel)).SingleOrDefault();

			if (item != null)
			{
				returnValue = item as IPdfGenerator<TModel>;
			}
			else
			{
				throw new Exception($"A PDF generator for document type '{typeof(TModel).Name}' was not found.");
			}

			return Task.FromResult(returnValue);
		}
	}
}
