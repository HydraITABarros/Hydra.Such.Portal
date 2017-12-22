using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hydra.Such.Portal.Configurations
{
    public class EnumHelper
    {
        public static List<KeyValuePair<int, string>> GetItemsFor(Type enumType, bool excludeEmptyDescription = true)
        {
            List<KeyValuePair<int, string>> items = new List<KeyValuePair<int, string>>();

            //TODO: Ensure Type is System.Enum
            if (enumType.BaseType != typeof(Enum))
                return items;

            Array values = System.Enum.GetValues(enumType);

            foreach (int value in values)
            {
                string displayName = GetDescriptionFor(enumType, value);// Enum.GetName(enumType, value);
                if (string.IsNullOrEmpty(displayName) && excludeEmptyDescription)
                    continue;
                items.Add(new KeyValuePair<int, string>(value, displayName));
            }
            return items.OrderBy(x => x.Value).ToList();
        }

        public static string GetDescriptionFor(Type enumType, string value)
        {
            //TODO: Ensure Type is System.Enum
            if (enumType.BaseType != typeof(Enum))
                return string.Empty;

            int id;
            if (int.TryParse(value, out id))
            {
                return GetDescriptionFor(enumType, id);
            }
            return string.Empty;
        }

        public static string GetDescriptionFor(Type enumType, int value)
        {

            //TODO: Ensure Type is System.Enum
            if (enumType.BaseType != typeof(Enum))
                return string.Empty;

            //tmp.b
            var enumValue = Enum.ToObject(enumType, value);

            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumValue.ToString();

            //tmp.e
            //return Enum.GetName(enumType, value);
        }

        public static int GetValueByDescription(Type enumType, string decription)
        {
            //TODO: Ensure Type is System.Enum
            if (enumType.BaseType != typeof(Enum))
                return -1;

            var values = Enum.GetValues(enumType);

            //Enum.Parse(typeof(aa), name);

            foreach (var item in values)
            {
                string itemDescription = GetDescriptionFor(enumType, item.ToString());
                if (itemDescription.ToLower().Equals(decription.ToLower()))
                {
                    return (int)item;
                }

            }
            return -1;
        }

        public static bool ValidateRange(Type enumType, int value)
        {
            //TODO: Ensure Type is System.Enum
            if (!enumType.IsEnum)//.BaseType != typeof(Enum))
                return false;

            var min = Enum.GetValues(enumType).Cast<int>().Min();
            var max = Enum.GetValues(enumType).Cast<int>().Max();

            return (value >= min && value <= max);
        }

        public static List<string> GetItemsDescriptionFor(Type type)
        {
            var descs = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    descs.Add(fd.Description);
                }
            }
            return descs;
        }

        public static string GetStatusDescriptionFor(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static Dictionary<string, int> GetItemsAsDictionary(Type enumType, bool excludeEmptyDescription = true)
        {
            Dictionary<string, int> items = new Dictionary<string, int>();
            foreach (var item in Enum.GetValues(enumType))
            {
                items.Add(item.ToString(), (int)item);
            }
            return items;
        }
    }
}
