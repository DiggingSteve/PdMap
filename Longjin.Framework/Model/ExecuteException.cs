using System.Collections;
using System.Collections.Generic;

namespace System
{
    public class ExecuteException : Exception
    {
        public ExecuteException(string msg, Dictionary<string, string> data = null) : base(msg)
        {
            this._data = data;
        }

        public ExecuteException(string msg, string data) : base(msg)
        {
            if (data != null)
            {
                this._data = new Dictionary<string, string>() { { "data", data } };
            }
        }

        private IDictionary _data = null;

        public override IDictionary Data
        {
            get
            {
                return this._data;
            }
        }
    }
}
