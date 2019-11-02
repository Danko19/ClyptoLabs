using System;

namespace Crypto_Lab4
{
	public static class ExtendedBitConverter
	{
		public static ulong ToUInt64(byte[] bytes, int startIndex)
		{
			if (startIndex < 0 || startIndex + 8 > bytes.Length)
				throw new ArgumentOutOfRangeException(nameof(startIndex));

			var endIndex = startIndex + 7;
			ulong result = 0;
			ulong multiplier = 1;
			for (var i = endIndex; i >= startIndex; i--)
			{
				result += bytes[i] * multiplier;
				multiplier <<= 8;
			}

			return result;
		}
	}
}