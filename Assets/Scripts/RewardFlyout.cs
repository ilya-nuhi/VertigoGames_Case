using UnityEngine;
using DG.Tweening; // Include DOTween namespace for tweening

public class RewardFlyout : MonoBehaviour
{
    public GameObject rewardPrefab;    // Prefab for each reward sprite
    public Transform startPoint;       // Start position (e.g., button position)
    public Transform targetPoint;      // Target position (e.g., inventory position)
    public int rewardCount = 5;        // Number of reward sprites to animate
    public float animationDuration = 1f; // Duration of the flyout animation

    public void ClaimRewards()
    {
        for (int i = 0; i < rewardCount; i++)
        {
            GameObject reward = Instantiate(rewardPrefab, startPoint.position, Quaternion.identity, transform);
            AnimateReward(reward);
        }
    }

    private void AnimateReward(GameObject reward)
    {
        // Random offset to spread out the rewards visually
        Vector3 randomOffset = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
        
        // Move reward to randomOffset first, then target point, and then destroy
        reward.transform.DOMove(startPoint.position + randomOffset, animationDuration / 2)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => 
            {
                reward.transform.DOMove(targetPoint.position, animationDuration / 2)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => Destroy(reward));
            });
    }
}
