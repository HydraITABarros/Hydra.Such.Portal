using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class ChatUsersOn
    {
        public int IdChatOn { get; set; }
        public int IdUser { get; set; }

        public virtual Utilizador IdUserNavigation { get; set; }
    }
}
