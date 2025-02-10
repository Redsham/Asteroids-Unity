const yandexPluginLibrary = {

    // Class definition

    $yandexPlugin: {
        isInitialized: false,
        isAuthorized: false,
        sdk: undefined,
        leaderboard: undefined,
        playerAccount: undefined,
        billing: undefined,
        isInitializeCalled: false,

        yandexSdkInitialize: function (successCallbackPtr) {
            if (yandexPlugin.isInitializeCalled) {
                return;
              }
              yandexPlugin.isInitializeCalled = true;
        
              const sdkScript = document.createElement('script');
              sdkScript.src = '/sdk.js';
              document.head.appendChild(sdkScript);
        
              sdkScript.onload = function () {
                window['YaGames'].init().then(function (sdk) {
                  yandexPlugin.sdk = sdk;
        
                  // The { scopes: false } ensures personal data permission request window won't pop up,
                  const playerAccountInitializationPromise = sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
                    if (playerAccount.getMode() !== 'lite') {
                      yandexPlugin.isAuthorized = true;
                    }
        
                    // Always contains permission info. Contains personal data as well if permissions were granted before.
                    yandexPlugin.playerAccount = playerAccount;
                  }).catch(function () { throw new Error('PlayerAccount failed to initialize.'); });
        
                  const leaderboardInitializationPromise = sdk.getLeaderboards().then(function (leaderboard) {
                    yandexPlugin.leaderboard = leaderboard;
                  }).catch(function () { throw new Error('Leaderboard failed to initialize.'); });
        
                  const billingInitializationPromise = sdk.getPayments({ signed: true }).then(function (billing) {
                    yandexPlugin.billing = billing;
                  }).catch(function () { throw new Error('Billing failed to initialize.'); });
        
                  Promise.allSettled([leaderboardInitializationPromise, playerAccountInitializationPromise, billingInitializationPromise]).then(function () {
                    yandexPlugin.isInitialized = true;
                    dynCall('v', successCallbackPtr, []);
                  });
                });
              }
        },

        throwIfSdkNotInitialized: function () {
            if (!yandexPlugin.isInitialized) {
                throw new Error('SDK is not initialized. Invoke YandexGamesSdk.Initialize() coroutine and wait for it to finish.');
            }
        },
        invokeErrorCallback: function (error, errorCallbackPtr) {
            var errorMessage;
            if (error instanceof Error) {
              errorMessage = error.message;
              if (errorMessage === null) { errorMessage = 'SDK API thrown an error with null message.' }
              if (errorMessage === undefined) { errorMessage = 'SDK API thrown an error with undefined message.' }
            } else if (typeof error === 'string') {
              errorMessage = error;
            } else if (error) {
              errorMessage = 'SDK API thrown an unexpected type as error: ' + JSON.stringify(error);
            } else if (error === null) {
              errorMessage = 'SDK API thrown a null as error.';
            } else {
              errorMessage = 'SDK API thrown an undefined as error.';
            }
      
            const errorUnmanagedStringPtr = yandexPlugin.allocateUnmanagedString(errorMessage);
            dynCall('vi', errorCallbackPtr, [errorUnmanagedStringPtr]);
            _free(errorUnmanagedStringPtr);
        },
        invokeErrorCallbackIfNotAuthorized: function (errorCallbackPtr) {
            if (!yandexPlugin.isAuthorized) {
              yandexPlugin.invokeErrorCallback(new Error('Needs authorization.'), errorCallbackPtr);
                return true;
            }
            return false;
        },

        // Game layout
        gameReady: function () {
            yandexPlugin.sdk.features.LoadingAPI.ready();
        },
        gameplayStart: function () {
            yandexPlugin.sdk.features.GameplayAPI.start()
        },
        gameplayStop: function () {
            yandexPlugin.sdk.features.GameplayAPI.stop()
        },

        // Advertising
        showRewardedVideo: function (openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
          yandexPlugin.sdk.adv.showRewardedVideo({
            callbacks: {
              onOpen: function () {
                dynCall('v', openCallbackPtr, []);
              },
              onRewarded: function () {
                dynCall('v', rewardedCallbackPtr, []);
              },
              onClose: function () {
                dynCall('v', closeCallbackPtr, []);
              },
              onError: function (error) {
                yandexPlugin.invokeErrorCallback(error, errorCallbackPtr);
              }
            }
          });
        },
        showFullscreenAdv: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
          yandexPlugin.sdk.adv.showFullscreenAdv({
            callbacks: {
              onOpen: function () {
                dynCall('v', openCallbackPtr, []);
              },
              onClose: function () {
                dynCall('vi', closeCallbackPtr, []);
              },
              onError: function (error) {
                yandexPlugin.invokeErrorCallback(error, errorCallbackPtr);
              }
            }
          });
        }
    },

    // External C# calls.

    // Initialization
    YandexSdkInitialize: function (successCallbackPtr) {
      yandexPlugin.yandexSdkInitialize(successCallbackPtr);
    },
    GetYandexSdkIsInitialized: function () {
        return yandexPlugin.isInitialized;
    },
    
    // Game layout
    YandexSdkGameReady: function () {
      yandexPlugin.throwIfSdkNotInitialized();

      yandexPlugin.gameReady();
    },
    YandexSdkGameplayStart: function () {
      yandexPlugin.throwIfSdkNotInitialized();

      yandexPlugin.gameplayStart();
    },
    YandexSdkGameplayStop: function () {
      yandexPlugin.throwIfSdkNotInitialized();

      yandexPlugin.gameplayStop();
    },

    // Advertising
    YandexSdkShowRewardedVideo: function (openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
      yandexPlugin.throwIfSdkNotInitialized();

      yandexPlugin.showRewardedVideo(openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr);
    },
    YandexSdkShowFullscreenAdv: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
      yandexPlugin.throwIfSdkNotInitialized();

      yandexPlugin.showFullscreenAdv(openCallbackPtr, closeCallbackPtr, errorCallbackPtr);
    }
}

autoAddDeps(yandexPluginLibrary, '$yandexPlugin');
mergeInto(LibraryManager.library, yandexPluginLibrary);