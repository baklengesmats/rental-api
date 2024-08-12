using AutoMapper;
using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Repositories;
using System.Collections.Generic;

namespace RentalApp.Services
{
    public class RentalObjectService : IRentalObjectService
    {
        private readonly IRentalObjectRepository _rentalObjectRepository;
        private readonly IMapper _mapper;

        public RentalObjectService(IRentalObjectRepository rentalObjectRepository, IMapper mapper)
        {
            _rentalObjectRepository = rentalObjectRepository ?? throw new ArgumentNullException(nameof(rentalObjectRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CarDto> GetRentalObjectById(string registraionId)
        {
            var result = await _rentalObjectRepository.GetRentalObject(registraionId);
            return _mapper.Map<CarDto>(result);
        }

        public async Task UpdateRentalObject(CarDto rentalObject)
        {
            await _rentalObjectRepository.UpdateRentalObject(rentalObject);
        }


        public async Task<IEnumerable<CarDto>> GetAllRentalObjects(CarType? type)
        {
            return _mapper.Map<IEnumerable<CarDto>>(await _rentalObjectRepository.GetRentalObjects(null, null));
        }
    }
}
