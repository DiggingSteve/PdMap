using System.Collections;
using System.Collections.Generic;

namespace System
{
    public class AuthException : Exception
    {
        public AuthException(string msg, ValueType code = null, Dictionary<string, string> data = null) : base(msg)
        {
            this.Code = code;
            this._data = data;
        }
        public AuthException(string msg, string data, ValueType code = null) : base(msg)
        {
            this.Code = code;
            if (data != null)
            {
                this._data = new Dictionary<string, string>() { { "data", data } };
            }
        }

        public ValueType Code { get; set; }

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
