using System.Numerics;
using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption;
using EncryptionService.Core.Utils;

namespace EncryptionService.Core.Services.AsymmetricEncryption
{
	public class ElGamalEncryptionService : IEncryptionService<ElGamalEncryptionResult,
		ElGamalEncryptionKey, ElGamalEncryptionKeyData>
	{
		public const char SEPARATOR_A = '|';
		public const char SEPARATOR_B = ',';

		private int _g;
		private int _x;
		private readonly int _y;
		private readonly Random _random = new();

		public ElGamalEncryptionService(ElGamalEncryptionKey key)
		{
			int p = key.Key.P;
			GenerateG(p);
			GenerateX(p);
			_y = (int)BigInteger.ModPow(_g, _x, p);
		}

		public ElGamalEncryptionResult Encrypt(string text, ElGamalEncryptionKey encryptionKey)
		{
			int p = encryptionKey.Key.P;
			int k = encryptionKey.Key.K;
			int a = (int)BigInteger.ModPow(_g, k, p);

			List<int> bList = [];

			foreach (char ch in text)
			{
				int m = ch;
				int b = (m * (int)BigInteger.ModPow(_y, k, p)) % p;
				bList.Add(b);
			}

			string result = Serialize(a, bList);
			return new ElGamalEncryptionResult(result, _y, _g, p, _x, a, bList);
		}
		public ElGamalEncryptionResult Decrypt(string encryptedText,
			ElGamalEncryptionKey encryptionKey)
		{
			int p = encryptionKey.Key.P;
			var (a, bList) = Deserialize(encryptedText);
			int a_inv = MathUtils.ModInverse((int)BigInteger.ModPow(a, _x, p), p);
			StringBuilder builder = new();

			foreach (int b in bList)
			{
				int m = (int)((b * a_inv) % p);
				builder.Append((char)m);
			}

			return new ElGamalEncryptionResult(builder.ToString(), _y, _g, p, _x, a, bList);
		}

		private static string Serialize(int a, IEnumerable<int> bList)
			=> $"{a}|{string.Join(",", bList)}";
		private static (int a, List<int> bValues) Deserialize(string input)
		{
			var parts = input.Split('|');
			int a = int.Parse(parts[0]);
			List<int> bValues = [.. parts[1].Split(',').Select(int.Parse)];

			return (a, bValues);
		}

		private void GenerateG(int p)
		{
			if (!IsPrime(p))
				throw new ArgumentException("p must be a prime number.");

			int phi = p - 1;
			var factors = GetPrimeFactors(phi);

			for (int g = 2; g < p; g++)
			{
				bool isPrimitiveRoot = true;
				foreach (int factor in factors)
					if (BigInteger.ModPow(g, phi / factor, p) == 1)
					{
						isPrimitiveRoot = false;
						break;
					}

				if (isPrimitiveRoot)
				{
					_g = g;
					break;
				}
			}
		}
		private void GenerateX(int p)
			=> _x = _random.Next(1, p);

		private static bool IsPrime(int n)
		{
			if (n <= 1) return false;
			if (n <= 3) return true;
			if (n % 2 == 0 || n % 3 == 0) return false;

			for (int i = 5; i * i <= n; i += 6)
				if (n % i == 0 || n % (i + 2) == 0)
					return false;

			return true;
		}
		public static HashSet<int> GetPrimeFactors(int n)
		{
			var factors = new HashSet<int>();
			while (n % 2 == 0)
			{
				factors.Add(2);
				n /= 2;
			}

			for (int i = 3; i * i <= n; i += 2)
				while (n % i == 0)
				{
					factors.Add(i);
					n /= i;
				}

			if (n > 2)
				factors.Add(n);

			return factors;
		}
	}
}