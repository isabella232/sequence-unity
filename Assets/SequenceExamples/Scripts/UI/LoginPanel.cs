using System.Net.Sockets;
using Sequence.Authentication;
using Sequence.WaaS;
using UnityEngine;

namespace Sequence.Demo
{
    public class LoginPanel : UIPanel
    {
        private TransitionPanel _transitionPanel;
        private ConnectPage _connectPage;
        private LoginPage _loginPage;
        private MultifactorAuthenticationPage _mfaPage;
        private LoginSuccessPage _loginSuccessPage;
        private WaaSDemoPage _waasDemoPage;
        
        protected override void Awake()
        {
            base.Awake();
            _transitionPanel = FindObjectOfType<TransitionPanel>();
            
            _connectPage = GetComponentInChildren<ConnectPage>();
            _loginPage = GetComponentInChildren<LoginPage>();
            _mfaPage = GetComponentInChildren<MultifactorAuthenticationPage>();

            _waasDemoPage = FindObjectOfType<WaaSDemoPage>();
            
            ILogin loginHandler = new WaaSLogin(new AWSConfig(
                "us-east-2", 
                "us-east-2:42c9f39d-c935-4d5c-a845-5c8815c79ee3", 
                "arn:aws:kms:us-east-2:170768627592:key/0fd8f803-9cb5-4de5-86e4-41963fb6043d"),
                9, "1.0.0");
            SetupLoginHandler(loginHandler);

            _loginSuccessPage = GetComponentInChildren<LoginSuccessPage>();
        }

        public void SetupLoginHandler(ILogin loginHandler)
        {
            _loginPage.SetupLogin(loginHandler);
            loginHandler.OnMFAEmailSent += OnMFAEmailSentHandler;
            loginHandler.OnMFAEmailFailedToSend += OnMFAEmailFailedToSendHandler;
            
            _mfaPage.SetupLogin(loginHandler);
            loginHandler.OnLoginSuccess += OnLoginSuccessHandler;
            loginHandler.OnLoginFailed += OnLoginFailedHandler;

            if (loginHandler is WaaSLogin waaSLogin)
            {
                waaSLogin.OnWaaSWalletCreated += OnWaaSWalletCreatedHandler;
            }
        } 

        public void OpenTransitionPanel()
        {
            _transitionPanel.OpenWithDelay(_closeAnimationDurationInSeconds);
        }

        private void OnLoginSuccessHandler(string sessionId, string walletAddress)
        {
            Debug.Log($"Successful login with session Id: {sessionId} | wallet address: {walletAddress}");
            StartCoroutine(SetUIPage(_loginSuccessPage));
        }

        private void OnLoginFailedHandler(string error)
        {
            Debug.LogError($"Failed login: {error}");
        }

        private void OnMFAEmailSentHandler(string email)
        {
            Debug.Log($"Successfully sent MFA email to {email}");
            StartCoroutine(SetUIPage(_mfaPage, email));
        }

        private void OnMFAEmailFailedToSendHandler(string email, string error)
        {
            Debug.Log($"Failed to send MFA email to {email} with error: {error}");
        }
        
        // On Windows standalone, deep link will open a second instance of tghe game.
        // We need to catch this, and send our deep link URL to the already-running instance (through a TCP server)
#if UNITY_STANDALONE_WIN
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void PassDeepLinkViaLocalServer()
        {
            var args = System.Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1].StartsWith(OpenIdAuthenticator.UrlScheme))
            {
                var socketConnection = new TcpClient("localhost", OpenIdAuthenticator.WINDOWS_IPC_PORT);
                var bytes = System.Text.Encoding.ASCII.GetBytes("@@@@" + args[1] + "$$$$");
                socketConnection.GetStream().Write(bytes, 0, bytes.Length);
                socketConnection.Close();
                Application.Quit();
            }
        }
#endif

        private void OnWaaSWalletCreatedHandler(WaaSWallet wallet)
        {
            _waasDemoPage.Open(wallet);
        }
    }
}