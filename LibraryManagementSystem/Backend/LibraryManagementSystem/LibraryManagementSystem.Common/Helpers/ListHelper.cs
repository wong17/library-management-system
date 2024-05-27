namespace LibraryManagementSystem.Common.Helpers
{
    public static class ListHelper
    {
        // Esta función hace magia
        public static IDictionary<TKey, List<TValue>>ListToDictionary<T, TKey, TValue>
            (List<T> list, Func<T, TKey> keySelector, Func<T, TValue> valueSelector) where TKey : notnull
        {
            var dictionary = new Dictionary<TKey, List<TValue>>();

            foreach (var item in list)
            {
                var key = keySelector(item);
                var value = valueSelector(item);
                // Sino hay una lista en esa posición...
                if (!dictionary.TryGetValue(key, out var valueList))
                {
                    // Crear nueva lista...
                    valueList = [];
                    dictionary[key] = valueList;
                }
                // Si ya hay una lista en esa posición, solo se agrega el valor
                valueList.Add(value);
            }

            return dictionary;
        }
    }
}
