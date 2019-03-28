using System;

namespace Leaf.POI.Style
{
    public class IconConfig
    {
        private string _name;
        private string _iconPath;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string IconPath
        {
            get { return _iconPath; }
            set { _iconPath = value; }
        }

        internal event Action<IconConfig> RequestKill;

        public void Remove()
        {
            RequestKill?.Invoke(this);
        }
    }
}