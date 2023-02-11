using AutoMapper;

namespace TeachBoard.Application.Mappings;

// inheritors mappings profiles realized at Mapping method will
// be invoked automatically by AssemblyMappingProfile class
public interface IMappable
{
    public void Mapping(Profile profile);
}