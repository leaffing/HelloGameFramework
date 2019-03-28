namespace Leaf.POI
{
    public class TagParameter
    {
        private string _code;
        private object _value;
        private TagParameterType _type;
        private string _name;

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public TagParameterType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
