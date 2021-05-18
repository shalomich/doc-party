using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Profile
{
    public class UserStatistics
    {
        public int AllProjectCount { set; get; }
        public int CreatedProjectCount { set; get; }
        public int ClosedAllProjectCount { set; get; }
        public int ClosedCreatedProjectCount { set; get; }
        public int AllUserProjectSnapshotsCount { set; get; }
        public int CreatedSnapshotsCount { set; get; }
        public int AllUserProjectCommentCount { set; get; }
        public int WrittenCommentCount { set; get; }
    }
}
