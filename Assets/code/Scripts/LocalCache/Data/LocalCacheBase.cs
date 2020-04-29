using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LocalCache
{
    [Serializable]
    public abstract class LocalCacheBase
    {
        public virtual string ToJson()
        {
            return LocalCacheJsonUtil.Serializer( this);
        }
        public virtual string GetTypeName()
        {
            return GetType().FullName;
        }

        public T Clone<T>()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this); //复制到流中
            ms.Position = 0;
            return (T)bf.Deserialize(ms);
        }
    }

}