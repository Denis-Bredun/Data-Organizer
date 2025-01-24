using Data_Organizer.Interfaces;
using System.ComponentModel;
using System.Reflection;

namespace Data_Organizer.Services
{
    public class EnumDescriptionResolverService : IEnumDescriptionResolverService
    {
        public string GetEnumDescription(Enum value)
        {
            var strValue = value.ToString();
            var field = value.GetType().GetField(strValue);
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? strValue : attribute.Description;
        }

        public T GetEnumValueFromDescription<T>(string description) where T : Enum
        {
            var field = typeof(T).GetFields()
                .FirstOrDefault(f =>
                f.GetCustomAttribute<DescriptionAttribute>()?.Description == description);

            if (field != null)
                return (T)field.GetValue(null);

            throw new ArgumentException($"Description '{description}' wasn't found in enum {typeof(T).Name}.", nameof(description));
        }
    }
}
