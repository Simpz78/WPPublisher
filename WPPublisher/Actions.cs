using System;
using WPPublisher.Controller;
using WPPublisher.Model;

namespace WPPublisher
{
    public class Actions
    {
        public int PostAndPublishMessage(WPPost post)
        {
            // Chiamata al controller per l'accodamento al rabbit
            WPPostController.EnqueuePost();
            // Chiamata al controller per la pubblicazione effettiva del messaggio

            return 0;
        }

        public int PostMessage(WPPost post)
        {
            return 0;
        }

        public int PublishMessage(int idMessage)
        {

            return 0;
        }
    }
}
