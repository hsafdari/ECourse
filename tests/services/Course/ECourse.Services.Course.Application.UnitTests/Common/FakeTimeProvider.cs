using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECourse.Services.Course.Application.UnitTests.Common
{
    internal sealed class FakeTimeProvider:TimeProvider
    {
        private DateTimeOffset _utcNow;
        public FakeTimeProvider(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }
        public override DateTimeOffset GetUtcNow()
        {
            return _utcNow;
        }
        public void SetUtcNow(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }
    }
}
