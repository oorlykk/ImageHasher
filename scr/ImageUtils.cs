using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Imagehasher
{
	// Token: 0x02000002 RID: 2
	public static class ImageUtils
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static Bitmap ReduceImage(Image image, int width, int height)
		{
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = InterpolationMode.Low;
				graphics.SmoothingMode = SmoothingMode.HighSpeed;
				graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
				graphics.CompositingQuality = CompositingQuality.HighSpeed;
				graphics.DrawImage(image, 0, 0, width, height);
			}
			return bitmap;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B8 File Offset: 0x000002B8
		public static Bitmap GrayscaleImage(Bitmap bmp)
		{
			Bitmap bitmap = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);
			byte[] grayscaleBytes = ImageUtils.GetGrayscaleBytes(bmp);
			int num = 0;
			for (int i = 0; i < bmp.Height; i++)
			{
				for (int j = 0; j < bmp.Width; j++)
				{
					byte b = grayscaleBytes[num];
					Color color = Color.FromArgb((int)b, (int)b, (int)b);
					bitmap.SetPixel(j, i, color);
					num++;
				}
			}
			return bitmap;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002130 File Offset: 0x00000330
		public static byte[] GetGrayscaleBytes(Bitmap bmp)
		{
			byte[] array = new byte[bmp.Height * bmp.Width];
			int num = 0;
			for (int i = 0; i < bmp.Height; i++)
			{
				for (int j = 0; j < bmp.Width; j++)
				{
					Color pixel = bmp.GetPixel(j, i);
					byte b = (byte)((double)pixel.R * 0.3 + (double)pixel.G * 0.59 + (double)pixel.B * 0.11);
					array[num] = b;
					num++;
				}
			}
			return array;
		}
	}
}
