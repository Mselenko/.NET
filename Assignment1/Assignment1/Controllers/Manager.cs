using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Assignment1.EntityModels;
using Assignment1.Models;

namespace Assignment1.Controllers
{
    public class Manager
    {

        private DataContext ds = new DataContext();

        public IMapper mapper;

        public Manager()
        {
 
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeBaseViewModel>();
                cfg.CreateMap<EmployeeAddViewModel, Employee>();
            });

            mapper = config.CreateMapper();


            ds.Configuration.ProxyCreationEnabled = false;

            ds.Configuration.LazyLoadingEnabled = false;
        }

        public IEnumerable<EmployeeBaseViewModel> EmployeeGetAll()
        {
            var sortedEmployee = ds.Employees.OrderBy(s => s.LastName).ThenBy(s => s.FirstName);

            return mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeBaseViewModel>>(sortedEmployee);
        }


        public EmployeeBaseViewModel EmployeeGetById(int id)
        {
            var obj = ds.Employees.Find(id);

            return obj == null ? null : mapper.Map<Employee, EmployeeBaseViewModel>(obj);
        }

       
        public EmployeeBaseViewModel EmployeeAdd(EmployeeAddViewModel newEmployee)
        {
            var addetItem = ds.Employees.Add(mapper.Map<EmployeeAddViewModel, Employee>(newEmployee));
            ds.SaveChanges();

            return addetItem == null ? null : mapper.Map<Employee, EmployeeBaseViewModel>(addetItem);
        }
    }
}