using AutoMapper;
using ZenCrm.Books;
using ZenCrm.Sales;

namespace ZenCrm;

public class ZenCrmApplicationAutoMapperProfile : Profile
{
    public ZenCrmApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();

        // Sales mappings
        CreateMap<Client, ClientDto>();
        CreateMap<CreateUpdateClientDto, Client>();

        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();

        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
