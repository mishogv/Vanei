namespace MIS.ServicesModels
{
    using Models;

    using Services.Mapping;

    public class InvitationServiceModel : IMapFrom<Invitation>
    {
    public int Id { get; set; }


    public string UserId { get; set; }
    public MISUser User { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; }
    }
}