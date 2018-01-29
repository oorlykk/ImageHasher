using System;
using System.Drawing;

namespace ImageHasher
{
	// Token: 0x02000003 RID: 3
	public static class ImageHasher
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000021C4 File Offset: 0x000003C4
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

		// Token: 0x06000005 RID: 5 RVA: 0x00002244 File Offset: 0x00000444
		private static int CountSetBits(int n)
		{
			return ImageHasher.CountSetBits((ulong)((long)n));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002250 File Offset: 0x00000450
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

		// Token: 0x06000007 RID: 7 RVA: 0x00002278 File Offset: 0x00000478
		public static int ComputeHammingDistance(ulong n1, ulong n2)
		{
			ulong num = n1 ^ n2;
			return (int)(ImageHasher.BitCounts16[(int)(checked((IntPtr)((num & 18446462598732840960UL) >> 48)))] + ImageHasher.BitCounts16[(int)(checked((IntPtr)((num & 281470681743360UL) >> 32)))] + ImageHasher.BitCounts16[(int)(checked((IntPtr)((num & unchecked((ulong)-65536)) >> 16)))] + ImageHasher.BitCounts16[(int)(checked((IntPtr)(num & 65535UL)))]);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022DB File Offset: 0x000004DB
		public static double ComputeSimilarity(ulong hash1, ulong hash2)
		{
			return (double)(64 - ImageHasher.ComputeHammingDistance(hash1, hash2)) * 0.015625;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022F4 File Offset: 0x000004F4
		public static ulong ComputeAverageHash(Image image)
		{
			ulong num = 0UL;
			int num2 = 0;
			int width = 8;
			int height = 8;
			byte[] grayscaleBytes;
			using (Bitmap bitmap = ImageUtils.ReduceImage(image, width, height))
			{
				grayscaleBytes = ImageUtils.GetGrayscaleBytes(bitmap);
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

		// Token: 0x0600000A RID: 10 RVA: 0x0000238C File Offset: 0x0000058C
		public static ulong ComputeDifferenceHash(Image image)
		{
			ulong num = 0UL;
			int num2 = 9;
			int height = 8;
			int num3 = 0;
			byte[] grayscaleBytes;
			using (Bitmap bitmap = ImageUtils.ReduceImage(image, num2, height))
			{
				grayscaleBytes = ImageUtils.GetGrayscaleBytes(bitmap);
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

		// Token: 0x04000001 RID: 1
		private static byte[] BitCounts8 = new byte[256];

		// Token: 0x04000002 RID: 2
		private static ushort[] BitCounts16 = new ushort[65536];
	}
}
