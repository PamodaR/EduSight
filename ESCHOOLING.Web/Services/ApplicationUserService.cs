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
        public async Task<ApplicationUser> RegisterUserAsync(ApplicationUser userObject)
        {
            try
            {
                var result = await _applicationUserRepository.AddUserAsync(userObject);
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
        public async Task<ApplicationUser> LoginUserAsync(ApplicationUser userObject)
        {
            try
            {
                var result = await _applicationUserRepository.AuthUserAsync(userObject);
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
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                var result = await _applicationUserRepository.GetAllUsersAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<ApplicationUser>();
            }
        }

        /// <summary>
        /// Gets user counts grouped by UserType, aggregated in SQL.
        /// </summary>
        public async Task<Dictionary<int, int>> GetUserCountsByTypeAsync()
        {
            try
            {
                var result = await _applicationUserRepository.GetUserCountsByTypeAsync();
                return result;
            }
            catch (Exception)
            {
                return new Dictionary<int, int>();
            }
        }

        /// <summary>
        /// Gets users whose UserType is not one of the excluded types, filtered in SQL.
        /// </summary>
        public async Task<List<ApplicationUser>> GetUsersExcludingTypesAsync(params int[] excludedUserTypes)
        {
            try
            {
                var result = await _applicationUserRepository.GetUsersExcludingTypesAsync(excludedUserTypes);
                return result;
            }
            catch (Exception)
            {
                return new List<ApplicationUser>();
            }
        }

        /// <summary>
        /// Gets new-registration counts grouped by month ("yyyy-MM"), for the last N months.
        /// </summary>
        public async Task<Dictionary<string, int>> GetRegistrationCountsByMonthAsync(int months)
        {
            try
            {
                var result = await _applicationUserRepository.GetRegistrationCountsByMonthAsync(months);
                return result;
            }
            catch (Exception)
            {
                return new Dictionary<string, int>();
            }
        }

        /// <summary>
        /// Gets the school-wide attendance rate (% present) grouped by month ("yyyy-MM"), for the last N months.
        /// </summary>
        public async Task<Dictionary<string, double>> GetAttendanceRateByMonthAsync(int months)
        {
            try
            {
                var result = await _applicationUserRepository.GetAttendanceRateByMonthAsync(months);
                return result;
            }
            catch (Exception)
            {
                return new Dictionary<string, double>();
            }
        }

        /// <summary>
        /// Gets today's school-wide attendance status: how many students have been marked, and how many of those are present.
        /// </summary>
        public async Task<(int Present, int TotalMarked)> GetTodayAttendanceStatusAsync()
        {
            try
            {
                var result = await _applicationUserRepository.GetTodayAttendanceStatusAsync();
                return result;
            }
            catch (Exception)
            {
                return (0, 0);
            }
        }

        /// <summary>
        /// Gets a single student's attendance rate (% present) grouped by month ("yyyy-MM"), for the last N months.
        /// </summary>
        public async Task<Dictionary<string, double>> GetAttendanceRateByMonthForStudentAsync(long studentId, int months)
        {
            try
            {
                var result = await _applicationUserRepository.GetAttendanceRateByMonthForStudentAsync(studentId, months);
                return result;
            }
            catch (Exception)
            {
                return new Dictionary<string, double>();
            }
        }

        public async Task<List<ApplicationUser>> SearchAsync(int grade)
        {
            try
            {
                var result = await _applicationUserRepository.SearchAsync(grade);
                return result;
            }
            catch (Exception)
            {
                return new List<ApplicationUser>();
            }
        }

        public async Task<List<ApplicationUser>> SearchByIdAsync(long id)
        {
            try
            {
                var result = await _applicationUserRepository.SearchByIdAsync(id);
                return result;
            }
            catch (Exception)
            {
                return new List<ApplicationUser>();
            }
        }

        public async Task<List<Attendance>> GetAttendanceListAsync(long id, string searchDate)
        {
            try
            {
                var result = await _applicationUserRepository.GetAttendanceListAsync(id, searchDate);
                return result;
            }
            catch (Exception)
            {
                return new List<Attendance>();
            }
        }

        public async Task<List<Attendance>> SearchForMonthAsync(string searchDate)
        {
            try
            {
                var result = await _applicationUserRepository.SearchForMonthAsync(searchDate);
                return result;
            }
            catch (Exception)
            {
                return new List<Attendance>();
            }
        }

        public async Task<Attendance> UpdateAttendanceAsync(Attendance attendanceObj)
        {
            try
            {
                var result = await _applicationUserRepository.UpdateAttendanceAsync(attendanceObj);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a single user by id.
        /// </summary>
        public async Task<ApplicationUser> GetUserByIdAsync(long id)
        {
            try
            {
                var result = await _applicationUserRepository.GetUserByIdAsync(id);
                return result;
            }
            catch (Exception)
            {
                return new ApplicationUser();
            }
        }

        /// <summary>
        /// Gets users of a specific UserType, filtered in SQL.
        /// </summary>
        public async Task<List<ApplicationUser>> GetUsersByTypeAsync(int userType)
        {
            try
            {
                var result = await _applicationUserRepository.GetUsersByTypeAsync(userType);
                return result;
            }
            catch (Exception)
            {
                return new List<ApplicationUser>();
            }
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser userObject)
        {
            try
            {
                var result = await _applicationUserRepository.UpdateUserAsync(userObject);
                return result;
            }
            catch (Exception)
            {
                return new ApplicationUser();
            }
        }

        /// <summary>
        /// Soft-deletes a user by setting IsActive to false.
        /// </summary>
        public async Task<bool> DeactivateUserAsync(long userId)
        {
            try
            {
                var result = await _applicationUserRepository.DeactivateUserAsync(userId);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
