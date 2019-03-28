using System;

namespace Leaf.POI
{
    [Serializable]
    public abstract class DataModelBase
    { 
        private long _id;
        private string _name;
        private bool _isSelected;

        public virtual long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }
    }
}
