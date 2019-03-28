namespace Leaf.POI.Style
{
    /// <summary>
    /// 数据订阅标签额外数据(暂时不可用，待完成)
    /// </summary>
    public class DataSubscribeTagExtraConfig : ITagExtraConfig
    {
        /*
        /// <summary>
        /// 订阅信息入口
        /// </summary>
        /// <param name="jsonRaw"></param>
        internal void AddItem(string jsonRaw)
        {
            if (CriticalContent == null) return;
            try
            {
                var jo = JsonConvertor.DeserializeAndFlatten(jsonRaw);

                if (jo == null) return;
                if (jo.ContainsKey("Content") && jo["Content"] is string content)
                {
                    try
                    {
                        var j2 = JsonConvertor.DeserializeAndFlatten(content);
                        if (j2 != null)
                        {
                            foreach (var o in j2)
                            {
                                jo.Add("Content." + o.Key, o.Value);
                            }
                        }
                    }
                    catch
                    {
                        //
                    }
                }
                if (!IsDataAccepted(jo)) return;
                var dataContent = new DataSubscribeDisplayContent()
                {
                    DataSource = jsonRaw,
                    DisplayContentA = new TagParameter(),
                    DisplayContentB = new TagParameter(),
                    DisplayContentC = new TagParameter(),
                    DisplayContentD = new TagParameter(),
                    DisplayContentE = new TagParameter()
                };

                dataContent.DisplayContentA.Value = GetValue(CriticalContent.DisplayContentA, jo);
                dataContent.DisplayContentB.Value = GetValue(CriticalContent.DisplayContentB, jo);
                dataContent.DisplayContentC.Value = GetValue(CriticalContent.DisplayContentC, jo);
                dataContent.DisplayContentD.Value = GetValue(CriticalContent.DisplayContentD, jo);
                dataContent.DisplayContentE.Value = GetValue(CriticalContent.DisplayContentE, jo);

                if (!IsCriticalEnabled || CriticalDataValidate == null || !jo.ContainsKey(CriticalDataValidate.ValidateField) || !IsAccepted(jo, CriticalDataValidate, false))
                {

                    AddToNormal(dataContent);
                }
                else
                {
                    CriticalContent = dataContent;
                }
            }
            catch
            {
                //
            }
        }
        /// <summary>
        /// 父ViewModel
        /// </summary>
        [JsonIgnore]
        public TagViewModel ParentModel { get; set; }

        private static string GetReferenceCodeValue(string code, TagViewModel model)
        {
            if (model == null) return "";
            switch (code)
            {
                case nameof(TagViewModel.Name):
                    return model.Name;
                case nameof(TagViewModel.UpdateTime):
                    return model.UpdateTime.ToString(CultureInfo.InvariantCulture);
            }

            if (model.Parameters == null) return "";
            foreach (var modelParameter in model.Parameters)
            {
                if (modelParameter.Code == code) return modelParameter.Value?.ToString();
            }

            return "";
        }

        private bool IsDataAccepted(Dictionary<string, object> kvs)
        {
            if (FilterCriterias == null || !FilterCriterias.Any()) return true;
            foreach (var dataSubscribeTagValidateCriteria in FilterCriterias)
            {
                if (!IsAccepted(kvs, dataSubscribeTagValidateCriteria, false)) return false;
            }

            return true;
        }

        private bool IsAccepted(Dictionary<string, object> kvs, DataSubscribeTagValidateCriteria criteria, bool criteriaEmptyDefault)
        {
            if (criteria == null) return criteriaEmptyDefault;
            if (string.IsNullOrEmpty(criteria.ValidateField)) return criteriaEmptyDefault;
            if (!kvs.ContainsKey(criteria.ValidateField)) return false;
            var value = kvs[criteria.ValidateField];

            var targetValue = criteria.TargetValue;
            if (criteria.ValidateValueMode == DataSubscribeTagValidateValueMode.ReferenceCode &&
                string.IsNullOrEmpty(criteria.TargetValue)) return false;
            if (criteria.ValidateValueMode == DataSubscribeTagValidateValueMode.ReferenceCode)
                targetValue = GetReferenceCodeValue(targetValue, ParentModel);
            switch (criteria.ValidateMode)
            {
                case DataSubscribeTagValidateMode.MoreThan:
                    {
                        if (double.TryParse(targetValue, out var target))
                        {
                            return false;
                        }
                        if (value is double source) return source > target;
                        if (double.TryParse(value?.ToString(), out source)) return source > target;
                    }
                    break;
                case DataSubscribeTagValidateMode.LessThan:
                    {
                        if (double.TryParse(targetValue, out var target))
                        {
                            return false;
                        }
                        if (value is double source) return source < target;
                        if (double.TryParse(value?.ToString(), out source)) return source < target;
                    }
                    break;
                case DataSubscribeTagValidateMode.Equal:
                    {
                        if (value?.ToString() is string source) return string.Equals(source, targetValue);
                        return value == null && string.IsNullOrEmpty(targetValue);
                    }
                case DataSubscribeTagValidateMode.NotEqual:
                    {
                        if (value?.ToString() is string source) return !string.Equals(source, targetValue);
                        return value == null && !string.IsNullOrEmpty(targetValue);
                    }
                case DataSubscribeTagValidateMode.Contains:
                    {
                        if (value?.ToString() is string source) return targetValue.Contains(source);
                    }
                    break;
                default:
                    return false;
            }

            return false;
        }
        /// <summary>
        /// 是否显示关键告警
        /// </summary>
        public bool IsCriticalEnabled
        {
            get => _isCriticalEnabled;
            set
            {
                _isCriticalEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isCriticalEnabled;

        private void AddToNormal(DataSubscribeDisplayContent item)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (DisplayContent == null) DisplayContent = new ObservableCollection<DataSubscribeDisplayContent>();

                if (DisplayContent.Count >= MaxWaitList)
                    DisplayContent.RemoveAt(MaxWaitList - 1);
                DisplayContent.Insert(0, item);
            });
        }

        private object GetValue(TagParameter content, Dictionary<string, object> kvs)
        {
            if (content?.Code == null) return null;
            if (!kvs.ContainsKey(content.Code)) return null;
            var item = kvs[content.Code];
            if (!content.Code.ToLower().Contains("datetime")) return kvs[content.Code];
            if (!int.TryParse(item?.ToString(), out var result)) return item;
            return result > 0 ? DateTime.Parse(System.DateTime.Now.ToString("1970-01-01 00:00:00")).AddSeconds(result + 3600 * 8) : DateTime.MinValue;
        }

        private bool _isItemDetailEnabled;
        /// <summary>
        /// 是否可点出详情
        /// </summary>
        public bool IsItemDetailEnabled
        {
            get { return _isItemDetailEnabled; }
            set
            {
                _isItemDetailEnabled = value;
                OnPropertyChanged();
            }
        }

        private int _maxWaitList;
        /// <summary>
        /// 最大缓存元素
        /// </summary>
        public int MaxWaitList
        {
            get { return _maxWaitList; }
            set
            {
                _maxWaitList = value;
                OnPropertyChanged();
            }
        }
        private string _normalTitle;
        /// <summary>
        /// 常规消息标题
        /// </summary>
        public string NormalTitle
        {
            get { return _normalTitle; }
            set
            {
                _normalTitle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 过滤器集合
        /// </summary>
        public List<DataSubscribeTagValidateCriteria> FilterCriterias
        {
            get => _filterCriterias;
            set
            {
                _filterCriterias = value;
                OnPropertyChanged();
            }
        }

        private List<DataSubscribeTagValidateCriteria> _filterCriterias;

        private string _criticalTitle;
        /// <summary>
        /// 关键消息标题
        /// </summary>
        public string CriticalTitle
        {
            get { return _criticalTitle; }
            set
            {
                _criticalTitle = value;
                OnPropertyChanged();
            }
        }

        private DataSubscribeDisplayType _displayType;
        /// <summary>
        /// 展示类型
        /// </summary>
        public DataSubscribeDisplayType DisplayType
        {
            get { return _displayType; }
            set
            {
                _displayType = value;
                OnPropertyChanged();
            }
        }
        private DataSubscribeDisplayContent _criticalContent;
        /// <summary>
        /// 关键数据
        /// </summary>
        public DataSubscribeDisplayContent CriticalContent
        {
            get { return _criticalContent; }
            set
            {
                _criticalContent = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<DataSubscribeDisplayContent> _displayContent;
        /// <summary>
        /// 展示元素
        /// </summary>
        public ObservableCollection<DataSubscribeDisplayContent> DisplayContent
        {
            get { return _displayContent; }
            set
            {
                _displayContent = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        private string _subject;

        /// <summary>
        /// 关键数据字段
        /// </summary>
        public DataSubscribeTagValidateCriteria CriticalDataValidate
        {
            get => _criticalDataValidate;
            set
            {
                _criticalDataValidate = value;
                OnPropertyChanged();
            }
        }

        private DataSubscribeTagValidateCriteria _criticalDataValidate;

        /// <summary>
        /// 通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 发起通知事件
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        */
        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        public string GetConfigJson()
        {
            return LitJson.JsonMapper.ToJson(this);
        }
    }

    /*
    /// <summary>
    /// 验证值验证模式
    /// </summary>
    public enum DataSubscribeTagValidateValueMode
    {
        /// <summary>
        /// 值验证
        /// </summary>
        Value,
        /// <summary>
        /// 寻找引用
        /// </summary>
        ReferenceCode,
    }

    /// <summary>
    /// 数据标签验证条件
    /// </summary>
    public class DataSubscribeTagValidateCriteria : INotifyPropertyChanged
    {
        /// <summary>
        /// 验证字段
        /// </summary>
        public string ValidateField
        {
            get => _validateField;
            set
            {
                _validateField = value;
                OnPropertyChanged();
            }
        }

        private string _validateField;

        /// <summary>
        /// 验证模式
        /// </summary>
        public DataSubscribeTagValidateMode ValidateMode
        {
            get => _validateMode;
            set
            {
                _validateMode = value;
                OnPropertyChanged();
            }
        }

        private DataSubscribeTagValidateMode _validateMode;

        /// <summary>
        /// 验证值模式
        /// </summary>
        public DataSubscribeTagValidateValueMode ValidateValueMode
        {
            get => _validateValueMode;
            set
            {
                _validateValueMode = value;
                OnPropertyChanged();
            }
        }

        private DataSubscribeTagValidateValueMode _validateValueMode;

        /// <summary>
        /// 验证目标值
        /// </summary>
        public string TargetValue
        {
            get => _targetValue;
            set
            {
                _targetValue = value;
                OnPropertyChanged();
            }
        }

        private string _targetValue;

        /// <summary>
        /// 通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 发起通知事件
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    */

}
