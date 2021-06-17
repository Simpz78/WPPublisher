using System;
using WPPublisher.Controller;
using WPPublisher.Model;

namespace WPPublisher
{
    public class Actions
    {
        private WPPostController _WPPostController = new WPPostController();

        public int PostMessage(WPPost post)
        {
            // Chiamata al controller per l'accodamento al rabbit
             
            // Chiamata al controller per la pubblicazione effettiva del messaggio

            return 0;
        }
    }
}
