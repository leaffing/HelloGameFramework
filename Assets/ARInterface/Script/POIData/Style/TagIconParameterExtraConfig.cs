using System.Linq;
using LitJson;
using System.Collections.Generic;

namespace Leaf.POI.Style
{
    public class TagIconParameterExtraConfig : ITagExtraConfig
    {
        private List<IconConfig> _iconPathPairs = new List<IconConfig>();

        public List<IconConfig> IconPathPairs
        {
            get
            {
                if (_iconPathPairs == null || !_iconPathPairs.Any()) return _iconPathPairs;
                foreach (var iconPathPair in _iconPathPairs)
                {
                    iconPathPair.RequestKill -= RemovePairs;
                    iconPathPair.RequestKill += RemovePairs;
                }
                return _iconPathPairs;
            }
            set
            {
                if (_iconPathPairs != null && _iconPathPairs.Any())
                {
                    foreach (var iconPathPair in _iconPathPairs)
                    {
                        iconPathPair.RequestKill -= RemovePairs;
                    }
                }
                _iconPathPairs = value;
                if (_iconPathPairs != null&&_iconPathPairs.Any())
                {
                    foreach (var iconPathPair in _iconPathPairs)
                    {
                        iconPathPair.RequestKill += RemovePairs;
                    }
                }
            }
        }

        public void AddPairs()
        {
            var pair = new IconConfig();
            pair.RequestKill += RemovePairs;
            IconPathPairs.Add(pair);
        }

        public void RemovePairs(IconConfig kv)
        {
            kv.RequestKill -= RemovePairs;
            if (IconPathPairs.Contains(kv))
                IconPathPairs.Remove(kv);
        }

        public string GetConfigJson()
        {
            return JsonMapper.ToJson(this);
        }
    }
}
