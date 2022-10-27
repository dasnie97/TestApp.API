﻿using Domain.Models;

namespace Domain.Interfaces
{
    public interface ILogFileRepository
    {
        LogFile Add(LogFile logFile);
        void Delete(LogFile logFile);
        LogFile Get(int id);
        IEnumerable<LogFile> GetAll();
        void Update(LogFile logFile);
    }
}