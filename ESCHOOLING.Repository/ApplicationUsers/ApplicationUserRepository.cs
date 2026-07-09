using AutoMapper;
using ECOMSYSTEM.Shared.Models;
using ESCHOOLING.DataAccess.EntityModel;
using ESCHOOLING.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ECOM_WebContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationUserRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public ApplicationUserRepository(ECOM_WebContext dbContext, IMapper mapper, ILogger<ApplicationUserRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        public async Task<ApplicationUser> AddUserAsync(ApplicationUser userObject)
        {
            try
            {
                var MappedObject = _mapper.Map<TblUserRegistration>(userObject);

                _dbContext.TblUserRegistrations.Add(MappedObject);
                var result = await _dbContext.SaveChangesAsync();

                userObject.UserId = MappedObject.UserId;
                if (result > 0) return userObject;

                return new ApplicationUser();
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "AddUserAsync failed for Email={Email}", userObject?.Email);
                return new ApplicationUser();
            }
        }

        /// <summary>
        /// Authentications the user.
        /// </summary>
        /// <param name="userObject">The user object.</param>
        /// <returns></returns>
        public async Task<ApplicationUser> AuthUserAsync(ApplicationUser userObject)
        {
            try
            {
                if (userObject == null || userObject.Password == null)
                {
                    return new ApplicationUser();
                }

                var result = await _dbContext.TblUserRegistrations.AsNoTracking().
                Where(data => data.Email.Equals(userObject.Email) &&
                data.Password.Equals(userObject.Password)).FirstOrDefaultAsync();

                var mappedobject = _mapper.Map<ApplicationUser>(result);
                if (mappedobject != null) return mappedobject;
                return new ApplicationUser();
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "AuthUserAsync failed for Email={Email}", userObject?.Email);
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
                var users = await _dbContext.TblUserRegistrations.AsNoTracking().ToListAsync();
                var mappedUsers = _mapper.Map<List<ApplicationUser>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "GetAllUsersAsync failed");
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
                var counts = await _dbContext.TblUserRegistrations.AsNoTracking()
                    .GroupBy(u => u.UserType)
                    .Select(g => new { UserType = g.Key ?? -1, Count = g.Count() })
                    .ToListAsync();

                return counts.ToDictionary(c => c.UserType, c => c.Count);
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "GetUserCountsByTypeAsync failed");
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
                var users = await _dbContext.TblUserRegistrations.AsNoTracking()
                    .Where(u => !excludedUserTypes.Contains(u.UserType ?? -1))
                    .ToListAsync();
                var mappedUsers = _mapper.Map<List<ApplicationUser>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "GetUsersExcludingTypesAsync failed for ExcludedUserTypes={ExcludedUserTypes}", excludedUserTypes);
                return new List<ApplicationUser>();
            }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationUser>> SearchAsync(int grade)
        {
            try
            {
                var users = await _dbContext.TblUserRegistrations.AsNoTracking().Where(data => data.Grade.Equals(grade)).ToListAsync();
                var mappedUsers = _mapper.Map<List<ApplicationUser>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "SearchAsync failed for Grade={Grade}", grade);
                return new List<ApplicationUser>();
            }
        }

        public async Task<List<ApplicationUser>> SearchByIdAsync(long id)
        {
            try
            {
                var users = await _dbContext.TblUserRegistrations.AsNoTracking().Where(data => data.UserId.Equals(id)).ToListAsync();
                var mappedUsers = _mapper.Map<List<ApplicationUser>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "SearchByIdAsync failed for UserId={UserId}", id);
                return new List<ApplicationUser>();
            }
        }

        public async Task<List<Attendance>> GetAttendanceListAsync(long id, string searchDate)
        {
            try
            {
                var attendance = await _dbContext.TblAttendances.AsNoTracking().Where(data => data.StudentId.Equals(id) && data.MonthForSearch.Equals(searchDate)).OrderBy(data => data.Date).ToListAsync();
                var mappedUsers = _mapper.Map<List<Attendance>>(attendance);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "GetAttendanceListAsync failed for StudentId={StudentId}, SearchDate={SearchDate}", id, searchDate);
                return new List<Attendance>();
            }
        }

        public async Task<List<Attendance>> SearchForMonthAsync(string searchDate)
        {
            try
            {
                var users = await _dbContext.TblAttendances.AsNoTracking().Where(data => data.MonthForSearch.Equals(searchDate)).ToListAsync();
                var mappedUsers = _mapper.Map<List<Attendance>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "SearchForMonthAsync failed for SearchDate={SearchDate}", searchDate);
                return null;
            }
        }

        public async Task<Attendance> UpdateAttendanceAsync(Attendance attendanceObj)
        {
            try
            {
                attendanceObj.IsActive = true;
                attendanceObj.CreatedDate = DateTime.Now;
                attendanceObj.Date = DateTime.Today;
                attendanceObj.MonthForSearch = DateTime.Today.ToString(("yyyy-MM"));

                var attendanceForUser = await _dbContext.TblUserRegistrations.Where(data => data.UserId.Equals(attendanceObj.StudentId)).FirstOrDefaultAsync();
                if (attendanceForUser != null)
                {
                    attendanceForUser.IsPresent = attendanceObj.IsPresent;
                    await _dbContext.SaveChangesAsync();
                }

                var result = await _dbContext.TblAttendances.Where(data => data.StudentId.Equals(attendanceObj.StudentId) && data.Date.Equals(attendanceObj.Date)).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.StudentId = attendanceObj.StudentId;
                    result.IsPresent = attendanceObj.IsPresent;
                    result.CreatedDate = attendanceObj.CreatedDate;
                    result.IsActive = attendanceObj.IsActive;
                    result.Date = attendanceObj.Date;
                    result.MonthForSearch = DateTime.Today.ToString(("yyyy-MM"));
                    await _dbContext.SaveChangesAsync();

                    var mappedobject = _mapper.Map<Attendance>(result);
                    if (mappedobject != null) return mappedobject;
                }
                else
                {
                    var MappedObject = _mapper.Map<TblAttendance>(attendanceObj);

                    _dbContext.TblAttendances.Add(MappedObject);
                    var attendance = await _dbContext.SaveChangesAsync();

                    attendanceObj.Id = MappedObject.Id;
                    if (attendance > 0) return attendanceObj;

                    return null;
                }

                return null;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "UpdateAttendanceAsync failed for StudentId={StudentId}", attendanceObj?.StudentId);
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
                var user = await _dbContext.TblUserRegistrations.AsNoTracking().Where(data => data.UserId.Equals(id)).FirstOrDefaultAsync();
                var mappedUser = _mapper.Map<ApplicationUser>(user);
                return mappedUser;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "GetUserByIdAsync failed for UserId={UserId}", id);
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
                var users = await _dbContext.TblUserRegistrations.AsNoTracking()
                    .Where(u => u.UserType == userType)
                    .ToListAsync();
                var mappedUsers = _mapper.Map<List<ApplicationUser>>(users);
                return mappedUsers;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "GetUsersByTypeAsync failed for UserType={UserType}", userType);
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
                var result = await _dbContext.TblUserRegistrations.Where(data => data.UserId.Equals(userObject.UserId)).FirstOrDefaultAsync();
                if (result == null)
                {
                    return new ApplicationUser();
                }

                result.Username = userObject.Username;
                result.Email = userObject.Email;
                result.Password = userObject.Password;
                result.Address = userObject.Address;
                result.MobileNo = userObject.MobileNo;
                result.IsActive = userObject.IsActive;
                result.Grade = userObject.Grade;

                await _dbContext.SaveChangesAsync();

                var mappedObject = _mapper.Map<ApplicationUser>(result);
                return mappedObject;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "UpdateUserAsync failed for UserId={UserId}", userObject?.UserId);
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
                var result = await _dbContext.TblUserRegistrations.Where(data => data.UserId.Equals(userId)).FirstOrDefaultAsync();
                if (result == null)
                {
                    return false;
                }

                result.IsActive = false;
                var saved = await _dbContext.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception eX)
            {
                _logger.LogError(eX, "DeactivateUserAsync failed for UserId={UserId}", userId);
                return false;
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
