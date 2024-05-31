﻿using ECourse.Services.CourseAPI.Models;
using Infrastructure.Repository;

namespace ECourse.Services.CourseAPI.Interfaces
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
    }
}
