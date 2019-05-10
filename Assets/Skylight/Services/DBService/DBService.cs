using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Skylight
{
    public class DBService : GameModule<DBService>
    {
        public bool autoLoad = true;
        private static DBTable[] tables;
        void Start()
        {
            Debug.Log("DBService");
            BinaryReader br = new BinaryReader(new FileStream("Assets/CSV/index.bytes", FileMode.Open));
            int nCount = br.ReadInt32();
            tables = new DBTable[nCount];
            for (int i = 0; i < nCount; i++)
            {
                int nLength = br.ReadSByte();
                string name = System.Text.Encoding.Default.GetString(br.ReadBytes(nLength));
                GameObject obj = new GameObject();
                obj.name = name;
                obj.transform.parent = transform;
                var table = obj.AddComponent<DBTable>();
                table.filePath = "Assets/CSV/" + name + ".csv";
                if (autoLoad)
                {
                    table.LoadFromFile(table.filePath);
                }
                tables[i] = table;
            }
        }

        public static DBTable FindTable(TableType type)
        {
			if (tables == null) 
			{
				return null;
			}
            DBTable table = tables[(int)type];
            return table;
        }
        
        public static T FindRecord<T>(TableType type, int id) where T : DBRecord, new()
        {
            DBTable table = FindTable(type);
            if (table)
            {
                return table.FindRecord<T>(id.ToString());
            }
            return default(T);

        }
    }
}
