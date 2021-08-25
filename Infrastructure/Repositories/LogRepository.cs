﻿using AdminPanel.Application.DTOs;
using AdminPanel.Application.Interfaces.Repositories;
using AdminPanel.Application.Interfaces.Shared;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Infrastructure.AuditModels;
using Domain.Common.Interfaces;
using System;

namespace AdminPanel.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryAsync<Audit> _repository;
        private readonly IDateTimeService _dateTimeService;

        public LogRepository(IRepositoryAsync<Audit> repository, IMapper mapper, IDateTimeService dateTimeService)
        {
            _repository = repository;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public async Task AddLogAsync(IAudit audit)
        {
            if (audit != null)
            {
                audit.DateTime = DateTime.Now;
                await _repository.AddAsync(_mapper.Map<Audit>(audit));
            }
        }

        public async Task AddLogAsync(string action, string userId)
        {
            var audit = new Audit()
            {
                Type = action,
                UserId = userId,
                DateTime = _dateTimeService.NowUtc
            };
            await _repository.AddAsync(audit);
        }

        public async Task<List<AuditLogResponse>> GetAuditLogsAsync(string userId)
        {
            var logs = await _repository.Entities.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
            var mappedLogs = _mapper.Map<List<AuditLogResponse>>(logs);
            return mappedLogs;
        }
    }

    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<AuditLogResponse, Audit>().ReverseMap();
        }
    }
}
