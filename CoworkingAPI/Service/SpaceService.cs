using CoworkingAPI.DataBase;
using CoworkingAPI.Interfaces;
using CoworkingAPI.Model;
using Microsoft.EntityFrameworkCore;

public class SpaceService : ISpaceService
{
    private readonly ContextDB _context;

    public SpaceService(ContextDB context)
    {
        _context = context;
    }

    // CRUD операции для Space
    public async Task<Space> GetSpaceById(int id)
    {
        return await _context.Spaces
            .Include(s => s.Owner)
            .Include(s => s.Workplaces)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Space>> GetAllSpaces(SpaceFilter filter, Pagination pagination)
    {
        var query = _context.Spaces
            .Include(s => s.Owner)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(s => s.Name.Contains(filter.Name));

        if (!string.IsNullOrEmpty(filter.Address))
            query = query.Where(s => s.Address.Contains(filter.Address));

        return await query
            .OrderBy(s => s.Name)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
    }

    public async Task<Space> CreateSpace(Space space)
    {
        _context.Spaces.Add(space);
        await _context.SaveChangesAsync();
        return space;
    }
    public async Task<bool> DeleteWorkplace(int workplaceId)
    {
        var workplace = await _context.Workplaces.FindAsync(workplaceId);
        if (workplace == null)
            return false;

        _context.Workplaces.Remove(workplace);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Space> UpdateSpace(int id, Space space)
    {
        var existingSpace = await GetSpaceById(id);
        if (existingSpace == null)
            throw new Exception("Space not found");

        existingSpace.Name = space.Name;
        existingSpace.Description = space.Description;
        existingSpace.Address = space.Address;
        existingSpace.OwnerId = space.OwnerId;

        _context.Spaces.Update(existingSpace);
        await _context.SaveChangesAsync();

        return existingSpace;
    }

    public async Task<bool> DeleteSpace(int id)
    {
        var space = await GetSpaceById(id);
        if (space == null)
            return false;

        _context.Spaces.Remove(space);
        await _context.SaveChangesAsync();
        return true;
    }

    // Добавление и получение рабочих мест
    public async Task<Workplace> AddWorkplaceToSpace(int spaceId, Workplace workplace)
    {
        var space = await _context.Spaces.FindAsync(spaceId);
        if (space == null)
            throw new Exception("Space not found");

        workplace.SpaceId = spaceId;
        _context.Workplaces.Add(workplace);
        await _context.SaveChangesAsync();

        return workplace;
    }

    public async Task<IEnumerable<Workplace>> GetWorkplacesBySpaceId(int spaceId)
    {
        return await _context.Workplaces
            .Where(w => w.SpaceId == spaceId)
            .ToListAsync();
    }
}