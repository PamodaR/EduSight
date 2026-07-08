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
        ApplicationUser RegisterUser(ApplicationUser userObject);

        /// <summary>
        /// Logins the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        ApplicationUser LoginUser(ApplicationUser userObject);

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
