using ProductTest.DTO;
using Application.Interfaces.Workstations;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Application.DTO;

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

        public WorkstationDTO Add(CreateWorkstationDTO workstation)
        {
            var mapped = _mapper.Map<Workstation>(workstation);
            _workstationRepository.Add(mapped);
            return _mapper.Map<WorkstationDTO>(mapped);
        }

        public void Delete(int id)
        {
            var workstationToRemove = _workstationRepository.Get().Where(x => x.Id == id).First();
            _workstationRepository.Delete(workstationToRemove);
        }

        public IEnumerable<WorkstationDTO> Get(GetWorkstationFilter? getWorkstationFilter = null)
        {
            var filter = _mapper.Map<GetWorkstationsQuery>(getWorkstationFilter);
            var filteredWorkstations = _workstationRepository.Get(filter);
            return _mapper.Map<IEnumerable<WorkstationDTO>>(filteredWorkstations);
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
