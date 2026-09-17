using ECourse.Services.Course.Application.Features.CourseGroups.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECourse.Services.Course.Application.Features.CourseGroups.GetList
{
    public interface IGetCouseGroupsByParentIdReadService
    {
        Task<IReadOnlyList<CourseGroupTreeResponse>?> GetCouseGroupsByParentIdAsync(Guid? parentId,CancellationToken cancellationToken = default);
    }
}
