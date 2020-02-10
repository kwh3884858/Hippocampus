using System;
using Newtonsoft.Json;

namespace LocalCache
{
    public class LocalCacheJsonUtil
    {
        public static object Deserialize(Type type, string json)
        {
            try
            {
                object obj = JsonConvert.DeserializeObject(json,type);
                return obj;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public static string Serializer(object obj)                                            
        {
            string str;
            try
            {
                str = JsonConvert.SerializeObject(obj);
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
            return str;
        }
    }
}