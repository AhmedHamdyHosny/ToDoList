using System;

namespace ToDoList.DataLayer
{
    public class CommonEntity
    {
        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public int? ModifyUserId { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
