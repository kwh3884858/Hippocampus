using System;

public class AssetPathAttribute : Attribute
{
    public string Path;
    public AssetPathAttribute(string id)
    {
        Path = id;
    }

    public static string GetPath<T>(T type) where T: Enum
    {
        var atrs = type.GetType().GetMember(type.ToString());
        var attributes = atrs[0].GetCustomAttributes(typeof(AssetPathAttribute), false);
        var atribute = (AssetPathAttribute)attributes[0];
        return atribute.Path;
    }
}
