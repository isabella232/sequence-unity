using System.Threading.Tasks;

namespace Sequence.Authentication
{
    public interface ILogin
    {
        public delegate void OnLoginSuccessHandler(string sessionId, string walletAddress);
        public event OnLoginSuccessHandler OnLoginSuccess;

        public delegate void OnLoginFailedHandler(string error);
        public event OnLoginFailedHandler OnLoginFailed;

        public delegate void OnMFAEmailSentHandler(string email);

        public event OnMFAEmailSentHandler OnMFAEmailSent;

        public delegate void OnMFAEmailFailedToSendHandler(string email, string error);

        public event OnMFAEmailFailedToSendHandler OnMFAEmailFailedToSend;

        /// <summary>
        /// Attempt to send the user an MFA email
        /// Emits an OnMFAEmailSent event when successful
        /// Emits an OnMFAEmailFailedToSend event when unsuccessful
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task Login(string email);

        /// <summary>
        /// Attempt to log the user in
        /// Emits an OnLoginSuccess event when successful
        /// Emits an OnLoginFailed event when unsuccessful
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task Login(string email, string code);

        /// <summary>
        /// Attempt to log the user in using Google login
        /// Emits an OnLoginSuccess event when successful
        /// Emits an OnLoginFailed event when unsuccessful
        /// Social login may also emit relevant events
        /// </summary>
        public void GoogleLogin();

        /// <summary>
        /// Attempt to log the user in using Discord login
        /// Emits an OnLoginSuccess event when successful
        /// Emits an OnLoginFailed event when unsuccessful
        /// Social login may also emit relevant events
        /// </summary>
        public void DiscordLogin();

        /// <summary>
        /// Attempt to log the user in using Facebook login
        /// Emits an OnLoginSuccess event when successful
        /// Emits an OnLoginFailed event when unsuccessful
        /// Social login may also emit relevant events
        /// </summary>
        public void FacebookLogin();

        /// <summary>
        /// Attempt to log the user in using Apple login
        /// Emits an OnLoginSuccess event when successful
        /// Emits an OnLoginFailed event when unsuccessful
        /// Social login may also emit relevant events
        /// </summary>
        public void AppleLogin();

        public bool IsLoggingIn();
    }
}