using System.Reflection;
using AutoMapper;

namespace TeachBoard.EducationService.Application.Mappings;

// Class for auto invokation mapping profiles at IMappable inheritors
public class AssemblyMappingProfile : Profile
{
    public AssemblyMappingProfile(Assembly assembly)
        => ApplyMappingsFromAssembly(assembly);
    
    // Method invoked in main. 
    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        // Get all types inheritors of IMappable
        var mappableTypes = assembly.GetExportedTypes()
            .Where(type => type.GetInterfaces()
                .Any(i => i == typeof(IMappable)))
            .ToList();
        
        // Run through IMappable inheritors and invoke Mapping method
        foreach (var mappableType in mappableTypes)
        {
            var instance = Activator.CreateInstance(mappableType);
            
            var method = mappableType.GetMethod("Mapping");
            
            method?.Invoke(instance, new object?[] { this });
        }
    }
}