using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Extentions;
using UnityEditor;

namespace UI
{
    public enum UIPrefabsType
    {
        None = 0,
        Panel,
        PanelElement
    }
    public class UIPrefabsPathData
    {
        public Dictionary<UIPrefabsType, string> Paths;
        public Dictionary<UIPrefabsType, string> TypeClassName;
        public Dictionary<UIPrefabsType, string> ScriptPaths;

        public UIPrefabsPathData()
        {
            Paths = new Dictionary<UIPrefabsType, string>()
            {
                {UIPrefabsType.Panel,"Assets/Art/UI/UIPanel/Prefabs/"},
                {UIPrefabsType.PanelElement,"Assets/Art/UI/UIPanel/Element/"}
            };
            
            TypeClassName = new Dictionary<UIPrefabsType, string>()
            {
                {UIPrefabsType.Panel,"public enum UIPanelType"},
                {UIPrefabsType.PanelElement,"public enum UIElementType"}
            };

            ScriptPaths = new Dictionary<UIPrefabsType, string>()
            {
                {UIPrefabsType.Panel,"Assets/Scripts/UI/UIPanelType.cs"},
                {UIPrefabsType.PanelElement,"Assets/Scripts/UI/UIElementType.cs"}
            };
        }
    }
    /// <summary>
    /// Постпросессор для формирования enum с типами ui панелей
    /// </summary>
    public class UIPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            UIPrefabsType type;
            foreach (string asset in importedAssets)
            {
                type = GetTargetAssetType(asset);
                if (type != UIPrefabsType.None)
                {
                    AddType(asset, type);
                }
            }

            foreach (string asset in deletedAssets)
            {
                type = GetTargetAssetType(asset);
                if (type != UIPrefabsType.None)
                {
                    RemoveType(asset, type);
                }
            }

            for (int i = 0; i < movedAssets.Length; i++)
            {
                type = GetTargetAssetType(movedAssets[i]);

                if (type != UIPrefabsType.None)
                {
                    type = GetTargetAssetType(movedAssets[i]);
                    ReplaceType(movedFromAssetPaths[i], movedAssets[i],type);
                }
                else
                {
                    type = GetTargetAssetType(movedFromAssetPaths[i]);
                    if (type != UIPrefabsType.None)
                    {
                        RemoveType(movedFromAssetPaths[i],type);
                    }
                }
            }
        }

        private static UIPrefabsType GetTargetAssetType(string assetPath)
        {
            foreach (var value in m_data.Paths)
            {
                if (assetPath.StartsWith(value.Value, true, CultureInfo.InvariantCulture))
                {
                    return value.Key;
                }
            }

            return UIPrefabsType.None;
        }

        private static void AddType(string assetPath,UIPrefabsType type)
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

            if (File.Exists(m_data.ScriptPaths[type]))
            {
                List<string> lines = new List<string>();
                using (StreamReader reader =
                    new StreamReader(m_data.ScriptPaths[type]))
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
                File.Delete(m_data.ScriptPaths[type]);

                using (StreamWriter outfile = new StreamWriter(m_data.ScriptPaths[type]))
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
                CreateFile(type);
            }

            AssetDatabase.ImportAsset(m_data.ScriptPaths[type], ImportAssetOptions.ForceUpdate);
        }

        private static void RemoveType(string assetPath,UIPrefabsType type)
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

            if (File.Exists(m_data.ScriptPaths[type]))
            {
                List<string> lines = new List<string>();
                using (StreamReader reader = new StreamReader(m_data.ScriptPaths[type]))
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
                File.Delete(m_data.ScriptPaths[type]);

                using (StreamWriter outfile =
                    new StreamWriter(m_data.ScriptPaths[type]))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        outfile.WriteLine(lines[i]);
                    }
                }
            }
            else
            {
                CreateFile(type);
            }

            AssetDatabase.ImportAsset(m_data.ScriptPaths[type], ImportAssetOptions.ForceUpdate);
        }

        private static void ReplaceType(string assetPath, string newAssetPath,UIPrefabsType type)
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

            if (File.Exists(m_data.ScriptPaths[type]))
            {
                bool replaced = false;
                List<string> lines = new List<string>();
                using (StreamReader reader = new StreamReader(m_data.ScriptPaths[type]))
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
                File.Delete(m_data.ScriptPaths[type]);

                using (StreamWriter outfile = new StreamWriter(m_data.ScriptPaths[type]))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        outfile.WriteLine(lines[i]);
                    }
                }

                if (!replaced)
                {
                    AddType(newName,type);
                }
            }
            else
            {
                CreateFile(type);
            }

            AssetDatabase.ImportAsset(m_data.ScriptPaths[type], ImportAssetOptions.ForceUpdate);
        }

        private static void CreateFile(UIPrefabsType type)
        {
            List<string> names = new List<string>();

            var files = Directory.GetFiles(m_data.Paths[type], "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var extension = Path.GetExtension(file);
                if (name.EndsWith(".prefab") || extension.Contains("meta"))
                {
                    continue;
                }

                names.Add(name);
            }

            File.Delete(m_data.ScriptPaths[type]);

            using (StreamWriter outfile = new StreamWriter(m_data.ScriptPaths[type]))
            {
                outfile.WriteLine("");
                outfile.WriteLine(m_data.TypeClassName[type]);
                outfile.WriteLine("{");
                outfile.WriteLine("    [AssetPath(\"0\")] None = 0,");

                for (int i = 0; i < names.Count; i++)
                {
                    var name = names[i];
                    var key = name;
                    name = name.Replace(" ", "_");
                    name = ConvertToPascalCase(name);
                    var line = string.Format("    [AssetPath(\"{0}\")] {1} = {2},", key, name, i + 1);
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

        private static UIPrefabsPathData m_data = new UIPrefabsPathData();
    }
}
