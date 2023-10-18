namespace ShBarcelona.Services.Area
{
    public interface IAreaService
    {
        Task<AreaDto> AddAsync(AreaDto areaDto);
        Task<IEnumerable<AreaDto>> GetAllAsync();
        Task<AreaDto> GetByAreaIdAsync(int areaId);

    }
}
