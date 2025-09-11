using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite unmuteSprite;

    [Header("UI Controls")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Button bgmMuteButton;
    [SerializeField] private Button seMuteButton;

    private float bgmVolume;
    private float seVolume;

    private bool isBGMMuted;
    private bool isSEMuted;


    void Start()
    {
        SyncUIWithAudio();
    }

    private void OnEnable()
    {
        SyncUIWithAudio();
    }

    /// <summary>
    /// Đồng bộ giá trị Slider và trạng thái Mute với AudioManager
    /// </summary>
    private void SyncUIWithAudio()
    {
        if (!AudioManager.HasInstance) return;

        bgmVolume = AudioManager.Instance.AttachBGMSource.volume;
        seVolume = AudioManager.Instance.AttachSESource.volume;
        bgmSlider.value = bgmVolume;
        seSlider.value = seVolume;

        isBGMMuted = AudioManager.Instance.AttachBGMSource.mute;
        isSEMuted = AudioManager.Instance.AttachSESource.mute;

        UpdateMuteButtonSprites();
    }

    /// <summary>
    /// Đổi sprite nút theo trạng thái mute hiện tại
    /// </summary>
    private void UpdateMuteButtonSprites()
    {
        bgmMuteButton.image.sprite = isBGMMuted ? muteSprite : unmuteSprite;
        seMuteButton.image.sprite = isSEMuted ? muteSprite : unmuteSprite;
    }

    // ==== Event của slider ====
    public void OnSliderChangeBGMValue(float v) => bgmVolume = v;
    public void OnSliderChangeSEValue(float v) => seVolume = v;

    // ==== Event khi ấn nút Mute ====
    public void OnBGMMuteButtonClick()
    {
        isBGMMuted = !isBGMMuted;
        UpdateMuteButtonSprites();
    }

    public void OnSEMuteButtonClick()
    {
        isSEMuted = !isSEMuted;
        UpdateMuteButtonSprites();
    }

    // ==== Nút Apply (Submit) ====
    public void OnSubmitButtonClick()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.ChangeBGMVolume(bgmVolume);
            AudioManager.Instance.ChangeSEVolume(seVolume);
            AudioManager.Instance.MuteBGM(isBGMMuted);
            AudioManager.Instance.MuteSE(isSEMuted);
        }

    }

    // ==== Nút Close ====
    public void OnCloseButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
