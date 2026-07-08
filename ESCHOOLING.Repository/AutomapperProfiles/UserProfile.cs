using AutoMapper;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.DataAccess.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Repository.AutomapperProfiles
{
    public class UserProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> class.
        /// </summary>
        public UserProfile()
        {
            CreateMap<TblUserRegistration, ApplicationUser>();
            CreateMap<ApplicationUser, TblUserRegistration>();
        }
    }
}
