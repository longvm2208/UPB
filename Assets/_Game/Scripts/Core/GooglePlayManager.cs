using Google.Play.AppUpdate;
using Google.Play.Review;
using System.Collections;
using UnityEngine;

public class GooglePlayManager : MonoBehaviour
{
    public void RequestAndLaunchInAppReview()
    {
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            var reviewManager = new ReviewManager();
            var requestFlowOperation = reviewManager.RequestReviewFlow();

            yield return requestFlowOperation;

            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.LogError("Request in app review failed");
                yield break;
            }

            var playReviewInfo = requestFlowOperation.GetResult();
            var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);

            yield return launchFlowOperation;

            playReviewInfo = null;

            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.LogError("Launch in app review failed");
                yield break;
            }

            Debug.Log("Launch in app review successfully");
        }
    }

    public void CheckForUpdate()
    {
        StartCoroutine(CheckForUpdateRoutine());
    }

    private IEnumerator CheckForUpdateRoutine()
    {
        var appUpdateManager = new AppUpdateManager();
        var appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;

        if (!appUpdateInfoOperation.IsSuccessful)
        {
            Debug.LogError("Get app update info failed");
            yield break;
        }

        var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
        var stalenessDays = appUpdateInfoResult.ClientVersionStalenessDays;
        var priority = appUpdateInfoResult.UpdatePriority;

        if (priority >= 3 || stalenessDays > 10)
        {
            StartCoroutine(ImmediateUpdateRoutine());
        }
        else
        {
            StartCoroutine(FlexibleUpdateRoutine());
        }

        IEnumerator FlexibleUpdateRoutine()
        {
            var appUpdateOptions = AppUpdateOptions.FlexibleAppUpdateOptions();
            var updateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

            while (!updateRequest.IsDone)
            {
                Debug.Log(updateRequest.DownloadProgress);

                yield return null;
            }

            var result = appUpdateManager.CompleteUpdate();

            yield return result;

            Debug.LogError($"Update failed. Error: {result.Error}");
        }

        IEnumerator ImmediateUpdateRoutine()
        {
            var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
            var updateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

            yield return updateRequest;
        }
    }
}
