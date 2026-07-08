using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMSYSTEM.Repository.ApplicationUsers
{
    public interface IApplicationUserRepository
    {
        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        ApplicationUser AddUser(ApplicationUser userObject);

        /// <summary>
        /// Authentications the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        ApplicationUser AuthUser(ApplicationUser userObject);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        List<ApplicationUser> GetAllUsers();
        List<ApplicationUser> Search(int grade);
        List<ApplicationUser> SearchById(long id);
        List<Attendance> GetAttendanceList(long id, string searchDate);
        List<Attendance> SearchForMonth(string searchDate);
        Attendance UpdateAttendance(Attendance attendanceObj);
    }
}
