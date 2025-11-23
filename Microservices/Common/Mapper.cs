using System.Reflection;

namespace Common
{
    public static class Mapper
    {
        public static TDestination Map<TDestination>(object source) where TDestination : new()
        {
            // Creates a new instance of the TDestination (a Dto)
            TDestination destination = new TDestination();

            // Gets all information about the source via Reflection. Creates a Dictonary from it, so its quicker to lookup later
            Dictionary<string, PropertyInfo> sourcePropertiesAsDictonary = source.GetType().GetProperties().ToDictionary(p => p.Name);

            // Gets all information about the Dto via Reflection
            PropertyInfo[] tDestinationProperties = typeof(TDestination).GetProperties();

            // Iterates through all Dtos properties
            foreach (PropertyInfo tDestinationProperty in tDestinationProperties)
            {
                // If the Dtos property is a public set. Continue
                if (!tDestinationProperty.CanWrite) continue;

                // Find a Property in source which has the same name. Such as Dto.Id and Source.Id
                if (sourcePropertiesAsDictonary.TryGetValue(tDestinationProperty.Name, out var sourceProperty))
                {
                    // Checks if desitnationProperty is of same type as sourceProperty. Example is both ints or is one string and the other an int
                    if (!tDestinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)) continue;

                    // Sets the destinationProperty on the implementation of TDestiantion with the souceproperty from sourceImplementation
                    tDestinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                }
            }

            return destination;
        }
    }
}
