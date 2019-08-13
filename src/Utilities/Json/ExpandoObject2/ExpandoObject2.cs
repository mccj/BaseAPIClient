using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace System.Dynamic
{
    /// <summary>
    /// 表示可在运行时动态添加和删除其成员的对象。
    /// </summary>
    [System.Serializable]
    public class ExpandoObject2 : System.Dynamic.DynamicObject, IDictionary<string, object>, INotifyPropertyChanged
    {
        /// <summary>
        /// 数据字段
        /// </summary>
        [NonSerialized]
        private Dictionary<string, object> viewData = new Dictionary<string, object>();
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged
        #region DynamicObject
        ///// <summary>
        ///// 调用 varo(); 时执行
        ///// dynamic varo = new VarObject();
        ///// </summary>
        ///// <param name="binder"></param>
        ///// <param name="args"></param>
        ///// <param name="result"></param>
        ///// <returns></returns>
        //public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        //{
        //    return base.TryInvoke(binder, args, out result);
        //}
        ///// <summary>
        ///// 调用 varo.Method(); 时执行
        ///// dynamic varo = new VarObject();
        ///// </summary>
        ///// <param name="binder"></param>
        ///// <param name="args"></param>
        ///// <param name="result"></param>
        ///// <returns></returns>
        //public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        //{
        //    return base.TryInvokeMember(binder, args, out result);
        //}
        ///// <summary>
        ///// 调用 varo + varo; 时执行
        ///// dynamic varo = new VarObject();
        ///// </summary>
        ///// <param name="binder"></param>
        ///// <param name="arg"></param>
        ///// <param name="result"></param>
        ///// <returns></returns>
        //public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        //{
        //    return base.TryBinaryOperation(binder, arg, out result);
        //}
        ///// <summary>
        ///// 调用 varo++; 时执行
        ///// dynamic varo = new VarObject();
        ///// </summary>
        ///// <param name="binder"></param>
        ///// <param name="result"></param>
        ///// <returns></returns>
        //public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        //{
        //    return base.TryUnaryOperation(binder, out result);
        //}
        /// <summary>
        /// 调用 varo["key"]; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes == null || indexes.Length != 1)
            {
                throw new ArgumentException("indexes");
            }
            result = null;
            string key = indexes[0] as string;
            if (key != null)
            {
                getValue(key, out result);
            }
            else
            {
                throw new ArgumentException("indexes");
            }
            return true;
        }
        /// <summary>
        /// 调用 varo["key"] = "value"; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes == null || indexes.Length != 1)
            {
                throw new ArgumentException("indexes");
            }
            string key = indexes[0] as string;
            if (key != null)
            {
                setValue(key, value);
                return true;
            }
            else
            {
                throw new ArgumentException("indexes");
            }
        }

        /// <summary>
        /// 获取所有key
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return viewData.Keys;
        }
        /// <summary>
        /// 调用 varo.key; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            getValue(binder.Name, out result);
            return true;
        }
        /// <summary>
        /// 调用 varo.key = "value"; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            setValue(binder.Name, value);
            return true;
        }
        private void getValue(string key, out object result)
        {
            if (viewData.ContainsKey(key))
                result = viewData[key];
            else
                result = null;
        }
        private void setValue(string key, object value)
        {
            var oldValue = viewData.ContainsKey(key) ? viewData[key] : null;
            if (value == null)
            {
                if (viewData.ContainsKey(key))
                    viewData.Remove(key);
            }
            else
            {
                viewData[key] = value;
            }
            if (PropertyChanged != null && value != oldValue)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(key));
            }
        }
        #endregion DynamicObject
        #region IDictionary<string, object>
        public object this[string key]
        {
            get
            {
                    return viewData[key];
            }

            set
            {
                viewData[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return viewData.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (viewData as ICollection<KeyValuePair<string, object>>).IsReadOnly;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return viewData.Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return viewData.Values;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
             (viewData as ICollection<KeyValuePair<string, object>>).Add(item);
        }

        public void Add(string key, object value)
        {
            viewData.Add(key, value);
        }

        public void Clear()
        {
            viewData.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return viewData.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return viewData.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            (viewData as ICollection<KeyValuePair<string, object>>).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return viewData.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return (viewData as ICollection<KeyValuePair<string, object>>).Remove(item);
        }

        public bool Remove(string key)
        {
            return viewData.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return viewData.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return viewData.GetEnumerator();
        }
        #endregion IDictionary<string, object>
        #region 重写
        //public override int GetHashCode()
        //{
        //    return this.ToJson().GetHashCode();
        //}
        //public override bool Equals(object obj)
        //{
        //    if (obj is ExpandoObject2)
        //    {
        //        return this.ToJson() == obj.ToJson();
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public static bool operator ==(ExpandoObject2 a, ExpandoObject2 b)
        //{
        //    return a.Equals(b);
        //}
        //public static bool operator !=(ExpandoObject2 a, ExpandoObject2 b)
        //{
        //    return !a.Equals(b);
        //}
        //public string ToJson()
        //{
        //    return System.Web.Helpers.Json.Encode(this);
        //}
        #endregion 重写
    }
}