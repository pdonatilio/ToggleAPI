using ToggleAPI.Models;
using ToggleAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace ToggleAPI.Services
{
     public interface IToggleService
     {
        IEnumerable<ToggleDto> Get();
        ToggleDto Get(int id);
        void Post(SavingToggleDto toggleDto);
        void Put(int id, ToggleDto toggleDto);
        void Delete(int id);
     }

    public class ToggleService : IToggleService
    {
         private readonly IMapper _mapper;

        private ToggleAPIContext _context;

        public ToggleService(ToggleAPIContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all and get by ID
        public IEnumerable<ToggleDto> Get() => _mapper.Map<IList<ToggleDto>>(_context.Toggles);
        public ToggleDto Get(int id) => _mapper.Map<ToggleDto>(_context.Toggles.Find(id));

         //Creating a New Toggle
        public void Post(SavingToggleDto toggleDto)
        {
            //Only Create a new toggle if Name and ServicesList is not null
            if (!string.IsNullOrWhiteSpace(toggleDto.Name) && toggleDto.ServicesList != null )
            {
                // including the DTO itens to toggle model
                //var toggle = _mapper.Map<Toggle>(toggleDto);
                Toggle toggle = new Toggle();
                toggle.Name = toggleDto.Name;
                toggle.State = toggleDto.State;

                _context.Toggles.Add(toggle);
                _context.SaveChanges();

                // including the Services having relatioship with this toggle
                if (toggleDto.ServicesList.First().ToString().Equals("All"))
                {
                    foreach (var serviceItem in _context.Services.ToList())
                    {
                        TogglesServices service = new TogglesServices();
                        service.ToggleId = toggle.Id;
                        service.ServiceId = serviceItem.Id;
                        _context.TogglesServices.Add(service);
                        _context.SaveChanges();
                    }

                } else {                    
                    foreach (var serviceItem in toggleDto.ServicesList)
                    {
                        TogglesServices service = new TogglesServices();
                        service.ToggleId = toggle.Id;
                        service.ServiceId = Convert.ToInt32(serviceItem);
                        _context.TogglesServices.Add(service);
                        _context.SaveChanges();
                    }
                }
            }                    
            else
                throw new ArgumentException("The fields name and ServicesList can't be null.");
        }
        public void Put(int id, ToggleDto toggleDto)
        {
            Toggle toggle = _context.Toggles.Find(id);
            if (toggle != null)
            {
                if (!string.IsNullOrWhiteSpace(toggleDto.Name))
                {
                    toggle.Name = toggleDto.Name;
                    toggle.State = toggleDto.State;
                    _context.Toggles.Update(toggle);
                    _context.SaveChanges();
                }
                else
                    throw new ArgumentException("The name can't be null");
            }
            else
                throw new ArgumentException("Toggle Not Found");
        }

        public void Delete(int id)
        {
            var toggle = _context.Toggles.Find(id);
            if (toggle != null)
            {
                _context.Toggles.Remove(toggle);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Service Not Found");
            }
        }       
    }
}