using System;
using System.Linq;
using Crypto_Lab4;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class Crypto_Lab4_Tests
	{
		[Test]
		public void SHA512PaddingStandardShouldReturnSingleBlock()
		{
			var data = new byte[] { 1, 2, 3 };
			var paddingStandard = new SHA512PaddingStandard();
			var result = paddingStandard.GetAlignedBlocks(data, 16, 1).ToArray();

			result.Length.Should().Be(1);
			var block = result.Single();
			block.Should().BeEquivalentTo(new byte[] { 1, 2, 3, 128 }
					.Concat(Enumerable.Repeat((byte)0, 11))
					.Concat(new byte[] { 19 * 8 })
					.ToArray(),
				options => options.WithStrictOrdering());
		}

		[Test]
		public void SHA512PaddingStandardShouldReturnTwoBlocks()
		{
			var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			var paddingStandard = new SHA512PaddingStandard();

			var result = paddingStandard
				.GetAlignedBlocks(data, 16, 0)
				.ToArray();

			result.Length.Should().Be(2);
			var firstBlock = result.First();
			var secondBlock = result.Last();
			firstBlock.Should().BeEquivalentTo(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 128 }
					.Concat(Enumerable.Repeat((byte)0, 7))
					.ToArray(),
				options => options.WithStrictOrdering());
			secondBlock.Should().BeEquivalentTo(Enumerable.Repeat((byte)0, 15)
					.Concat(new byte[] { 8 * 8 })
					.ToArray(),
				options => options.WithStrictOrdering());
		}

		[Test]
		public void SHA512HashTest()
		{
			var data = new byte[123456];
			new Random().NextBytes(data);
			var hash1 = SHA512.GetHash(data);
			var hash2 = System.Security.Cryptography.SHA512.Create().ComputeHash(data);
			hash1.Should().BeEquivalentTo(hash2, options => options.WithStrictOrdering());
		}
	}
}