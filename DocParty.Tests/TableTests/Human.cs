using System;
using System.Collections.Generic;
using System.Text;

namespace DocParty.Tests.TableTests
{
    public class Human
    {
        public Human()
        {
            Age = 10;
        }
        public int Age { private set; get; }
        public Address Address { set; get; }
    }

    public class Address
    {
        public string Street => "Lenin";

        public override string ToString()
        {
            return Street;
        }
    }
}
