using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace nanoSDKHash
{
    public class nanoSDKCheckHashes
    {
        public static void CheckHashes()
        {
            string path = $"Assets{Path.DirectorySeparatorChar}VRCSDK{Path.DirectorySeparatorChar}nanoSDK";

            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                                 .OrderBy(p => p).ToList();

            var hashFile = File.ReadAllText($"{path}{Path.DirectorySeparatorChar}hashes.txt");


            foreach (var file in files)
            {
                if (!file.EndsWith(".cs"))
                {
                    continue;
                }
                var md5 = MD5.Create();
                var hash = md5.ComputeHash(File.ReadAllBytes(file));
                var result = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                //Debug.Log(file + " : " + result);
                if (!hashFile.Contains(result))
                {
                    Debug.LogError("File is not the same: " + file + " : " + result);
                    if (EditorUtility.DisplayDialog("Error", "Manipulation Detected, Please dont Manipulate nanoSDK Code!", "OK"))
                    {
                        
                        foreach (var filePath in files)
                        {
                            File.Delete(filePath);
                        }
                        //Exit Unity
                        EditorApplication.Exit(0);
                    }
                }

            }
        }
    }
}
