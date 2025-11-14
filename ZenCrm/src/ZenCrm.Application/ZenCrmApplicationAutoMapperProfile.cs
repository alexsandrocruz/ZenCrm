using AutoMapper;
using ZenCrm.Books;

namespace ZenCrm;

public class ZenCrmApplicationAutoMapperProfile : Profile
{
    public ZenCrmApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
