using ECOMSYSTEM.Repository.ApplicationUsers;
using ECOMSYSTEM.Shared;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.Shared.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace ECOMSYSTEM.Web.Services
{
    public class ApplicationUserService : IApplicatioUser
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserService(IApplicationUserRepository userRepository)
        {
            _applicationUserRepository = userRepository;
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        public ApplicationUser RegisterUser(ApplicationUser userObject)
        {
            try
            {
                var result = _applicationUserRepository.AddUser(userObject);
                return result;
            }
            catch (Exception eX)
            {
                return new ApplicationUser();
            }
        }

        /// <summary>
        /// Logins the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        public ApplicationUser LoginUser(ApplicationUser userObject)
        {
            try
            {
                var result = _applicationUserRepository.AuthUser(userObject);
                return result;
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
                var result = _applicationUserRepository.GetAllUsers();
                return result;
            }
            catch (Exception)
            {
                return new List<ApplicationUser>();
            }
        }

        public List<ApplicationUser> Search(int grade)
        {
            try
            {
                var result = _applicationUserRepository.Search(grade);
                return result;
            }
            catch (Exception)
            {
                return new List<ApplicationUser>();
            }
        }

        public List<ApplicationUser> SearchById(long id)
        {
            try
            {
                var result = _applicationUserRepository.SearchById(id);
                return result;
            }
            catch (Exception)
            {
                return new List<ApplicationUser>();
            }
        }

        public List<Attendance> GetAttendanceList(long id, string searchDate)
        {
            try
            {
                var result = _applicationUserRepository.GetAttendanceList(id, searchDate);
                return result;
            }
            catch (Exception)
            {
                return new List<Attendance>();
            }
        }

        public List<Attendance> SearchForMonth(string searchDate)
        {
            try
            {
                var result = _applicationUserRepository.SearchForMonth(searchDate);
                return result;
            }
            catch (Exception)
            {
                return new List<Attendance>();
            }
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        public List<ApplicationUser> GetAllProducts()
        {
            try
            {
                var result = _applicationUserRepository.GetAllUsers();
                return result;
            }
            catch (Exception)
            {
                return new List<ApplicationUser>();
            }
        }

        public Attendance UpdateAttendance(Attendance attendanceObj)
        {
            try
            {
                var result = _applicationUserRepository.UpdateAttendance(attendanceObj);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
