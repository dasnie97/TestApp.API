using Application.DTO.Workstations;
using Application.Interfaces.Workstations;
using AutoMapper;
using Domain.Interfaces.Workstations;
using Domain.Models.Workstations;

namespace Application.Services.Workstations
{
    public class WorkstationService : IWorkstationService
    {
        private readonly IWorkstationRepository _workstationRepository;
        private readonly IMapper _mapper;
        public WorkstationService(IWorkstationRepository workstationRepository, IMapper mapper)
        {
            _mapper= mapper;
            _workstationRepository= workstationRepository;
        }

        public WorkstationDTO Add(AddWorkstationDTO workstation)
        {
            var mapped = _mapper.Map<Workstation>(workstation);
            _workstationRepository.Add(mapped);
            return _mapper.Map<WorkstationDTO>(mapped);
        }

        public IEnumerable<WorkstationDTO> Get()
        {
            var workstations = _workstationRepository.Get();
            return _mapper.Map<IEnumerable<WorkstationDTO>>(workstations);
        }

        public WorkstationDTO Update(WorkstationDTO workstation)
        {
            var mapped = _mapper.Map<Workstation>(workstation);
            _workstationRepository.Update(mapped);
            return workstation;
        }
    }
}
