using UnityEngine;

public static class CustomSliderBarUtils
{
    /// <summary>
    /// Updates the bar position based on current and max values of the bar.
    /// </summary>
    /// <param name="barSprite">The RectTransform or GameObject of the bar.</param>
    /// <param name="currentValue">Current bar value.</param>
    /// <param name="maxValue">Maximum bar value.</param>
    /// <param name="barEmptyXPosition">X position when empty.</param>
    /// <param name="barFullXPosition">X position when full.</param>
    public static void UpdateBarPosition(GameObject barSprite, float currentValue, float maxValue, float barEmptyXPosition, float barFullXPosition)
    {
        float positionDifference = barFullXPosition - barEmptyXPosition;
        float healthBarPosition = -1f * (positionDifference - ((currentValue / maxValue) * positionDifference));
        barSprite.transform.localPosition = new Vector3(healthBarPosition, 0f, 0f);
    }
}

