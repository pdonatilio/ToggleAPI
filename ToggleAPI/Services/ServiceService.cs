using ToggleAPI.Models;
using ToggleAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ToggleAPI.Services
{
     public interface IServiceService
     {
        IEnumerable<ServiceDto> Get();
        ServiceDto Get(int id);
        void Post(ServiceDto serviceDto);
        void Put(int id, ServiceDto serviceDto);
        void Delete(int id);
        //get the service and the tokens linked
        ServiceTogglesDto GetAllToggles(int id);
     }

    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;

        private ToggleAPIContext _context;

        public ServiceService(ToggleAPIContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all and get by ID
        public IEnumerable<ServiceDto> Get() => _mapper.Map<IList<ServiceDto>>(_context.Services);
        public ServiceDto Get(int id) => _mapper.Map<ServiceDto>(_context.Services.Find(id));

         //Creating a New Service
        public void Post(ServiceDto serviceDto)
        {
            //Only Create a new service if version and name is not null
            if (serviceDto.version > 0 && 
                !string.IsNullOrWhiteSpace(serviceDto.name))
            {
                // including the DTO itens to service model
                Service service = _mapper.Map<Service>(serviceDto);

                _context.Services.Add(service);
                _context.SaveChanges();
            }                    
            else
                throw new ArgumentException("The fields version and name can't be null.");
        }
        public void Put(int id, ServiceDto serviceDto)
        {
            Service service = _context.Services.Find(id);
            if (service != null)
            {
                if (serviceDto.version > 0 && !string.IsNullOrWhiteSpace(serviceDto.name))
                {
                    service.version = serviceDto.version;
                    service.name = serviceDto.name;                    
                }
                else
                    throw new ArgumentException("The name and version can't be null");

                _context.Services.Update(service);
                _context.SaveChanges();
            }
            else
                throw new ArgumentException("Service Not Found");
        }

        public void Delete(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Service Not Found");
            }
        }

        public ServiceTogglesDto GetAllToggles(int id)
        {
            ServiceTogglesDto serviceToggles = new ServiceTogglesDto();
            var service = _context.Services.Find(id);

            serviceToggles.Id = service.Id;
            serviceToggles.name = service.name;
            serviceToggles.version = service.version;

            var togglesList = _context.TogglesServices
                                .Where(s => s.ServiceId == service.Id)
                                .Select( t => new {
                                    Id = t.Toggle.Id,
                                    Name = t.Toggle.Name,
                                    State = t.Toggle.State
                                    })
                                .ToList();

            serviceToggles.togglesList = _mapper.Map<IList<ToggleDto>>(togglesList);

            serviceToggles.mostImportantToggle = _mapper.Map<ToggleDto>(togglesList.Last());

            return serviceToggles;
        }

        
    }
}