namespace AutoProxy.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommonUtilities;

    public class MethodDefinitionComparer : IEqualityComparer<KeyValuePair<string, Type[]>>
    {

        public bool Equals(KeyValuePair<string, Type[]> x, KeyValuePair<string, Type[]> y)
        {
            
            bool equal = true;
            equal = x.Key != null && y.Key != null && x.Key == y.Key;
            if (equal)
            {
                
                equal = 
                    (y.Value != null || x.Value == null) &&
                    (x.Value != null || y.Value == null) 
                    && (
                    (y.Value == null && x.Value == null) ||
                    (y.Value.Length == 0 && x.Value.Length == 0 ) ||
                        (y.Value.Length == x.Value.Length &&
                        x.Value.Intersect(y.Value).FirstOrDefault() != default(Type)));
            }

            return equal;
        }

        public int GetHashCode(KeyValuePair<string, Type[]> obj)
        {
            List<object> objects = new List<object>();
            objects.Add(obj.Key);
            objects.AddRange(obj.Value);
            return objects.ToArray().GetArrayHashCode();
        }
    }
}
