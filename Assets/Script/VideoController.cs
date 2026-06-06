using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;

    void Start()
    {
        // Hubungkan output video ke RawImage
        videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0);
        rawImage.texture = videoPlayer.targetTexture;

        // Play otomatis saat scene mulai
        videoPlayer.Play();
    }

    // Fungsi untuk Play manual (opsional)
    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    // Fungsi Pause
    public void PauseVideo()
    {
        videoPlayer.Pause();
    }

    // Fungsi Stop
    public void StopVideo()
    {
        videoPlayer.Stop();
    }
}