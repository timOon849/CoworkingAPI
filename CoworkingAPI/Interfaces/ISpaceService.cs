using CoworkingAPI.Model;

namespace CoworkingAPI.Interfaces
{
    public interface ISpaceService
    {
        Task<Space> GetSpaceById(int id);
        Task<IEnumerable<Space>> GetAllSpaces(SpaceFilter filter, Pagination pagination);
        Task<Space> CreateSpace(Space space);
        Task<Space> UpdateSpace(int id, Space space);
        Task<bool> DeleteSpace(int id);
        Task<Workplace> AddWorkplaceToSpace(int spaceId, Workplace workplace);
        Task<IEnumerable<Workplace>> GetWorkplacesBySpaceId(int spaceId);
        Task<bool> DeleteWorkplace(int workplaceId);
    }

    public class SpaceFilter
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class Pagination
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
