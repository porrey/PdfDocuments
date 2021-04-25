using System;
using System.Drawing;

namespace PdfDocuments.Theme.Abstractions
{
	public class Hsl
	{
		public Hsl(double h, double s, double l)
		{
			this.H = h;
			this.S = s;
			this.L = l;
		}

		public double H { get; set; }
		public double S { get; set; }
		public double L { get; set; }

		public Color ToColor()
		{
			double v;
			double r, g, b;

			r = this.L;
			g = this.L;
			b = this.L;
			v = (this.L <= 0.5) ? (this.L * (1.0 + this.S)) : (this.L + this.S - this.L * this.S);
			if (v > 0)
			{
				double m;
				double sv;
				int sextant;
				double fract, vsf, mid1, mid2;

				m = this.L + this.L - v;
				sv = (v - m) / v;
				double hue = (this.H / 360.0) * 6.0;
				sextant = (int)hue;
				fract = hue - sextant;
				vsf = v * sv * fract;
				mid1 = m + vsf;
				mid2 = v - vsf;
				switch (sextant)
				{
					case 0:
						r = v;
						g = mid1;
						b = m;
						break;
					case 1:
						r = mid2;
						g = v;
						b = m;
						break;
					case 2:
						r = m;
						g = v;
						b = mid1;
						break;
					case 3:
						r = m;
						g = mid2;
						b = v;
						break;
					case 4:
						r = mid1;
						g = m;
						b = v;
						break;
					case 5:
						r = v;
						g = m;
						b = mid2;
						break;
				}
			}

			return Color.FromArgb(Convert.ToByte(r * 255.0), Convert.ToByte(g * 255.0), Convert.ToByte(b * 255.0));
		}

		public override string ToString()
		{
			return string.Format("{0:N1}, {1:P0}, {2:P0}", this.H, this.S, this.L);
		}
	}
}
