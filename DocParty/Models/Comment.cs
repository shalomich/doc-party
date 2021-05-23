using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    public class Comment : IEntity
    {
        private const int MaxTextLength = 100;
        private readonly string LengthyTextMessage = $"Length of the comment text must not be more than {MaxTextLength} symbols";
        private readonly string EmptyTextMessage = $"Comment text must not be empty";

        private string _text;
        public int Id { set; get; }

        public Comment()
        {

        }

        public Comment(ProjectSnapshot projectSnapshot, User author)
        {
            ProjectSnapshot = projectSnapshot ?? throw new ArgumentNullException(nameof(projectSnapshot));
            Author = author ?? throw new ArgumentNullException(nameof(author));
        }

        public string Text 
        {
            set 
            {
                if (value.Length > MaxTextLength)
                    throw new ArgumentException(LengthyTextMessage);
                if (String.IsNullOrEmpty(value.Trim(' ')))
                    throw new ArgumentException(EmptyTextMessage);
                _text = value;
            }
            get 
            {
                return _text;
            } 
        }

        public ProjectSnapshot ProjectSnapshot { set; get; }
        public int ProjectSnapshotId { set; get; }

        public User Author { set; get; }
    }
}
