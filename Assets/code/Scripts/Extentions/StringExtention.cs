using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extentions
{
    public static class StringHelper
    {
        public static string DeleteSuffix(this string _input,string suffix)
        {
            return _input.TrimEnd(suffix.ToCharArray());
        }
    }
}
