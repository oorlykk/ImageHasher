using System;
using System.Drawing;

namespace Imagehasher
{
	public static class ImageHasher
	{

		static ImageHasher()
		{
			for (int i = 0; i < 256; i++)
			{
				ImageHasher.BitCounts8[i] = (byte)ImageHasher.CountSetBits(i);
			}
			for (int j = 0; j < 65536; j++)
			{
				int num = 0;
				for (int k = j; k > 0; k >>= 8)
				{
					num += (int)ImageHasher.BitCounts8[k & 255];
				}
				ImageHasher.BitCounts16[j] = (ushort)num;
			}
		}

		private static int CountSetBits(int n)
		{
			return ImageHasher.CountSetBits((ulong)((long)n));
		}

		private static int CountSetBits(ulong n)
		{
			int num = 0;
			while (n > 0UL)
			{
				if ((n & 1UL) == 1UL)
				{
					num++;
				}
				n >>= 1;
			}
			return num;
		}


		public static int ComputeHammingDistance(ulong n1, ulong n2)
		{
			ulong num = n1 ^ n2;
			return (int)(ImageHasher.BitCounts16[(int)(checked((IntPtr)((num & 18446462598732840960UL) >> 48)))] + ImageHasher.BitCounts16[(int)(checked((IntPtr)((num & 281470681743360UL) >> 32)))] + ImageHasher.BitCounts16[(int)(checked((IntPtr)((num & unchecked((ulong)-65536)) >> 16)))] + ImageHasher.BitCounts16[(int)(checked((IntPtr)(num & 65535UL)))]);
		}

		public static double ComputeSimilarity(ulong hash1, ulong hash2)
		{
			return (double)(64 - ImageHasher.ComputeHammingDistance(hash1, hash2)) * 0.015625;
		}

		public static ulong ComputeAverageHash(Image image)
		{
			ulong num = 0UL;
			int num2 = 0;
			int width = 8;
			int height = 8;
			byte[] grayscaleBytes;
			using (Bitmap bitmap = ImageHasherUtils.ReduceImage(image, width, height))
			{
				grayscaleBytes = ImageHasherUtils.GetGrayscaleBytes(bitmap);
			}
			foreach (byte b in grayscaleBytes)
			{
				num2 += (int)b;
			}
			num2 /= grayscaleBytes.Length;
			for (int j = 0; j < grayscaleBytes.Length; j++)
			{
				if ((int)grayscaleBytes[j] >= num2)
				{
					num |= 1UL << 63 - j;
				}
			}
			return num;
		}

		public static ulong ComputeDifferenceHash(Image image)
		{
			ulong num = 0UL;
			int num2 = 9;
			int height = 8;
			int num3 = 0;
			byte[] grayscaleBytes;
			using (Bitmap bitmap = ImageHasherUtils.ReduceImage(image, num2, height))
			{
				grayscaleBytes = ImageHasherUtils.GetGrayscaleBytes(bitmap);
			}
			for (int i = 0; i < grayscaleBytes.Length; i++)
			{
				if (i == 0 || (i + 1) % num2 != 0)
				{
					if (grayscaleBytes[i] > grayscaleBytes[i + 1])
					{
						num |= 1UL << 63 - num3;
					}
					num3++;
				}
			}
			return num;
		}

		private static byte[] BitCounts8 = new byte[256];

		private static ushort[] BitCounts16 = new ushort[65536];
	}
}
