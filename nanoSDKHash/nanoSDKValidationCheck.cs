using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace nanoSDKHash
{
    public class nanoSDKValidationCheck
    {
        [MenuItem("nanoSDK Security/Compare Current")]
        public static void ValidateHashCheck()
        {
            try
            {
                string path = $"Assets{Path.DirectorySeparatorChar}VRCSDK{Path.DirectorySeparatorChar}nanoSDK";
                Uri serverUrl = new Uri("https://www.nanosdk.net/download/Hash/hashes.txt");

                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                                     .OrderBy(p => p).ToList();

                //get all hashes from .cs files in path then compare to server
                List<string> hashes = new List<string>();
                foreach (var file in files)
                {
                    if (file.EndsWith(".cs"))
                    {
                        string hash = GetMD5HashFromFile(file);
                        hashes.Add(hash);
                    }
                }
                //read all lines from server
                List<string> serverHashes = new List<string>();
                using (WebClient client = new WebClient())
                {
                    string serverHashesString = client.DownloadString(serverUrl);
                    serverHashes = serverHashesString.Split('\n').ToList();
                }
                //compare if any hash is different from local and server
                bool isValid = true;
                for (int i = 0; i < hashes.Count; i++)
                {
                    if (hashes[i] != serverHashes[i])
                    {
                        isValid = false;
                        break;
                    }
                }
                if (isValid)
                {
                    //debug log color
                    //debug underline color
                    Debug.Log("<color=green><b>[nanoSDK Security]:</b></color>\n" +
                        "you are using a Valid Version of nanoSDK!");
                }
                else
                {
                    //debug underline color
                    Debug.Log("<color=red><b>[nanoSDK Security]:</b></color>\n" +
                        "you are using an Invalid Version of nanoSDK!");
                    if (EditorUtility.DisplayDialog("Hash Check", "you are using an Invalid Version of nanoSDK!", "Open nanoSDK Website", "OK"))
                    {
                        Application.OpenURL("https://www.nanosdk.net/");
                    }
                }
            }
            catch (NotImplementedException)
            {
                //catch nothing bc it get called bc of obfuscation
            }
        }

        private static string GetMD5HashFromFile(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
            
        }
    }
}
