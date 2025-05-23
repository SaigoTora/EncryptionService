using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.KnapsackEncryption;

namespace EncryptionService.Core.Services.AsymmetricEncryption
{
	public class KnapsackEncryptionService : IEncryptionService<KnapsackEncryptionResult,
		KnapsackEncryptionKey, KnapsackEncryptionKeyData>
	{
		public const string SEPARATOR = " ";

		private int[] _d = [], _e = [];
		private int _m, _n, _inverseN;
		private bool[] _bits = new bool[8];
		private readonly Random _random = new();

		public KnapsackEncryptionService(KnapsackEncryptionKey key)
		{
			GenerateEncryptionValues(key.Key);
		}

		public KnapsackEncryptionResult Encrypt(string text, KnapsackEncryptionKey encryptionKey)
			=> ProcessEncryption(text, true);
		public KnapsackEncryptionResult Decrypt(string encryptedText,
			KnapsackEncryptionKey encryptionKey)
			=> ProcessEncryption(encryptedText, false);

		private KnapsackEncryptionResult ProcessEncryption(string text, bool isEncryption)
		{
			StringBuilder builder = new();

			if (isEncryption)
				foreach (char ch in text)
				{
					_bits = ConvertCharToBits(ch);

					builder.Append(EncryptBits(_bits) + SEPARATOR);
				}
			else
				foreach (string str in text.Split(SEPARATOR))
				{
					int weightSum = (Convert.ToInt32(str) * _inverseN) % _m;
					_bits = DecryptBitsFromSum(weightSum);

					builder.Append(ConvertBitsToChar(_bits));
				}

			return new KnapsackEncryptionResult(builder.ToString(), _n, _m, _d, _e);
		}

		#region Generating encryption values
		private void GenerateEncryptionValues(KnapsackEncryptionKeyData keyData)
		{
			GenerateD(keyData);
			GenerateM(keyData);
			GenerateN();
			GenerateE();
			_inverseN = ModInverse(_n, _m);
		}

		private void GenerateD(KnapsackEncryptionKeyData keyData)
		{
			_d = new int[8];
			_d[0] = keyData.D0;
			for (int i = 1; i < _d.Length; i++)
				_d[i] = _d.Sum() + _random.Next(keyData.StepMin, keyData.StepMax);
		}
		private void GenerateM(KnapsackEncryptionKeyData keyData)
			=> _m = _d.Sum() + _random.Next(keyData.StepMin, keyData.StepMax);
		private void GenerateN()
		{
			List<int> numbers = [];
			for (int i = 2; i < _m; i++)
				if (i % 2 != 0 && GCD(i, _m) == 1)
					numbers.Add(i);

			_n = numbers[_random.Next(numbers.Count)];
		}
		private void GenerateE()
		{
			_e = new int[_d.Length];
			for (int i = 1; i < _e.Length; i++)
				_e[i] = (_d[i] * _n) % _m;
		}
		#endregion

		private static bool[] ConvertCharToBits(char ch)
		{
			byte value = (byte)ch;
			bool[] bits = new bool[8];

			for (int i = 0; i < 8; i++)
				bits[i] = (value & (1 << (7 - i))) != 0;

			return bits;
		}
		private static char ConvertBitsToChar(bool[] bits)
		{
			byte result = 0;

			for (int i = 0; i < bits.Length; i++)
				if (bits[i])
					result |= (byte)(1 << (7 - i));

			return (char)result;
		}

		private int EncryptBits(bool[] bits)
		{
			int sum = 0;
			for (int i = 0; i < bits.Length && i < _e.Length; i++)
				if (bits[i])
					sum += _e[i];

			return sum;
		}
		public bool[] DecryptBitsFromSum(int sum)
		{
			if (_d == null)
				return [];

			bool[] bits = new bool[_d.Length];

			for (int i = _d.Length - 1; i >= 0; i--)
			{
				if (sum >= _d[i])
				{
					bits[i] = true;
					sum -= _d[i];
				}
				else
					bits[i] = false;
			}

			return bits;
		}

		private static int GCD(int a, int b)
		{
			while (b != 0)
			{
				int temp = b;
				b = a % b;
				a = temp;
			}
			return a;
		}
		private static int ModInverse(int a, int mod)
		{
			int t = 0, newT = 1;
			int r = mod, newR = a;

			while (newR != 0)
			{
				int quotient = r / newR;

				(t, newT) = (newT, t - quotient * newT);
				(r, newR) = (newR, r - quotient * newR);
			}

			if (r > 1)
				throw new ArgumentException("The number does not have an inverse modulus.");

			if (t < 0)
				t += mod;

			return t;
		}
	}
}