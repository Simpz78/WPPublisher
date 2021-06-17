using System;
using WPPublisher.Controller;
using WPPublisher.Model;

namespace WPPublisher
{
    public class Actions
    {
        public int PostAndPublishMessage(WPPost post)
        {
            int response = 0;
            // Chiamata al controller per l'accodamento al rabbit
            response = PostMessage(post);
            // Chiamata al controller per la pubblicazione effettiva del messaggio

            return response;
        }

        public int PostMessage(WPPost post)
        {
            int response = WPPostController.EnqueuePost(post);

            return response;
        }

        public int PublishMessage(int idMessage)
        {

            return 0;
        }
    }
}
