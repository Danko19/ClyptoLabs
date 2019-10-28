using System;

namespace CryptoCommon
{
	public static class SetOperationsExtensions
	{
		public static byte[] Xor(this byte[] first, byte[] second)
		{
			if (first.Length != second.Length)
				throw new ArgumentException(
					$"Arrays have different sizes. First: {first.Length}, second {second.Length}");

			var result = new byte[first.Length];
			for (var i = 0; i < first.Length; i++)
			{
				result[i] = (byte)(first[i] ^ second[i]);
			}

			return result;
		}

		public static ulong Rotr(this ulong number, int shifts)
		{
			var copy = number << 64 - shifts % 64;
			return copy + number >> shifts;
		}

		public static ulong Xor(this ulong num1, ulong num2)
		{
			return num1 ^ num2;
		}
	}
}