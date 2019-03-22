using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class ChatUsersOn
    {
        public int IdChatOn { get; set; }
        public int IdUser { get; set; }

        public Utilizador IdUserNavigation { get; set; }
    }
}
