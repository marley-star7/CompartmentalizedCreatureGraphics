namespace CompartmentalizedCreatureGraphics.Cosmetics;

public class CreatureCosmeticLayerContractResolver<CosmeticLayerType> : DefaultContractResolver where CosmeticLayerType : struct, Enum
{
    protected override JsonProperty CreateProperty(
        MemberInfo member, 
        MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        
        if (property.PropertyName == "layerName" && 
            property.PropertyType == typeof(int))
        {
            property.Converter = new EnumToIntJsonConverter<CosmeticLayerType>();
        }
        
        return property;
    }
}
