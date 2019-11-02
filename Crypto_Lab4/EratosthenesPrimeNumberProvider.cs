using System.Collections.Generic;
using System.Linq;

namespace Crypto_Lab4
{
	public class EratosthenesPrimeNumberProvider
	{
		private readonly SortedSet<int> primeNumbers = new SortedSet<int> { 2 };

		public int[] GetPrimeNumbersFrom2To(int n)
		{
			if (primeNumbers.Any(x => x > n))
				return primeNumbers.TakeWhile(x => x < n).ToArray();

			var maxKnownPrimeNumber = primeNumbers.Last();
			GetPrimeNumbers(maxKnownPrimeNumber + 1, n);
			return primeNumbers.ToArray();
		}

		public int[] GetFirstPrimeNumbers(int n)
		{
			var last = primeNumbers.Last();
			while (primeNumbers.Count < n)
			{
				var dif = n - primeNumbers.Count;
				var average = (double)primeNumbers.Last() / primeNumbers.Count * 1.15;
				var tableSize = (int) (dif * average);
				GetPrimeNumbers(last + 1, last + tableSize);
				last += tableSize;
			}

			return primeNumbers.Take(n).ToArray();
		}

		private void GetPrimeNumbers(int from, int to)
		{
			var table = Enumerable
				.Range(from, to + 1 - from)
				.Select(number => new EratosthenesTableItem { Number = number })
				.ToArray();

			foreach (var primeNumber in primeNumbers)
				CrossBy(table, primeNumber);

			foreach (var t in table)
			{
				if (t.Crossed)
					continue;
				var p = t.Number;
				CrossBy(table, p);
				primeNumbers.Add(p);
			}
		}

		private static void CrossBy(IReadOnlyList<EratosthenesTableItem> table, int p)
		{
			var deltaP = p == 2 ? p : 2 * p;
			var delta = 1;
			for (var i = 0; i < table.Count; i += delta)
			{
				if (table[i].Number == p || table[i].Number % p != 0 || table[i].Crossed)
					continue;
				delta = deltaP;
				table[i].Crossed = true;
			}
		}

		private class EratosthenesTableItem
		{
			public int Number { get; set; }
			public bool Crossed { get; set; }
		}
	}
}