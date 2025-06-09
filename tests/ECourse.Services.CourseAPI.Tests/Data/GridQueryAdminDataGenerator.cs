using Bogus;
using ECourse.Services.CourseAPI.Tests.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECourse.Services.CourseAPI.Tests.Data
{
    class GridQueryAdminDataGenerator
    {
        Faker<GridQueryAdmin> _gridQueryAdminFaker;
        public GridQueryAdminDataGenerator()
        {
            _gridQueryAdminFaker = new Faker<GridQueryAdmin>()
                .RuleFor(x => x.Filter, "")
                .RuleFor(x => x.FilterColumn, "")
                .RuleFor(x => x.FilterValue, "")
                .RuleFor(x => x.Page, 1)
                .RuleFor(x => x.Skip, 0)
                .RuleFor(x => x.SortColumn, "")
                .RuleFor(x => x.SortOrder, "asc")
                .RuleFor(x => x.Top, 20);

                ;
        }
        public GridQueryAdmin GenerateFakeQuery()
        {
            return _gridQueryAdminFaker.Generate();
        }
    }
}
