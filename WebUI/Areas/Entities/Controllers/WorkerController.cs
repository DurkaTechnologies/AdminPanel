using AdminPanel.Application.Interfaces.Repositories;
using AdminPanel.Domain.Entities;
using AdminPanel.Infrastructure.Repositories;
using AdminPanel.Web.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Areas.Entities.Models;

namespace WebUI.Areas.Entities.Controllers
{
	[Area("Entities")]
	public class WorkerController : BaseController<WorkerController>
	{
        IWorkerRepository _workerRepository;
        IMapper _mapper;

        public WorkerController(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;

            IConfigurationProvider configuration = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Worker, WorkerViewModel>();
                    cfg.CreateMap<WorkerViewModel, Worker>();
                });

            _mapper = new Mapper(configuration);
        }

        public IActionResult Index() 
        {
            return View();
        }

        public async Task<IActionResult> LoadAll()
        {
            var response = await _workerRepository.GetListAsync();
            
            if (response != null)
            {
                var viewModel = _mapper.Map<List<WorkerViewModel>>(response);
                return PartialView("_ViewAll", viewModel);
            }

            return null;
        }
    }
}
