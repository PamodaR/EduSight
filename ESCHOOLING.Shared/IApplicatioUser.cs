using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Shared
{
    public interface IApplicatioUser
    {
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        Task<ApplicationUser> RegisterUserAsync(ApplicationUser userObject);

        /// <summary>
        /// Logins the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        Task<ApplicationUser> LoginUserAsync(ApplicationUser userObject);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        Task<List<ApplicationUser>> GetAllUsersAsync();

        /// <summary>
        /// Gets user counts grouped by UserType, aggregated in SQL.
        /// </summary>
        Task<Dictionary<int, int>> GetUserCountsByTypeAsync();

        /// <summary>
        /// Gets users whose UserType is not one of the excluded types, filtered in SQL.
        /// </summary>
        Task<List<ApplicationUser>> GetUsersExcludingTypesAsync(params int[] excludedUserTypes);

        /// <summary>
        /// Gets new-registration counts grouped by month ("yyyy-MM"), for the last N months.
        /// </summary>
        Task<Dictionary<string, int>> GetRegistrationCountsByMonthAsync(int months);

        /// <summary>
        /// Gets the school-wide attendance rate (% present) grouped by month ("yyyy-MM"), for the last N months.
        /// </summary>
        Task<Dictionary<string, double>> GetAttendanceRateByMonthAsync(int months);

        /// <summary>
        /// Gets today's school-wide attendance status: how many students have been marked, and how many of those are present.
        /// </summary>
        Task<(int Present, int TotalMarked)> GetTodayAttendanceStatusAsync();

        /// <summary>
        /// Gets a single student's attendance rate (% present) grouped by month ("yyyy-MM"), for the last N months.
        /// </summary>
        Task<Dictionary<string, double>> GetAttendanceRateByMonthForStudentAsync(long studentId, int months);

        Task<List<ApplicationUser>> SearchAsync(int grade);
        Task<List<ApplicationUser>> SearchByIdAsync(long id);
        Task<List<Attendance>> GetAttendanceListAsync(long id, string searchDate);
        Task<List<Attendance>> SearchForMonthAsync(string searchDate);

        Task<Attendance> UpdateAttendanceAsync(Attendance attendanceObj);

        /// <summary>
        /// Gets a single user by id.
        /// </summary>
        Task<ApplicationUser> GetUserByIdAsync(long id);

        /// <summary>
        /// Gets users of a specific UserType, filtered in SQL.
        /// </summary>
        Task<List<ApplicationUser>> GetUsersByTypeAsync(int userType);

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser userObject);

        /// <summary>
        /// Soft-deletes a user by setting IsActive to false.
        /// </summary>
        Task<bool> DeactivateUserAsync(long userId);
    }
}
