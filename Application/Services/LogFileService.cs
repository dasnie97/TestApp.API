﻿using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class LogFileService : ILogFileService
    {
        private readonly ILogFileRepository _logFileRepository;
        private readonly IMapper _mapper;
        public LogFileService(ILogFileRepository logFileRepository, IMapper mapper)
        {
            _logFileRepository = logFileRepository;
            _mapper = mapper;
        }

        public LogFileDTO AddNewLogFile(CreateLogFileDTO logFile)
        {
            if (string.IsNullOrEmpty(logFile.SerialNumber)) throw new Exception("Log file has to have serial number!");
            var mappedLogFile = _mapper.Map<LogFile>(logFile);
            _logFileRepository.Add(mappedLogFile);
            return _mapper.Map<LogFileDTO>(mappedLogFile);
        }

        public void DeleteLogFile(int id)
        {
            var logFile = _logFileRepository.Get(id);
            _logFileRepository.Delete(logFile);
        }

        public IEnumerable<LogFileDTO> GetAllLogFiles()
        {
            var logFiles = _logFileRepository.GetAll();
            return _mapper.Map<IEnumerable<LogFileDTO>>(logFiles);
        }

        public LogFileDTO GetLogFileById(int id)
        {
            var logFile = _logFileRepository.Get(id);
            return _mapper.Map<LogFileDTO>(logFile);
        }

        public void UpdateLogFile(UpdateLogFileDTO updateLogFile)
        {
            var originalLogFile = _logFileRepository.Get(updateLogFile.Id);
            var logFile = _mapper.Map(updateLogFile, originalLogFile);
            _logFileRepository.Update(logFile);
        }
    }
}
