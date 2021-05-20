﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    public class Project : IEntity
    {
        private static readonly string MaxNameLengthMessage = $"{nameof(Name)} length can be not more than {MaxNameLength}";
        private static readonly string EmptyNameMessage = $"{nameof(Name)} is empty";

        public const int MaxNameLength = 30;

        private string _name;

        public Project()
        {
            var initialSnapshot = new ProjectSnapshot();
            
            Snapshots = new List<ProjectSnapshot>() 
            {
                initialSnapshot
            };
        }
        public Project(string projectName, string description) : this()
        { 
            Name = projectName;

            var initialSnapshot = Snapshots.First();

            initialSnapshot.Description = description;
        }
        public int Id { set; get; }
        public string Name
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException(EmptyNameMessage);
                if (value.Length > MaxNameLength)
                    throw new ArgumentException(MaxNameLengthMessage);
                
                _name = value;
                Snapshots.First().Name = value;
            }
            get
            {
                return _name;
            }
        }
        public bool isActive { set; get; } = true;
        public User Creator { set; get; }
        public int? CreatorId { set; get; }
        public IEnumerable<UserProjectRole> AuthorRoles { set; get; }
        public IEnumerable<ProjectSnapshot> Snapshots { get; }

    }
}
