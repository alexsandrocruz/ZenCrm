using AutoMapper;
using ZenCrm.Books;

namespace ZenCrm.Web;

public class ZenCrmWebAutoMapperProfile : Profile
{
    public ZenCrmWebAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();
        
        //Define your object mappings here, for the Web project
    }
}
