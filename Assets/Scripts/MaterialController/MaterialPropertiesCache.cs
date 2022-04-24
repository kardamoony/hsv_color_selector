using System.Collections.Generic;
using UnityEngine;

namespace MaterialController
{
    public class MaterialPropertiesCache
    {
        private readonly Dictionary<string, int> _propertiesDictionary;

        public MaterialPropertiesCache(IEnumerable<string> properties)
        {
            _propertiesDictionary = new Dictionary<string, int>();

            foreach (var propertyName in properties)
            {
                _propertiesDictionary.Add(propertyName, Shader.PropertyToID(propertyName));
            }
        }

        public bool GetPropertyId(string propertyName, out int propertyId)
        {
            return _propertiesDictionary.TryGetValue(propertyName, out propertyId);
        }
    }
}
