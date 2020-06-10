using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Extentions;
using UnityEditor;

namespace UI
{
    /// <summary>
    /// Постпросессор для формирования enum с типами ui панелей
    /// </summary>
    public class UIPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string asset in importedAssets.Where(IsPanelAsset))
            {
                AddType(asset);
            }

            foreach (string asset in deletedAssets.Where(IsPanelAsset))
            {
                RemoveType(asset);
            }

            for (int i = 0; i < movedAssets.Length; i++)
            {
                if (IsPanelAsset(movedAssets[i]) || IsPanelAsset(movedFromAssetPaths[i]))
                {
                    ReplaceType(movedFromAssetPaths[i], movedAssets[i]);
                }
            }
        }

        private static bool IsPanelAsset(string assetPath)
        {
            return assetPath.StartsWith(m_path, true, CultureInfo.InvariantCulture);
        }

        public static void AddType(string assetPath)
        {
            if (!assetPath.EndsWith(".prefab"))
            {
                return;
            }

            assetPath = assetPath.DeleteSuffix(".prefab");
            var parts = assetPath.Split('/');
            var name = parts[parts.Length - 1];
            var key = name;
            name = name.Replace(" ", "_");
            name = ConvertToPascalCase(name);

            if (File.Exists(m_copyPath))
            {
                List<string> lines = new List<string>();
                using (StreamReader reader =
                    new StreamReader(m_copyPath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (line.Contains(name))
                        {
                            return;
                        }

                        lines.Add(line);
                    }
                }
                // Удаляем закрывающую скобку
                lines.RemoveAt(lines.Count - 1);
                // Удаялем файл
                File.Delete(m_copyPath);

                using (StreamWriter outfile = new StreamWriter(m_copyPath))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        outfile.WriteLine(lines[i]);
                    }

                    parts = lines[lines.Count - 1].Split(' ');
                    int lastNumber = int.Parse(parts[parts.Length - 1].Substring(0, parts[parts.Length - 1].Length - 1));
                    lastNumber++;
                    outfile.WriteLine(string.Format("    [AssetPath(\"{0}\")] {1} = {2},", key, name, lastNumber));
                    outfile.WriteLine("}");
                }
            }
            else
            {
                CreateFile();
            }

            AssetDatabase.ImportAsset(m_copyPath, ImportAssetOptions.ForceUpdate);
        }

        private static void RemoveType(string assetPath)
        {
            if (!assetPath.EndsWith(".prefab"))
            {
                return;
            }

            assetPath = assetPath.DeleteSuffix(".prefab");

            var parts = assetPath.Split('/');
            var name = parts[parts.Length - 1];
            name = name.Replace(" ", "_");
            name = ConvertToPascalCase(name);

            if (File.Exists(m_copyPath))
            {
                List<string> lines = new List<string>();
                using (StreamReader reader = new StreamReader(m_copyPath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (line.Contains(name))
                        {
                            continue;
                        }

                        lines.Add(line);
                    }
                }
                // Удаялем файл
                File.Delete(m_copyPath);

                using (StreamWriter outfile =
                    new StreamWriter(m_copyPath))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        outfile.WriteLine(lines[i]);
                    }
                }
            }
            else
            {
                CreateFile();
            }

            AssetDatabase.ImportAsset(m_copyPath, ImportAssetOptions.ForceUpdate);
        }

        private static void ReplaceType(string assetPath, string newAssetPath)
        {
            if (!assetPath.EndsWith(".prefab"))
            {
                return;
            }

            assetPath = assetPath.DeleteSuffix(".prefab");
            var parts = assetPath.Split('/');
            var name = parts[parts.Length - 1];
            name = name.Replace(" ", "_");
            name = ConvertToPascalCase(name);

            newAssetPath = newAssetPath.DeleteSuffix(".prefab");
            parts = newAssetPath.Split('/');
            var newName = parts[parts.Length - 1];
            var key = newName;
            newName = newName.Replace(" ", "_");
            newName = ConvertToPascalCase(newName);

            if (File.Exists(m_copyPath))
            {
                bool replaced = false;
                List<string> lines = new List<string>();
                using (StreamReader reader = new StreamReader(m_copyPath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (line.Contains(name))
                        {
                            var lineParts = line.Split('=');
                            line = string.Format("    [AssetPath(\"{0}\")] {1} ={2}", key, newName, lineParts[1]);
                            replaced = true;
                        }

                        lines.Add(line);
                    }
                }
                // Удаялем файл
                File.Delete(m_copyPath);

                using (StreamWriter outfile = new StreamWriter(m_copyPath))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        outfile.WriteLine(lines[i]);
                    }
                }

                if (!replaced)
                {
                    AddType(newName);
                }
            }
            else
            {
                
            }

            AssetDatabase.ImportAsset(m_copyPath, ImportAssetOptions.ForceUpdate);
        }

        private static void CreateFile()
        {
            Dictionary<string, string> names = new Dictionary<string, string>();

            var files = Directory.GetFiles(m_path, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var guid = AssetDatabase.AssetPathToGUID(file);
                var extension = Path.GetExtension(file);
                if (name.EndsWith(".prefab") || extension.Contains("meta"))
                {
                    continue;
                }

                names.Add(name, guid);
            }

            File.Delete(m_copyPath);

            using (StreamWriter outfile = new StreamWriter(m_copyPath))
            {
                outfile.WriteLine("");
                outfile.WriteLine("public enum UIPanelType");
                outfile.WriteLine("{");
                outfile.WriteLine("    [AssetPath(\"0\")] None = 0,");
                var namesArray = names.Keys.ToArray();
                var guids = names.Values.ToArray();

                for (int i = 0; i < names.Count; i++)
                {
                    var name = namesArray[i];
                    var guid = guids[i];
                    name = name.Replace(" ", "_");
                    name = ConvertToPascalCase(name);
                    var line = string.Format("    [AssetPath(\"{0}\")] {1} = {2},", guid, name, i + 1);
                    outfile.WriteLine(line);
                }
                outfile.WriteLine("}");
            }
        }

        private static string ConvertToPascalCase(string source)
        {
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            source = source.Replace("_", " ");
            return info.ToTitleCase(source).Replace(" ", string.Empty);
        }

        private static string m_path = "Assets/Art/UI/UIPanel/Prefabs/";
        private static string m_copyPath = "F:/Unity/Hippocampus/Assets/code/Scripts/UI/UIPanelType.cs";
    }
}
