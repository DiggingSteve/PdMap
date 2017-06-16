using System.Collections;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// 参数异常
    /// </summary>
    public class ParamException : ArgumentException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="paramName"></param>
        /// <param name="data"></param>
        public ParamException(string msg, string paramName = null, Dictionary<string, string> data = null) : base(msg, paramName)
        {
            this._data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="paramName"></param>
        /// <param name="data"></param>
        public ParamException(string msg, string paramName, string data) : base(msg, paramName)
        {
            if (data != null)
            {
                this._data = new Dictionary<string, string>() { { "data", data } };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public ParamException(string msg, string data) : base(msg)
        {
            this._data = new Dictionary<string, string>() { { "data", data } };
        }

        private IDictionary _data = null;

        /// <summary>
        /// 数据集合
        /// </summary>
        public override IDictionary Data
        {
            get
            {
                return this._data;
            }
        }
    }
}
