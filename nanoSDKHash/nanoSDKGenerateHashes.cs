using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace nanoSDKHash
{
    public class nanoSDKGenerateHashes
    {
        public static void GenerateHash()
        {
            string path = $"Assets{Path.DirectorySeparatorChar}VRCSDK{Path.DirectorySeparatorChar}nanoSDK";
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                                 .OrderBy(p => p).ToList();
            File.WriteAllText($"{path}{Path.DirectorySeparatorChar}hashes.txt", "");
            foreach (var file in files)
            {
                if (!file.EndsWith(".cs"))
                {
                    continue;
                }
                var md5 = MD5.Create();
                var hash = md5.ComputeHash(File.ReadAllBytes(file));
                var result = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                Debug.Log(file + " : " + result);

                File.AppendAllText($"{path}{Path.DirectorySeparatorChar}hashes.txt", $"{result}\n");
            }
        }
    }
}
