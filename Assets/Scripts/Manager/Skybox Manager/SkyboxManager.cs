using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    [SerializeField] private Material[] skyboxes;

    private void Awake()
    {
        ApplyRandomSkybox();
    }

    private void ApplyRandomSkybox()
    {
        if (skyboxes == null || skyboxes.Length == 0)
            return;

        RenderSettings.skybox = skyboxes[Random.Range(0, skyboxes.Length)];

        DynamicGI.UpdateEnvironment();
    }
}
