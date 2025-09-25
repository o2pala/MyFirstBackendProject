using TodoApi.ViewModels;

namespace TodoApi.Repository
{
    public interface IContactRepository
    {
        Task <PagedDataResult<ContactQueryResponse>> Index(PagedDataQuery<ContactQueryRequest> query);
        Task <ContactQueryResponse> GetDetail(Guid id);
        Task <Contact> CreateContact(ContactCreateRequest contact);
        Task<Contact> UpdateContact(ContactUpdateRequest contact);
        Task DeleteContact(Guid id);
        Task MultiDelete(Guid[] ids);
    }
}
