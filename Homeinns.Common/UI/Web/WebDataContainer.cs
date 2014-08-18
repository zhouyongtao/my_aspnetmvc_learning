using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Homeinns.Common.UI.Common;
using Homeinns.Common.Entity.Schema;

namespace Homeinns.Common.UI.Web
{
    /// <summary>
    /// UI数据操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WebDataContainer<T> : DataContainer<T> where T : new()
    {
        private readonly Control[] _containerArray;
        private readonly ControlMap _controlMap;
        private readonly DatabaseSchema _DatabaseSchema;
        private readonly Type _entityType;
        private bool _isEmbed;
        private List<WebControlInfo> _webControlInfoList;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="controlMap"></param>
        /// <param name="databaseSchema"></param>
        /// <param name="isEmbed"></param>
        /// <param name="containerArray"></param>
        public WebDataContainer(ControlMap controlMap, DatabaseSchema databaseSchema, bool isEmbed,
                                params Control[] containerArray)
        {
            _entityType = typeof (T);
            _controlMap = controlMap;
            _isEmbed = isEmbed;
            _containerArray = containerArray;
            _DatabaseSchema = databaseSchema;
        }

        /// <summary>
        /// 页面控件列表
        /// </summary>
        public List<WebControlInfo> WebControlInfoList
        {
            get
            {
                if (_webControlInfoList == null)
                {
                    _webControlInfoList = new List<WebControlInfo>();
                    foreach (Control cont in _containerArray)
                    {
                        FillWebControlInfoList(cont);
                    }
                }

                return _webControlInfoList;
            }
        }

        /// <summary>
        /// 是否递归进入子控件
        /// </summary>
        public bool IsEnbed
        {
            get { return _isEmbed; }
            set { _isEmbed = value; }
        }

        /// <summary>
        /// 绑定界面值时
        /// </summary>
        public event TypeChangeHandler SetDataPropertyValueSettingEvent;

        /// <summary>
        /// 收集界面值时
        /// </summary>
        public event TypeChangeHandler GetDataPropertyValueSettingEvent;

        /// <summary>
        /// 数据验证时
        /// </summary>
        public event ValidateControlValueHandler ValidateControlValueEvent;

        /// <summary>
        /// 填充页面控件列表
        /// </summary>
        /// <param name="cont"></param>
        protected void FillWebControlInfoList(Control cont)
        {
            string prefix = string.Empty;
            string field = string.Empty;

            foreach (Control childCont in cont.Controls)
            {
                // 递归进入子控件
                if (_isEmbed && childCont.Controls.Count != 0)
                {
                    FillWebControlInfoList(childCont);
                }

                if (!AnalyzeControlID(childCont.ID, ref prefix, ref field))
                    continue;

                WebControlInfo webControlInfo = new WebControlInfo();
                webControlInfo.Control = childCont;
                webControlInfo.Prefix = prefix;
                webControlInfo.Field = field;
                webControlInfo.ControlDictInfo = _controlMap.ControlDictionary[prefix];

                _webControlInfoList.Add(webControlInfo);
            }
        }

        /*
        /// <summary>
        /// 设置可用性
        /// </summary>
        public bool Enabled
        {
            set { SetEnabled(value); }
        }

        public void SetEnabled(bool enabled)
        {
            foreach (Control cont in this._containerArray)
            {
                this.SetControl(enabled, cont);
            }
        }

        private void SetControl(bool enabled, Control cont)
        {
            string prefix = string.Empty;
            string field = string.Empty;

            foreach (Control childCont in cont.Controls)
            {
                // 递归进入子控件
                if (this._isEmbed && childCont.Controls.Count != 0)
                {
                    SetControl(enabled, childCont);
                }

                if (!AnalyzeControlID(childCont.ID, ref prefix, ref field))
                    continue;

                WebControl webControl = childCont as WebControl;
                if (webControl != null)
                    webControl.Enabled = enabled;

            }
        }
        */

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="entity"></param>
        public void SetData(T entity)
        {
            foreach (WebControlInfo webCont in WebControlInfoList)
            {
                if (webCont.ControlDictInfo.ControlGroup == ControlGroupEnum.DataGroup)
                {
                    PropertyInfo entityPropertyInfo = _entityType.GetProperty(webCont.Field);
                    if (entityPropertyInfo == null)
                        continue;

                    object entityPropertyValue = entityPropertyInfo.GetValue(entity, null);
                    if (entityPropertyValue == null)
                        continue;

                    Type entityPropertyType = GetUnderlyingType(entityPropertyInfo.PropertyType);
                    object controlValue = null;
                    var e = new PropertyValueSettingEventArgs(webCont.ControlDictInfo.PropertyInfo.PropertyType,
                                                              entityPropertyType, entityPropertyValue, controlValue);
                    SetDataPropertyValueSettingEventMethod(webCont.Control, e);

                    if (e.Cancel)
                        continue;

                    webCont.ControlDictInfo.PropertyInfo.SetValue(webCont.Control, e.ControlValue, null);
                }
            }
        }

        /// <summary>
        /// SetData属性值绑定事件管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDataPropertyValueSettingEventMethod(object sender, PropertyValueSettingEventArgs e)
        {
            if (SetDataPropertyValueSettingEvent != null)
            {
                SetDataPropertyValueSettingEvent(sender, e);
            }
        }

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns></returns>
        public T GetData()
        {
            var entity = new T();
            
            foreach (WebControlInfo webCont in WebControlInfoList)
            {
                if (webCont.ControlDictInfo.ControlGroup == ControlGroupEnum.DataGroup)
                {
                    object controlValue = webCont.ControlDictInfo.PropertyInfo.GetValue(webCont.Control, null);

                    if (controlValue == null)
                        continue;

                    string controlStr = controlValue as string;
                    if (controlStr != null && controlStr == string.Empty)
                        continue;

                    PropertyInfo entityPropertyInfo = _entityType.GetProperty(webCont.Field);
                    if (entityPropertyInfo == null)
                        continue;

                    Type entityPropertyType = GetUnderlyingType(entityPropertyInfo.PropertyType);

                    object entityPropertyValue = null;
                    var e = new PropertyValueSettingEventArgs(webCont.ControlDictInfo.PropertyInfo.PropertyType,
                                                              entityPropertyType, entityPropertyValue, controlValue);
                    GetDataPropertyValueSettingEventMethod(webCont.Control, e);

                    if (e.Cancel)
                        continue;

                    entityPropertyInfo.SetValue(entity, e.EntityPropertyValue, null);
                }
            }

            return entity;
        }

        /// <summary>
        /// SetData属性值绑定事件管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetDataPropertyValueSettingEventMethod(object sender, PropertyValueSettingEventArgs e)
        {
            if (GetDataPropertyValueSettingEvent != null)
            {
                GetDataPropertyValueSettingEvent(sender, e);
            }
        }

        /// <summary>
        /// 获得编辑数据
        /// </summary>
        public EditData<T> GetEditDate()
        {
            var editData = new EditData<T>(GetData()) {EditPropertys = GetEditPropertys()};
            return editData;
        }

        /// <summary>
        /// 得到页面编辑属性名
        /// </summary>
        /// <returns></returns>
        protected List<string> GetEditPropertys()
        {
            var editPropertys = new List<string>();

            foreach (WebControlInfo webCont in WebControlInfoList)
            {
                if (webCont.ControlDictInfo.ControlGroup == ControlGroupEnum.DataGroup)
                {
                    editPropertys.Add(webCont.Field);
                }
            }

            return editPropertys;
        }

        /// <summary>
        /// 分解控件ID
        /// </summary>
        /// <param name="controlID"></param>
        /// <param name="prefix"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool AnalyzeControlID(string controlID, ref string prefix, ref string field)
        {
            if (controlID == null || controlID.Length < 3)
                return false;

            prefix = controlID.Substring(0, 3);
            field = controlID.Substring(3);
            if (!_controlMap.ControlDictionary.ContainsKey(prefix))
                return false;

            return true;
        }

        /// <summary>
        /// 获得根本类型
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        private Type GetUnderlyingType(Type propertyType)
        {
            if (propertyType.ToString().IndexOf("System.Nullable") != -1)
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }

            return propertyType;
        }

        public ResultData ValidateData()
        {
            string tableNameUp = _entityType.Name.ToUpper();
            var result = new ResultData();

            for (int i = 0; i < WebControlInfoList.Count; ++i)
            {
                WebControlInfo webCont = WebControlInfoList[i];
                if (webCont.ControlDictInfo.ControlGroup == ControlGroupEnum.ValidateGroup)
                {
                    HtmlGenericControl validateControl = webCont.Control as HtmlGenericControl;
                    if (validateControl == null)
                        continue;

                    string dataControlID = validateControl.Attributes["for"];
                    if (string.IsNullOrEmpty(dataControlID))
                        continue;

                    WebControlInfo dataControl = FindControlFor(dataControlID, i);
                    if (dataControl == null)
                        continue;

                    ValidateControlValueEventArgs e = new ValidateControlValueEventArgs(
                        dataControl.ControlDictInfo.PropertyInfo.GetValue(dataControl.Control, null),
                        validateControl.Attributes["class"].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries),
                        _DatabaseSchema.TableDict[tableNameUp].ColumnDict[dataControl.Field.ToUpper()],
                        validateControl.InnerText);

                    ValidateControlValueEventMethod(dataControl.Control, e);
                    if (!e.Pass)
                    {
                        result.Success = e.Pass;
                        result.Message = e.Message;
                        result.Tag = dataControl.Control;
                        return result;
                    }
                }
            }

            result.Success = true;
            return result;
        }

        private void ValidateControlValueEventMethod(object sender, ValidateControlValueEventArgs e)
        {
            if (ValidateControlValueEvent != null)
            {
                ValidateControlValueEvent(sender, e);
            }
        }

        private WebControlInfo FindControlInControlInfoList(string controlID)
        {
            foreach (WebControlInfo webCont in WebControlInfoList)
            {
                if (string.Equals(webCont.Control.ID, controlID))
                {
                    return webCont;
                }
            }

            return null;
        }

        private WebControlInfo FindControlFor(string controlID, int index)
        {
            for (int i = index; i < WebControlInfoList.Count; ++i)
            {
                if (string.Equals(WebControlInfoList[i].Control.ID, controlID))
                {
                    return WebControlInfoList[i];
                }
            }

            for (int i = 0; i < index; ++i)
            {
                if (string.Equals(WebControlInfoList[i].Control.ID, controlID))
                {
                    return WebControlInfoList[i];
                }
            }

            return null;
        }
    }
}