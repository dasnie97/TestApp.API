using TestEngineering.DTO;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Application.DTO;
using Application.Interfaces;

namespace Application.Services;

public class WorkstationService : IWorkstationService
{
    private readonly IWorkstationRepository _workstationRepository;
    private readonly IMapper _mapper;
    public WorkstationService(IWorkstationRepository workstationRepository, IMapper mapper)
    {
        _mapper = mapper;
        _workstationRepository = workstationRepository;
    }

    public WorkstationDTO Add(CreateWorkstationDTO workstation)
    {
        var mapped = _mapper.Map<Workstation>(workstation);
        _workstationRepository.Add(mapped);
        return _mapper.Map<WorkstationDTO>(mapped);
    }

    public IEnumerable<WorkstationDTO> GetAll()
    {
        var getWorkstationFilter = new WorkstationFilterDTO();
        var filter = _mapper.Map<WorkstationFilter>(getWorkstationFilter);
        var filteredWorkstations = _workstationRepository.Get(filter);
        return _mapper.Map<IEnumerable<WorkstationDTO>>(filteredWorkstations);
    }

    public IEnumerable<WorkstationDTO> Get(WorkstationFilterDTO getWorkstationFilter)
    {
        var filter = _mapper.Map<WorkstationFilter>(getWorkstationFilter);
        var filteredWorkstations = _workstationRepository.Get(filter);
        return _mapper.Map<IEnumerable<WorkstationDTO>>(filteredWorkstations);
    }

    public WorkstationDTO Update(WorkstationDTO workstation)
    {
        var mapped = _mapper.Map<Workstation>(workstation);
        _workstationRepository.Update(mapped);
        return workstation;
    }

    public void Delete(string name)
    {
        var getWorkstationFilter = new WorkstationFilterDTO();
        var filter = _mapper.Map<WorkstationFilter>(getWorkstationFilter);
        var workstationToRemove = _workstationRepository.Get(filter).Where(x => x.Name == name).First();
        _workstationRepository.Delete(workstationToRemove);
    }
}
