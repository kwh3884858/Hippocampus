using System;

public class AssetPathAttribute : Attribute
{
    public string Path;
    public AssetPathAttribute(string id)
    {
        Path = id;
    }
}
