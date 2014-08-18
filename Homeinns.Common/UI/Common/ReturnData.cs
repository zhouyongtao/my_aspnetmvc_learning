namespace Homeinns.Common.UI.Common
{
    public class ResultData
    {
        protected string _message; // 返回信息
        protected bool _success; // 成功 

        public ResultData()
        {
        }

        public ResultData(bool success)
        {
            _success = success;
        }

        #region 属性过程

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        public object Tag { get; set; }

        #endregion
    }

    public class ResultData<T> : ResultData where T : new()
    {
        protected T _entity;

        public ResultData()
        {
        }

        public ResultData(T entity)
        {
            _entity = entity;
        }

        /// <summary>
        /// 返回实体
        /// </summary>
        public T Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
    }
}