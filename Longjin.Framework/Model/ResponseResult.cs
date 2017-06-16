using Newtonsoft.Json;
using System;

namespace Longjin.Model.Common
{
    public class ResponseResult
    {
        public ResponseResult(bool status = true, object data = null, string msg = null)
        {
            this.Status = status;
            this.Data = data;
            this.Msg = msg;
        }

        /// <summary>
        /// 执行状态  执行结果
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 返回结果 业务结果
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 总数据条数
        /// </summary>
        public long? Total { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public ValueType Code { get; set; }

        /// <summary>
        /// 序列化为JSON
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
