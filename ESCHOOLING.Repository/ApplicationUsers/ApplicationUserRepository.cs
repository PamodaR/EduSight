using AutoMapper;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Repository.ApplicationUsers
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly ECOM_WebContext _dbContext = new ECOM_WebContext();
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserRepository"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public ApplicationUserRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        public ApplicationUser AddUser(ApplicationUser userObject)
        {
            try
            {
                var MappedObject = _mapper.Map<TblUserRegistration>(userObject);

                _dbContext.TblUserRegistrations.Add(MappedObject);
                var result = _dbContext.SaveChanges();

                userObject.UserId = MappedObject.UserId;
                if (result > 0) return userObject;

                return new ApplicationUser();
            }
            catch (Exception eX)
            {
                return new ApplicationUser();
            }
        }

        /// <summary>
        /// Authentications the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        public ApplicationUser AuthUser(ApplicationUser userObject)
        {
            try
            {
                var result = new TblUserRegistration();
                if (userObject == null || userObject.Password == null)
                {
                    return new ApplicationUser();
                }

                result = _dbContext.TblUserRegistrations.Select(data => data).
                Where(data => data.Email.ToLower().Equals(userObject.Email.ToLower()) &&
                data.Password.Equals(userObject.Password)).FirstOrDefault();

                var mappedobject = _mapper.Map<ApplicationUser>(result);
                if (mappedobject != null) return mappedobject;
                return new ApplicationUser();
            }
            catch (Exception)
            {
                return new ApplicationUser();
            }

        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public List<ApplicationUser> GetAllUsers()
        {
            try
            {
                var users = _dbContext.TblUserRegistrations.ToList();
                var mappedUsers = _mapper.Map<List<ApplicationUser>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                return new List<ApplicationUser>();
            }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public List<ApplicationUser> Search(int grade)
        {
            try
            {
                var users = _dbContext.TblUserRegistrations.Select(data => data).Where(data => data.Grade.Equals(grade)).ToList();
                var mappedUsers = _mapper.Map<List<ApplicationUser>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                return new List<ApplicationUser>();
            }
        }

        public List<ApplicationUser> SearchById(long id)
        {
            try
            {
                var users = _dbContext.TblUserRegistrations.Select(data => data).Where(data => data.UserId.Equals(id)).ToList();
                var mappedUsers = _mapper.Map<List<ApplicationUser>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                return new List<ApplicationUser>();
            }
        }

        public List<Attendance> GetAttendanceList(long id, string searchDate)
        {
            try
            {
                var attendance = _dbContext.TblAttendances.Select(data => data).Where(data => data.StudentId.Equals(id) && data.MonthForSearch.Equals(searchDate)).OrderBy(data => data.Date).ToList();
                var mappedUsers = _mapper.Map<List<Attendance>>(attendance);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                return new List<Attendance>();
            }
        }

        public List<Attendance> SearchForMonth(string searchDate)
        {
            try
            {
                var users = _dbContext.TblAttendances.Select(data => data).Where(data => data.MonthForSearch.Equals(searchDate)).ToList();
                var mappedUsers = _mapper.Map<List<Attendance>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                return null;
            }
        }

        public Attendance UpdateAttendance(Attendance attendanceObj)
        {
            try
            {
                attendanceObj.IsActive = true;
                attendanceObj.CreatedDate = DateTime.Now;
                attendanceObj.Date = DateTime.Today;
                attendanceObj.MonthForSearch = DateTime.Today.ToString(("yyyy-MM"));

                var attendanceForUser = _dbContext.TblUserRegistrations.Select(data => data).Where(data => data.UserId.Equals(attendanceObj.StudentId)).FirstOrDefault();
                if (attendanceForUser != null)
                {
                    attendanceForUser.IsPresent = attendanceObj.IsPresent;
                    _dbContext.SaveChanges();
                }

                var result = _dbContext.TblAttendances.Select(data => data).Where(data => data.StudentId.Equals(attendanceObj.StudentId) && data.Date.Equals(attendanceObj.Date)).FirstOrDefault();
                if(result != null)
                {
                    result.StudentId = attendanceObj.StudentId;
                    result.IsPresent = attendanceObj.IsPresent;
                    result.CreatedDate = attendanceObj.CreatedDate;
                    result.IsActive = attendanceObj.IsActive;
                    result.Date = attendanceObj.Date;
                    result.MonthForSearch = DateTime.Today.ToString(("yyyy-MM"));
                    _dbContext.SaveChanges();

                    var mappedobject = _mapper.Map<Attendance>(result);
                    if (mappedobject != null) return mappedobject;
                }
                else
                {
                    var MappedObject = _mapper.Map<TblAttendance>(attendanceObj);

                    _dbContext.TblAttendances.Add(MappedObject);
                    var attendance = _dbContext.SaveChanges();

                    attendanceObj.Id = MappedObject.Id;
                    if (attendance > 0) return attendanceObj;

                    return null;
                }

                return null;
            }
            catch (Exception eX)
            {
                return null;
            }
        }

        private static Random random = new Random();

        public static long GenerateId()
        {
            byte[] buffer = new byte[8];
            random.NextBytes(buffer);
            long id = BitConverter.ToInt64(buffer, 0);
            return Math.Abs(id);
        }

    }
}
