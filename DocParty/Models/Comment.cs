using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    public class Comment : IEntity
    {
        public int Id { set; get; }

        public string Text { set; get; }

        public ProjectSnapshot ProjectSnapshot { set; get; }
        public int ProjectSnapshotId { set; get; }

        public User Author { set; get; }
    }
}
