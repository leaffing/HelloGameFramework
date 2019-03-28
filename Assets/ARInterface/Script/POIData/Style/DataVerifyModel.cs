using Leaf.POI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Leaf.POI.Style
{
    public abstract class DataVerifyModel : DataModelBase , IDataErrorInfo
    {

        protected DataVerifyModel(Type typeOfChild)
        {
            var ac = typeOfChild.GetMembers().ToList();
            Errors = new Dictionary<string, string>();
            foreach (var memberInfo in ac)
            {
                if (memberInfo.MemberType == MemberTypes.Property)
                {
                    Errors.Add(memberInfo.Name, "");
                }
            }
        }

        protected Dictionary<string, string> Errors;

        protected abstract void OnPostValidate(string columnName);

        private string _validateResult;
        protected abstract string VerifyColumnData(string columnName);

        public string this[string columnName]
        {
            get
            {
                Errors[columnName] = VerifyColumnData(columnName);
                OnPostValidate(columnName);
                _validateResult = Errors[columnName];
                return Errors[columnName];
            }
        }

        public bool GetHasError(string columnName)
        {
            return !string.IsNullOrEmpty(Errors[columnName]);
        }

        public string GetError(string columnName)
        {
            return Errors[columnName];
        }

        public bool HasError => Errors.Any(x => !string.IsNullOrEmpty(x.Value));

        public string Error => _validateResult;
    }
}
