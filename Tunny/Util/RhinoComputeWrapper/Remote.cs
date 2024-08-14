using System.Collections.Generic;

namespace Tunny.Util.RhinoComputeWrapper
{
    public class Remote<T>
    {
        readonly string _url;
        readonly T _data;

        public Remote(string url)
        {
            _url = url;
        }

        public Remote(T data)
        {
            _data = data;
        }

        public object JsonObject()
        {
            if (_url != null)
            {
                var dict = new Dictionary<string, string>
                {
                    ["url"] = _url
                };
                return dict;
            }
            return _data;
        }
    }
}
