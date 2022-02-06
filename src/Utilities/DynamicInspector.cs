using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utilities
{
    public static class DynamicInspector
    {
        public static T GetDynamicValue<T>(dynamic data, string possiblePropertyName)
        {
            if (data == null)
            {
                return default;
            }

            try
            {
                var value = data[possiblePropertyName];
                if (value == null)
                {
                    return default;
                }

                if (value is JArray)
                {
                    return JsonConvert.SerializeObject(value);
                }

                return value;
            }
            catch
            {
                return default;
            }
        }

        public static T GetDynamicValue<T>(dynamic data, List<string> possiblePropertyNames)
        {
            //if (data == null)
            //{
            //    return default;
            //}

            foreach (var possiblePropertyName in possiblePropertyNames)
            {
                try
                {
                    var value = data[possiblePropertyName];
                    if (value == null)
                    {
                        continue;
                    }

                    if (value is JArray)
                    {
                        return JsonConvert.SerializeObject(value);
                    }

                    return value;
                }
                catch
                {
                    continue;
                }
            }

            return default;
        }

        public static T GetDynamicValueFromChild<T>(dynamic data, List<string> possibleDataPropertyNames, List<string> possibleSubPropertyNames)
        {
            if (data == null)
            {
                return default;
            }

            foreach (var possibleDataPropertyName in possibleDataPropertyNames)
            {
                try
                {
                    var value = data[possibleDataPropertyName];
                    if (value == null)
                    {
                        continue;
                    }

                    dynamic dataItem = null;

                    if (value is JArray)
                    {
                        var dataArray = value.ToObject<List<dynamic>>();
                        if (dataArray == null)
                        {
                            continue;
                        }

                        dataItem = ((List<dynamic>)dataArray).FirstOrDefault();
                    }
                    else if (value is JObject)
                    {
                        dataItem = value.ToObject<dynamic>();
                        if (dataItem == null)
                        {
                            continue;
                        }
                    }

                    T subValue = DynamicInspector.GetDynamicValue<T>(dataItem, possibleSubPropertyNames);
                    if (subValue == null)
                    {
                        return DynamicInspector.GetDynamicValueFromChild<T>(value, possibleDataPropertyNames, possibleSubPropertyNames);
                    }
                    else
                    {
                        return subValue;
                    }
                }
                catch
                {
                    continue;
                }
            }

            return default;
        }
    }
}
