using ProjectPlanner_API.Models;

namespace ProjectPlanner_API.IMethod
{
    public interface IAccount
    {
        Task<List<RegistrationModel>> ValidateUsername(string username);
    }
}
