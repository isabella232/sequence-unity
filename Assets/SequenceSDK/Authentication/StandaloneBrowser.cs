using UnityEngine.Device;

namespace Sequence.Authentication
{
    public class StandaloneBrowser : IBrowser
    {
        public void Authenticate(string url, string redirectUrl = "")
        {
            Application.OpenURL(url);
        }
    }
}