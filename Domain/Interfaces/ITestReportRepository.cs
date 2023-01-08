﻿using Domain.Models;

namespace Domain.Interfaces;

public interface ITestReportRepository
{
    TestReport Add(TestReport logFile);
    void Delete(TestReport logFile);
    TestReport Get(int id);
    IEnumerable<TestReport> GetAll(GetLogFilesQuery getLogFilesQuery);
    void Update(TestReport logFile);
    IEnumerable<Workstation> GetAllWorkstations();
    Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints();
}