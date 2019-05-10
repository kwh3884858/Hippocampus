using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skylight
{
    public abstract class DBRecord
    {
        public abstract void Parse(string[] cols);
        protected object [] fileds;
    }
}