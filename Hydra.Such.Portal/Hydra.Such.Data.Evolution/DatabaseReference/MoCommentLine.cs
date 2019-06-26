using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class MoCommentLine
    {
        public byte[] Timestamp { get; set; }
        public int Table { get; set; }
        public int Type { get; set; }
        public string HeaderNo { get; set; }
        public int LineNo { get; set; }
        public int CommentNo { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int TableSubType { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
        public int OrcAlternativo { get; set; }
    }
}
