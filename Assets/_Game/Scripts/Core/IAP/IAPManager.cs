using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

public class IAPManager : SingletonMonoBehaviour<IAPManager>, IDetailedStoreListener
{
#if UNITY_EDITOR
    const string environment = "dev";
#else
    const string environment = "production";
#endif

    [SerializeField] GameObject blocker;
    [SerializeField, ExposedScriptableObject]
    IAPProductContainer container;

    bool isInitialized = false;
    IStoreController controller;
    IExtensionProvider extensions;
    IAPLocation location;

    public bool IsInitialized => isInitialized;

    async void Start()
    {
        await InitializeUnityServices();

        Initialize();
    }
    
    async Task InitializeUnityServices()
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            Debug.LogError("Unity services failed to initialize. Exception: " + exception);
        }
    }

    void Initialize()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));

        for (int i = 0; i < container.Count; i++)
        {
            builder.AddProduct(container[i].id, container[i].type);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    #region INITIALIZE CALLBACKS
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP successfully initialized");

        this.controller = controller;
        this.extensions = extensions;

        //FetchAdditionalProducts();
        RestoreTransaction();

        isInitialized = true;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"IAP failed to initialize. Reason: {error}";

        if (message != null)
        {
            errorMessage += $". Details: {message}";
        }

        Debug.LogError(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        DisableBlocker();

        Product product = purchaseEvent.purchasedProduct;

        if (IsReceiptValid(product))
        {
            FirebaseManager.Instance.LogIapPurchase(
                product.definition.id,
                location.GetDescription());

            AppsFlyerManager.Instance.SendIapPurchase(
                product.metadata.localizedPrice,
                product.metadata.isoCurrencyCode, 1,
                product.definition.id);

            HandlePurchase(product.definition.id);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) { }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogError($"Purchase failed. Product: {product.definition.id}. Reason: {failureDescription.reason}. Details: {failureDescription.message}");

        DisableBlocker();
    }
    #endregion

    void FetchAdditionalProducts()
    {
        var additional = new HashSet<ProductDefinition>()
        {
            new ProductDefinition("id", ProductType.Consumable),
        };

        Action onSuccess = () =>
        {
            Debug.Log("Fetched successfully");
        };

        Action<InitializationFailureReason, string> onFailure = (reason, message) =>
        {
            var errorMessage = $"Fetching additional products failed. Reason: {reason}";

            if (message != null)
            {
                errorMessage += $". Details: {message}";
            }

            Debug.LogError(errorMessage);
        };

        controller.FetchAdditionalProducts(additional, onSuccess, onFailure);
    }

    bool IsReceiptValid(Product product)
    {
        bool isValid = true;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX)
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

        try
        {
            var receipts = validator.Validate(product.receipt);
        }
        catch (IAPSecurityException exception)
        {
            Debug.LogError($"Validation failed. Product: {product.definition.id}. Exception: {exception}");

            isValid = false;
        }
#endif

        return isValid;
    }

    void HandlePurchase(string id)
    {
        switch (id)
        {
            case IAPProductId.Coin50:
                Debug.Log("Receive 50 coin");
                break;
            default:
                Debug.LogError("Invalid product id");
                break;
        }
    }

    void RestoreTransaction()
    {
#if UNITY_ANDROID
        foreach (var product in controller.products.all)
        {
            if (!product.availableToPurchase) continue;
            if (product.receipt == null) continue;

            switch (product.definition.type)
            {
                case ProductType.NonConsumable:
                    break;
                case ProductType.Subscription:
                    break;
                default:
                    break;
            }
        }
#elif UNITY_IOS || UNITY_STANDALONE_OSX
#else
#endif
    }

    public string GetLocalizedPriceString(string id)
    {
        if (isInitialized)
        {
            Product product = controller.products.WithID(id);

            if (product != null)
            {
                return product.metadata.localizedPriceString;
            }
            else
            {
                return "---";
            }
        }
        else
        {
            return "---";
        }
    }

    public void OnPurchaseClicked(string id, IAPLocation location)
    {
        if (isInitialized)
        {
            EnableBlocker();
            this.location = location;
            controller.InitiatePurchase(id);
        }
    }

    public void EnableBlocker()
    {
        blocker.SetActive(true);
    }

    public void DisableBlocker()
    {
        blocker.SetActive(false);
    }
}
