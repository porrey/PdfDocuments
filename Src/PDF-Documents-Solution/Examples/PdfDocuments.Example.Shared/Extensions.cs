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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PdfDocuments.Example
{
	public static class Extensions
	{
		public static async Task<bool> SaveAndOpenPdfAsync<TModel>(this IPdfGenerator<TModel> generator, TModel model)
			where TModel : IPdfModel
		{
			bool returnValue = false;

			//
			// Generate the PDF.
			//
			(bool result, byte[] fileData) = await generator.BuildAsync(model);

			if (result)
			{
				//
				// Save the PDF to the desktop.
				//
				string fileName = $@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\{model.GetType().Name} [{model.Id}].pdf";
				File.WriteAllBytes(fileName, fileData);

				//
				// Launch the system PDF viewer.
				//
				Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
				returnValue = true;
			}

			return returnValue;
		}
	}
}
