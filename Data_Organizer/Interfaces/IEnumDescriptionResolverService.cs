namespace Data_Organizer.Interfaces
{
    public interface IEnumDescriptionResolverService
    {
        string GetEnumDescription(Enum value);
        T GetEnumValueFromDescription<T>(string description) where T : Enum;
    }
}
