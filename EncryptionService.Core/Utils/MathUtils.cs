using System.Numerics;

namespace EncryptionService.Core.Utils
{
	public static class MathUtils
	{
		/// <summary>
		/// The method calculates GCD (Greatest Common Divisor)
		/// </summary>
		/// <param name="a">The first integer.</param>
		/// <param name="b">The second integer.</param>
		/// <returns>The greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.</returns>
		public static int CalculateGCD(int a, int b)
		{
			while (b != 0)
			{
				int temp = b;
				b = a % b;
				a = temp;
			}

			return a;
		}

		/// <summary>
		/// Calculates the modular multiplicative inverse of <paramref name="a"/> modulo <paramref name="mod"/>.
		/// </summary>
		/// <param name="a">The number to find the inverse for.</param>
		/// <param name="mod">The modulus.</param>
		/// <returns>The modular inverse of <paramref name="a"/> modulo <paramref name="mod"/>.</returns>
		/// <exception cref="ArgumentException">Thrown when the modular inverse does not exist.</exception>
		public static int ModInverse(int a, int mod)
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

		/// <summary>
		/// Calculates a simplified quadratic convolution hash value for the given text modulo <paramref name="mod"/>.
		/// </summary>
		/// <param name="text">The input string to hash.</param>
		/// <param name="mod">The modulus used in the hash calculation.</param>
		/// <returns>The hash value as an integer.</returns>
		public static int ComputeQuadraticHash(string text, int mod)
		{
			int m = 0;

			foreach (char ch in text)
				m = (int)BigInteger.ModPow(m + ch, 2, mod);

			return m;
		}
	}
}