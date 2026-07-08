using AutoMapper;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESCHOOLING.Repository.AutomapperProfiles
{
    public class AttendenceProfile : Profile
    {
        public AttendenceProfile()
        {
            CreateMap<TblAttendance, Attendance>();
            CreateMap<Attendance, TblAttendance>();
        }
    }
}
