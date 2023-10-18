using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NLog;
using ShBarcelona.DAL;
using ShBarcelona.DAL.Entities;

namespace ShBarcelona.Services.Area
{
    public class AreaService : IAreaService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ShBarcelonaContext _shBarcelonaContext;

        public AreaService(
            ILogger logger,
            IMapper mapper,
            ShBarcelonaContext shBarcelonaContext
            )
        {
            _logger = logger;
            _mapper = mapper;
            _shBarcelonaContext = shBarcelonaContext;
        }

        public async Task<AreaDto> AddAsync(AreaDto areaDto)
        {
            _logger.Debug($"AreaService > AddAsync");

            if (_shBarcelonaContext.Areas.Any(s => s.Name == areaDto.Name))
            {
                _logger.Warn($"Area with name {areaDto.Name} already exists.");
                throw new Exception($"Area with name {areaDto.Name} already exists.");
            }

            AreaEntity entity = _mapper.Map<AreaEntity>(areaDto);

            await _shBarcelonaContext.AddAsync(entity);
            await _sh.SaveChangesAsync();

            return _mapper.Map<AreaDto>(entity);
        }

        public async Task<IEnumerable<AreaDto>> GetAllAsync()
        {
            _logger.Debug($"AreaService > GetAllAsync");

            var listCustomerDto = await _shBarcelonaContext.Areas.AsNoTracking()
                .Select(x => _mapper.Map<AreaDto>(x))
                .ToListAsync();

            if (listCustomerDto is null)
            {
                _logger.Error($"Error getting all areas");
                throw new Exception($"Error getting all areas");
            }

            return listCustomerDto;
        }

        public async Task<AreaDto> GetByAreaIdAsync(int areaId)
        {
            _logger.Debug($"AreaService > GetByAreaIdAsnyc > AreaId {areaId}");

            var customerDto = await _shBarcelonaContext.Areas.AsNoTracking()
                .Where(x => x.Id.Equals(areaId))
                .Select(x => _mapper.Map<AreaDto>(x))
                .FirstOrDefaultAsync();

            if (customerDto is null)
            {
                _logger.Error($"Error getting area with id {areaId}");
                throw new Exception($"Error getting area with id {areaId}");
            }

            return customerDto;
        }

    }
}
