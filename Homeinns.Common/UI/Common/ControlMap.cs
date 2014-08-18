using System.Collections.Generic;

namespace Homeinns.Common.UI.Common
{
    /// <summary>
    /// 控件前缀及绑定值设置类
    /// </summary>
    public class ControlMap
    {
        private readonly Dictionary<string, ControlDictInfo> _dict;

        public ControlMap()
        {
            _dict = new Dictionary<string, ControlDictInfo>();
        }

        public ControlMap(params ControlDictInfo[] controlDictInfoArray)
            : this()
        {
            foreach (ControlDictInfo controlDictInfo in controlDictInfoArray)
            {
                _dict.Add(controlDictInfo.Prefix, controlDictInfo);
            }
        }

        public Dictionary<string, ControlDictInfo> ControlDictionary
        {
            get { return _dict; }
        }

        public void Add(ControlDictInfo controlDictInfo)
        {
            _dict.Add(controlDictInfo.Prefix, controlDictInfo);
        }
    }
}