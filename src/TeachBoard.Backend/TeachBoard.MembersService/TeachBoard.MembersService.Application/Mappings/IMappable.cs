using AutoMapper;

namespace TeachBoard.MembersService.Application.Mappings;

/// <summary>
/// Inheritors mappings profiles realized at Mapping method will
/// be invoked automatically by AssemblyMappingProfile class
/// </summary>
public interface IMappable
{
    void Mapping(Profile profile);
}