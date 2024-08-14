using System.Collections.Generic;

namespace Tunny.Util.RhinoComputeWrapper
{
    public class GrasshopperDataTree
    {
        public GrasshopperDataTree(string paramName)
        {
            ParamName = paramName;
            InnerTree = new Dictionary<string, List<GrasshopperObject>>();
        }

        public string ParamName { get; }


        public Dictionary<string, List<GrasshopperObject>> InnerTree { get; set; }

        public List<GrasshopperObject> this[string key]
        {
            get
            {
                return ((IDictionary<string, List<GrasshopperObject>>)InnerTree)[key];
            }

            set
            {
                ((IDictionary<string, List<GrasshopperObject>>)InnerTree)[key] = value;
            }
        }

        public bool Contains(GrasshopperObject item)
        {

            foreach (List<GrasshopperObject> list in InnerTree.Values)
            {
                if (list.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void Append(List<GrasshopperObject> items, GrasshopperPath GhPath)
        {
            Append(items, GhPath.ToString());
        }

        public void Append(List<GrasshopperObject> items, string GhPath)
        {

            if (!InnerTree.TryGetValue(GhPath, out List<GrasshopperObject> value))
            {
                value = new List<GrasshopperObject>();
                InnerTree.Add(GhPath, value);
            }

            value.AddRange(items);
        }

        public void Append(GrasshopperObject item, GrasshopperPath path)
        {
            Append(item, path.ToString());
        }

        public void Append(GrasshopperObject item, string GhPath)
        {
            if (!InnerTree.TryGetValue(GhPath, out List<GrasshopperObject> value))
            {
                value = new List<GrasshopperObject>();
                InnerTree.Add(GhPath, value);
            }

            value.Add(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, List<GrasshopperObject>>)InnerTree).ContainsKey(key);
        }

        public void Add(string key, List<GrasshopperObject> value)
        {
            ((IDictionary<string, List<GrasshopperObject>>)InnerTree).Add(key, value);
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, List<GrasshopperObject>>)InnerTree).Remove(key);
        }

        public bool TryGetValue(string key, out List<GrasshopperObject> value)
        {
            return ((IDictionary<string, List<GrasshopperObject>>)InnerTree).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, List<GrasshopperObject>> item)
        {
            ((IDictionary<string, List<GrasshopperObject>>)InnerTree).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<string, List<GrasshopperObject>>)InnerTree).Clear();
        }

        public bool Contains(KeyValuePair<string, List<GrasshopperObject>> item)
        {
            return ((IDictionary<string, List<GrasshopperObject>>)InnerTree).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, List<GrasshopperObject>>[] array, int arrayIndex)
        {
            ((IDictionary<string, List<GrasshopperObject>>)InnerTree).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, List<GrasshopperObject>> item)
        {
            return ((IDictionary<string, List<GrasshopperObject>>)InnerTree).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, List<GrasshopperObject>>> GetEnumerator()
        {
            return ((IDictionary<string, List<GrasshopperObject>>)InnerTree).GetEnumerator();
        }
    }
}

