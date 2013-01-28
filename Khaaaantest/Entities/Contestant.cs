using System;

namespace Khaaaantest.Entities
{
    public class Contestant
    {
        public int Id { get; set; }
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public string Email{ get; set; }
        public int ContestId{ get; set; }
        public int WinnerNumber{ get; set; }
        public DateTime CreatedOn{ get; set; }
    }
}

