using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Shared;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.AuditModels;
using Domain.Common.Interfaces;
using System;
using Infrastructure.DbContexts;
using Newtonsoft.Json;

namespace Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryAsync<Audit, ApplicationDbContext> _repository;
        private readonly IDateTimeService _dateTimeService;

        public LogRepository(IRepositoryAsync<Audit, ApplicationDbContext> repository, IMapper mapper, IDateTimeService dateTimeService)
        {
            _repository = repository;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public async Task AddLogAsync(ILog log)
        {
            if (log != null)
            {
                Audit audit = new Audit()
                {
                    DateTime = DateTime.Now,
                    Type = log.Action,
                    TableName = log.TableName,
                    UserId = log.UserId,
                    OldValues = ConvertValues(log.OldValues),
                    NewValues = ConvertValues(log.NewValues),
                    PrimaryKey = log.Key
                };

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

        private string ConvertValues(object values) 
        {
            if (values != null)
            {
                if (values is List<string>)
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();

                    foreach (var item in values as IList<string>)
                        dictionary.Add(item, " ");

                   return JsonConvert.SerializeObject(dictionary);
                }
                else
                    return JsonConvert.SerializeObject(values);
            }
            return null;
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
