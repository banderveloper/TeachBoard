using System.Reflection;
using AutoMapper;

namespace TeachBoard.MembersService.Application.Mappings;

/// <summary>
/// Class for auto invokation mapping profiles at IMappable inheritors
/// </summary>
public class AssemblyMappingProfile : Profile
{
    public AssemblyMappingProfile(Assembly assembly)
        => ApplyMappingsFromAssembly(assembly);
    
    /// <summary>
    /// Automatically invokes mapping profiles creations in IMappable inheritors 
    /// </summary>
    /// <param name="assembly">Assembly containing mapping types</param>
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