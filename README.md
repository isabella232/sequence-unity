# Sequence Unity SDK

## Documentation
https://docs.sequence.xyz/unity-waas-sdk

## Requirements
Unity 2021.3.6f1 or later

## Supported Platforms

- Android
- iOS
- PC standalone
- Mac standalone -> (Mono builds only when using OpenIdAuthentication -> in our testing MacOS doesn't pick up custom URL schemes automatically unless you run some system commands first; these system commands only work on Mono -> see OpenIdAuthentication.PlatformSpecificSetup)
- WebGL (excluding OpenIdAuthentication)

## Contributing

As an open source project, we welcome and encourage your contributions! However, due to security reasons, we require that all commits be signed and verified on GitHub in order to merge into our master branch. So before contributing, you'll want to spend a few minutes setting that up if you haven't already.

To start signing your commits, please see: https://docs.github.com/en/authentication/managing-commit-signature-verification/signing-commits

## Testing
The project makes use of Unity's test runner. You can open the test runner window in 
Unity by navigating to `Window > General > Test Runner`.

Many of the tests make use of a Hardhat-based testchain. This testchain can be found 
in the root folder of the project - in case you are experiencing issues with it.

Before running tests, boot up the test chain with `make start-testchain`. You may find 
that you need to stop (control + c) the testchain and restart it between some of the 
tests.

### Testing via command line
It can sometimes be useful to quickly test the project via command line. This can be done without opening Unity or starting the testchain.
#### One-Time Setup
Add this line to your `~/.zshrc` or `~/.bashrc`
`export PATH="/Applications/Unity/Hub/Editor/2021.3.6f1/Unity.app/Contents/MacOS:$PATH"` - note: this is an example path, the exact path may vary based on your system
Then
`source ~/.bashrc` or `source ~/.zshrc`
Then
`touch TestResults.xml` from the route directory of the project

Run `make bootstrap` to install dependancies locally that are required for running the testchain.
#### Running the test
To run the test please use
`make test`
This will automatically start the testchain and open Unity to run the tests. When the tests are finished, the testchain and Unity will be shutdown.
The test results can be found in `TestResults.xml` located in the root directory of the project. The Makefile command will automatically display a summary of the test results.
When a test fails, it is recommended that you open up Unity and test via the usual method.
Note: Please do not run `make test` while you have the project open in Unity - the tests will not run and you will need to `touch TestResults.xml` again.
### Testing the test chain
Occasionally, it may be necessary to test the testchain to a) confirm it is 
giving the behaviours you expect and b) to use for comparison with our 
Unity tests. We can safely assume that ethers (which we use to test the 
testchain) works correctly. To test please use `make test-testchain`. Test output will be in 
chaintest.out and will also be printed to the terminal. If you need to end the test suite 
early, use `Control+C` and `make stop`.
*Note: if you already have an instance of Unity running, this will open up a new instance of Unity that will terminate upon completion.

### Troubleshooting
Do you have tests that are failing that you don't think should be or were previously passing and you haven't changed anything?
Here are a few things to try:
1. If you are or were using a debugger, disconnect the debugger from Unity and then reconnect
2. Restart the test chain `Control+C` and `make start-testchain`
3. Restart Unity
* Also note that since tests on the testchain are being run sequentially, if a prior test fails, it may not 
have unwound properly and may leave the next test in an unexpected state - causing it to fail.

## Component Overview
The SDK is broken into a number of components with different responsibilities. This section will give an overview of some of the most important components for users and their intended purposes.

### Client
IEthClient provides an interface for clients. Clients handle the connection to blockchain networks, making various RPC requests. Any time you wish to query the blockchain or submit a transaction, you will need a client. As a rule of thumb, if a method requires a client, you should expect that you will be making a web request and will need to work with async tasks and be prepared to catch any exceptions that are thrown.

### Wallet
EthWallet implements a standard EOA wallet. A wallet keeps track of its own private/public key pair and address and is responsible for providing its private key to the signer when signing transactions.

### Transaction
A transaction, as implemented in EthTransaction, contains all the data and parameters for an EVM transaction. The object is used for initiating its RLP encoding (transactions must be signed and RLP encoded when submitted). Note that all transactions are encoded with a chain Id included to protect against replay attacks, see [EIP-155](https://eips.ethereum.org/EIPS/eip-155).

### Contract
A contract is responsible for creating transactions (for method calls) and messages (for queries) agaisnt it. These transactions are later signed by the wallet + signer and submitted (along with query messages) using a client.

## Sample UI
A sample UI scene can be found under `Assets > SequenceExamples > Scenes > Demo.unity`
This scene contains sample Sequence UI (with integration) for login flow and wallet view, settings, and transaction flow.

### How It Works
The sample Sequence UI is comprised of a few important components.

#### UIPage
A `UIPage` is the base implementation of a "page" in the sample UI. Example pages: `LoginPage`, `TokenInfoPage`
It is responsible for opening/closing the page and managing the chosen `ITween`.

#### ITween
An `ITween` is an interface for an animation (in/out) that can be applied to a `RectTransform` (a required component of a `UIPage`).

#### UIPanel
Inherriting from `UIPage`, a `UIPanel` is the base implementation of a "panel" in the sample UI. Example panels: `LoginPanel`, `WalletPanel`
In addition to `UIPage` responsibilities, UIPanels maintain a stack of UIPages and `object[]` (open arguments) and are responsible for managing any required event listeners and UI transitions between their child pages (according to Scene inspector heirarchy), including handling the "Back" button.

#### SequenceSampleUI
`SequenceSampleUI` can be thought of as the "manager" of the sample UI. It holds a reference to all the UIPanels and is responsible for opening them as needed, including at `Start()`. If you are integrating all or part of the provided sample UI into your project, you may find it more practical to replace `SequenceSampleUI` with your own UI "manager".

### Customizations + Color Schemes
As a Made-With-Unity UI, the sample UI is cross platform and easily customizable.
To make customization even easier, the sample UI comes equipped with a Color Scheme Manager. This monobehaviour script is attached to the `SequenceCanvas` gameObject. By attaching a `ColorScheme` scriptable object and clicking the `Apply` button in the Inspector, the `ColorSchemeManager` will quickly apply the desired color scheme, allowing for faster UI iterations.
To create a `ColorScheme` scriptable object, go to `Assets > Create > Sequence > Color Scheme`. From here, you can give the color scheme a name, move it to the desired directory, and choose your colors.

## Architecture Decision Records

Please add any ADRs below. In the future, it may be worthwhile to move these into 
separate files, but for now since there are few ADRs, the README should suffice. 
Please use [Michael Nygard's template for 
ADRs](https://github.com/joelparkerhenderson/architecture-decision-record/blob/main/templates/decision-record-template-by-michael-nygard/index.md)

### ADR 3 - WaaS Client
August 2, 2023 - author: Quinn Purdy
Updated Aug 16, 2023 - author: Quinn Purdy
Updated Jan 3, 2023 - author: Quinn Purdy

#### Status
Approved

#### Context
A direct integration of Sequence into sequence-unity is a time-intensive process and requires porting over logic from go-sequence and/or sequence.js. Recently, we've established a WaaS service that exposes the core logic from go-sequence via http. This WaaS service, with our authentication system, can be used to provide users with a more secure and more frictionless (less "wallet-like") UX.

#### Decision
In order to save time on the integration and provide users with a more secure and frictionless UX, sequence-unity will integrate directly with the WaaS service, iceboxing the implementation of "SequenceCore" (see ADR 2) for a later date.

For authentication, sequence-unity will use the [OIDC implicit flow](https://openid.net/specs/openid-connect-implicit-1_0.html#ImplicitFlow) or AWS Cognito Email with OTP Sign In to obtain an idToken, which is combined with some config variables to establish a session with the WaaS service. The Authentication logic (obtaining the idToken) can be found in the `SequenceAuthentication` assembly.

The SDK will require developers to input a number of config variables during setup. This will be done via a ScriptableObject, defined in the `SequenceConfig` assembly, that can be fetched via [Resources.Load](https://docs.unity3d.com/ScriptReference/Resources.Load.html) when needed.

Similar to ADR 2, the WaaS client will be implemented in a separate assembly from "SequenceEthereum". This assembly will be called "SequenceWaaS" and will reference and depend on the Ethereum library assembly "SequenceEthereum".

Since use of WaaS requires an idToken that cannot currently be hardcoded, some of the tests live in a separate assembly, `SequenceWaaSEndToEndTests`, that is used when building the `WaaSEndToEndTests` scene for end to end testing. Additionally, we've included unit tests, and other tests using mocks that can be run from within the editor, in the `SequenceWaaSTests` assembly. 

#### Consequences
As the WaaS client will rely on network requests, interactions will be slower than with a direct integration. However, the speed to market with this approach is greatly improved and there is a better UX for end-users and developers alike, justifying the trade-off.

Additionally, since the WaaS client relies on network requests, we must add additional async Tasks to the SequenceEthereum IWallet interface. This will require additional await statements throughout, harming readability.

Remaining consequences follow those from ADR 2 (with respect to assemblies).

### ADR 2 - Separate assemblies for Sequence integration and Ethereum library
July 12, 2023 - author: Quinn Purdy

#### Status
Accepted - July 14, 2023

#### Context
Integration of Sequence into the sequence-unity is the next step in the Unity SDK project - preparations are being made, with modifications to project structure.

#### Decision
Move the previous Sequence integration work, and all future Sequence integration work, into a separate assembly from the Ethereum libraries we're writing. The Sequence "SequenceCore" assembly will reference and depend on the Ethereum library assembly "SequenceEthereum".

For now, all tests will remain in the same assembly "SequenceTests".

#### Consequences
While SequenceCore will be able to reference namespaces in SequenceEthereum, SequenceEthereum will not be able to reference anything in SequenceCore. While, on the surface, this may sound problematic as it reduces our flexibility when writing the SDK, SequenceEthereum should not need to depend on SequenceCore; this will reduce coupling leading to an overall more readable and maintainable project. This also makes it easier for us to release SequenceEthereum as a standalone package, should we ever choose to do so.

By splitting SequenceCore into a separate assembly, we will not need to recompile the entire SDK whenever we make changes to SequenceCore; instead, we will only need to recompile SequenceCore. Similarly, if we were to precompile the SDK, this would give us two separate dlls (SequenceEthereum.dll and SequenceCore.dll).

### ADR 1 - sequence-unity
June 21, 2023 - author: Quinn Purdy

#### Status
This ADR document is being made retroactively after inheriting the project.

#### Context
Sequence Unity SDK v1 was made quickly as a proof of concept. The SDK relies on 
Nethereum; a library that is overly heavy-weight. The SDK also relies on the Vuplex 
webview unity package - this package is not free, leading to developer frustrations.

#### Decision
Modifying the existing v1 SDK was deemed to be unworthy undertaking. Building a new 
SDK from scratch was determined to be faster and easier.

#### Consequences
Iteration on SDK v2 during development will be significantly faster and lower risk 
than modifying the 
existing SDK the customers are currently using. However, this means that current 
customers using v1 of the SDK can expect limited support during the development of SDK 
v2 as v1 will be deprecated. 
