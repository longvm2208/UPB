using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : SingletonMonoBehaviour<IAPManager>, IDetailedStoreListener
{
#if UNITY_EDITOR
    private const string environment = "dev";
#else
    private const string environment = "production";
#endif

    [SerializeField, ExposedScriptableObject]
    private IAPProductContainer container;

    private bool isInitialized = false;
    private IStoreController controller;
    private IExtensionProvider extensions;

    public bool IsInitialized => isInitialized;

    private async void Start()
    {
        await InitializeUnityServices();

        Initialize();
    }
    
    private async Task InitializeUnityServices()
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch (System.Exception exception)
        {
            Debug.LogError("Unity services failed to initialize. Exception: " + exception);
        }
    }

    private void Initialize()
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
        HandlePurchase(purchaseEvent.purchasedProduct.definition.id);

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase failed. Product: {product.definition.id}. Reason: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogError($"Purchase failed. Product: {product.definition.id}. Reason: {failureDescription.reason}. Details: {failureDescription.message}");
    }
    #endregion

    private void FetchAdditionalProducts()
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

    private void HandlePurchase(string id)
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

    public string GetLocalizedPriceString(string id)
    {
        Product product = controller.products.WithID(id);
        return product != null ? product.metadata.localizedPriceString : "";
    }

    public void OnPurchaseClicked(string id)
    {
        controller.InitiatePurchase(id);
    }
}
