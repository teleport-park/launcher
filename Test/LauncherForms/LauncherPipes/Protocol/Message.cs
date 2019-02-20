using System;
using System.Collections.Generic;
using System.Text;

namespace LauncherPipes.Protocol {
    [Serializable]
    public class Message {
        public string RecipientId { get; set; }
        public string MessageId { get; set; }
        public string Text { get; set; }
    }
}
