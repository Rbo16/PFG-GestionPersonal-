using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Utiles
{
    public static class Password
    {
            private static readonly char[] Punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

            /// <summary>
            /// Genera una contraseña aleatori tan larga como se indique y con tantos caracteres especiales como 
            /// se indique.
            /// </summary>
            /// <param name="length">Longitud deseada de la contraseña a generar. Entre 1 y 128.</param>
            /// <param name="numberOfNonAlphanumericCharacters">Número de caracteres especiales que deseamos
            /// incluir en la contraseña. No puede superar la longitud de la contraseña.</param>
            /// <returns>La nueva contraseña generada en fortamo string.</returns>
            /// <exception cref="ArgumentException"></exception>
            public static string Generate(int length, int numberOfNonAlphanumericCharacters)
            {
                if (length < 1 || length > 128)
                {
                    throw new ArgumentException(nameof(length));
                }

                if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
                {
                    throw new ArgumentException(nameof(numberOfNonAlphanumericCharacters));
                }

                using (var rng = RandomNumberGenerator.Create())
                {
                    var byteBuffer = new byte[length];

                    rng.GetBytes(byteBuffer);

                    var count = 0;
                    var characterBuffer = new char[length];

                    for (var iter = 0; iter < length; iter++)
                    {
                        var i = byteBuffer[iter] % 87;

                        if (i < 10)
                        {
                            characterBuffer[iter] = (char)('0' + i);
                        }
                        else if (i < 36)
                        {
                            characterBuffer[iter] = (char)('A' + i - 10);
                        }
                        else if (i < 62)
                        {
                            characterBuffer[iter] = (char)('a' + i - 36);
                        }
                        else
                        {
                            characterBuffer[iter] = Punctuations[i - 62];
                            count++;
                        }
                    }

                    if (count >= numberOfNonAlphanumericCharacters)
                    {
                        return new string(characterBuffer);
                    }

                    int j;
                    var rand = new Random();

                    for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
                    {
                        int k;
                        do
                        {
                            k = rand.Next(0, length);
                        }
                        while (!char.IsLetterOrDigit(characterBuffer[k]));

                        characterBuffer[k] = Punctuations[rand.Next(0, Punctuations.Length)];
                    }

                    return new string(characterBuffer);
                }
            }
    }
}

