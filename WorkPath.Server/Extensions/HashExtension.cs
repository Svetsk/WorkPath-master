using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace WorkPath.Server.Extensions;

public static class HashExtension
{
   /// <summary>
    /// Конвертация строчки в хэш алгоритма SHA256.
    /// Encoding UTF8
    /// </summary>
    /// <param name="input">Строчка.</param>
    /// <returns>Хэш алгоритма SHA256.</returns>
    public static string ToHashSHA256(this string input)
    {
        return ToHashSHA256(input, Encoding.UTF8);
    }
    
    /// <summary>
    /// Конвертация строчки в хэш алгоритма SHA256.
    /// </summary>
    /// <param name="input">Строчка.</param>
    /// <returns>Хэш алгоритма SHA256.</returns>
    public static string ToHashSHA256(this string input, Encoding enc)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA256.Create())
        {
            byte[] result = hash.ComputeHash(enc.GetBytes(input));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }

    /// <summary>
    /// Конвертация строчки в хэш алгоритма HMACSHA256.
    /// Encoding UTF8
    /// </summary>
    /// <param name="input">Строчка.</param>
    /// <param name="key">Ключ.</param>
    /// <returns>Хэш алгоритма HMACSHA256.</returns>
    public static string ToHashHMACSHA256(this string input, string key)
    {
        return ToHashHMACSHA256(input, key, Encoding.UTF8);
    }
    
    /// <summary>
    /// Конвертация строчки в хэш алгоритма HMACSHA256.
    /// </summary>
    /// <param name="input">Строчка.</param>
    /// <param name="key">Ключ.</param>
    /// <returns>Хэш алгоритма HMACSHA256.</returns>
    public static string ToHashHMACSHA256(this string input, string key, Encoding enc)
    {
        StringBuilder Sb = new StringBuilder();
        byte[] keyBytes = enc.GetBytes(key);
        
        using (var hash = new HMACSHA256(keyBytes))
        {
            byte[] result = hash.ComputeHash(enc.GetBytes(input));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }

    /// <summary>
    /// Конвертация листа обьектов в хэш алгоритма SHA256.
    /// Encoding UTF8
    /// </summary>
    /// <param name="datas"></param>
    /// <returns></returns>
    public static string ToHashSHA256(this List<object> datas)
    {
        return ToHashSHA256(JsonConvert.SerializeObject(datas,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
    }

    /// <summary>
    /// Конвертация обьекта в хэш алгоритма SHA256.
    /// Encoding UTF8
    /// </summary>
    /// <param name="datas"></param>
    /// <returns></returns>
    public static string ToHashSHA256(this object data)
    {
        return ToHashSHA256(JsonConvert.SerializeObject(data,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
    }


    /// <summary>
    /// Конвертация строчки в хэш алгоритма SHA1.
    /// Encoding UTF8
    /// </summary>
    /// <param name="input">Строчка.</param>
    /// <returns>Хэш алгоритма SHA1.</returns>
    public static string ToHashSHA1(this string input)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA1.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(input));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }
}