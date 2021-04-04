
namespace DMC.DAL.Script
{
    public interface IPermission
    {
        string GetMainMenu { get; }
        string GetMyCompany { get; }
        string GetMenuStatusByUser { get; }
        string GetMenuStatusByRose { get; }
        string GetMyMenu { get; }
        string GetMyPgmOpt { get; }
    }
}
