using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonal.Utiles
{
    public static class ConvertidorHASH
    {
    
    /// <summary>
    /// Método estático para obtener el Hash de la cadena indicada.
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static byte[] GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }
   
    /// <summary>
    /// Devuelve ek Hash de la cadena indicada en formato string.
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}
}
