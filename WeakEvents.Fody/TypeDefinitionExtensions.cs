using Mono.Cecil;
using System.Linq;

namespace WeakEvents.Fody
{
    internal static class TypeDefinitionExtensions
    {
        public static bool IsTypeToProcess(this TypeDefinition typeDefinition)
        {
            return typeDefinition.HasEvents && !typeDefinition.IsInterface && HasWeakEventAttribute(typeDefinition);
        }

        private static bool HasWeakEventAttribute(TypeDefinition typeDefinition)
        {
            return typeDefinition.CustomAttributes.Any(attr => "WeakEvents.ImplementWeakEventsAttribute".Equals(attr.AttributeType.FullName));
        }
    }
}