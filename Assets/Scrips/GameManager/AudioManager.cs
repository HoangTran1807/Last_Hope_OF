using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager<AudioManager>
{


    protected override void Awake()
    {
    base.Awake();

        InitAudio();
    }


    private enum BGMType
    {
        None,
        Menu,
        InGame
    }
    private BGMType currentBGMType = BGMType.None;


    // ==== Hằng số cài đặt ====
    private const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;   // tốc độ fade nhanh
    private const float BGM_FADE_SPEED_RATE_LOW = 0.3f;    // tốc độ fade chậm

    private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY"; // key lưu volume BGM
    private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";   // key lưu volume SE
    private const float BGM_VOLUME_DEFAULT = 0.2f;          // volume mặc định cho BGM
    private const float SE_VOLUME_DEFAULT = 1f;             // volume mặc định cho SE

    private const string BGM_MUTE_KEY = "BGM_MUTE_KEY";     // key lưu mute BGM
    private const string SE_MUTE_KEY = "SE_MUTE_KEY";       // key lưu mute SE

    // ==== Trạng thái hiện tại ====
    private float bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;
    private string nextBGMName;      // tên bài nhạc sẽ phát tiếp sau khi fade
    private bool isFadeOut = false;  // có đang fade out không

    // ==== Audio Sources gắn sẵn trong Unity Inspector ====
    public AudioSource AttachBGMSource; // nguồn phát nhạc nền
    public AudioSource AttachSESource;  // nguồn phát hiệu ứng âm thanh

    // ==== Từ điển chứa các file nhạc ====
    private Dictionary<string, AudioClip> menuBgmDic;   // nhạc nền cho Menu
    private Dictionary<string, AudioClip> ingameBgmDic; // nhạc nền trong Game
    private Dictionary<string, AudioClip> seDic;        // hiệu ứng âm thanh

    // biến lưu trữ chức năng phát nhạc nền ngẫu nhiên 
    private bool autoPlayRandomInGame = false;   // Bật/tắt chế độ tự phát ngẫu nhiên
    private string currentInGameBGM = "";        // Bài đang phát hiện tại

    // Bật chế độ phát nhạc ngẫu nhiên liên tục trong InGame
    public void PlayRandomBGM(bool isInGame)
    {
        currentBGMType = isInGame ? BGMType.InGame : BGMType.Menu;
        PlayNextRandomBGM();
    }

    private void PlayNextRandomBGM()
    {
        Dictionary<string, AudioClip> dic;

        if (currentBGMType == BGMType.Menu)
            dic = menuBgmDic;
        else if (currentBGMType == BGMType.InGame)
            dic = ingameBgmDic;
        else
            return;

        if (dic.Count == 0)
        {
            Debug.LogWarning("Không có BGM nào trong dictionary!");
            return;
        }

        List<string> keys = new List<string>(dic.Keys);

        // Tránh lặp lại bài đang phát
        if (!string.IsNullOrEmpty(currentInGameBGM) && keys.Count > 1)
            keys.Remove(currentInGameBGM);

        string randomKey = keys[Random.Range(0, keys.Count)];
        currentInGameBGM = randomKey;

        // Phát nhạc đúng loại
        if (currentBGMType == BGMType.Menu)
            PlayMenuBGM(randomKey);
        else
            PlayInGameBGM(randomKey);
    }
    // Chọn và phát bài ngẫu nhiên tiếp theo
    private void PlayNextRandomInGameBGM()
    {
        if (ingameBgmDic.Count == 0)
        {
            Debug.LogWarning("Không có InGame BGM nào trong dictionary!");
            return;
        }

        List<string> keys = new List<string>(ingameBgmDic.Keys);

        // Tránh lặp lại ngay bài vừa phát
        if (!string.IsNullOrEmpty(currentInGameBGM) && keys.Count > 1)
        {
            keys.Remove(currentInGameBGM);
        }

        string randomKey = keys[Random.Range(0, keys.Count)];
        currentInGameBGM = randomKey;

        PlayInGameBGM(randomKey); // phát nhạc
    }

    // Khởi tạo audio, load toàn bộ file từ Resources
    private void InitAudio()
    {
        menuBgmDic = new Dictionary<string, AudioClip>();
        ingameBgmDic = new Dictionary<string, AudioClip>();
        seDic = new Dictionary<string, AudioClip>();

        // Load nhạc Menu từ thư mục Resources/Audio/BGM/Menu
        object[] menuBgmList = Resources.LoadAll("Audio/BGM/Menu");
        foreach (AudioClip bgm in menuBgmList)
            menuBgmDic[bgm.name] = bgm;

        // Load nhạc InGame từ thư mục Resources/Audio/BGM/InGame
        object[] ingameBgmList = Resources.LoadAll("Audio/BGM/InGame");
        foreach (AudioClip bgm in ingameBgmList)
            ingameBgmDic[bgm.name] = bgm;

        // Load toàn bộ SE từ thư mục Resources/Audio/SE
        object[] seList = Resources.LoadAll("Audio/SE");
        foreach (AudioClip se in seList)
            seDic[se.name] = se;
    }

    private void Start()
    {
        // Lấy volume và mute từ PlayerPrefs (nếu có lưu trước đó)
        AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFAULT);
        AttachSESource.volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFAULT);

        AttachBGMSource.mute = PlayerPrefs.GetInt(BGM_MUTE_KEY, 0) != 0;
        AttachSESource.mute = PlayerPrefs.GetInt(SE_MUTE_KEY, 0) != 0;
    }

    // ==== Phát nhạc Menu ====
    public void PlayMenuBGM(string bgmName)
    {
        if (!menuBgmDic.ContainsKey(bgmName))
        {
            Debug.LogWarning($"Không tìm thấy Menu BGM {bgmName}");
            return;
        }
        PlayBGM(menuBgmDic[bgmName]);
    }

    // ==== Phát nhạc InGame ====
    public void PlayInGameBGM(string bgmName)
    {
        if (!ingameBgmDic.ContainsKey(bgmName))
        {
            Debug.LogWarning($"Không tìm thấy InGame BGM {bgmName}");
            return;
        }
        PlayBGM(ingameBgmDic[bgmName]);
    }

    // ==== Logic phát nhạc chung ====
    private void PlayBGM(AudioClip clip, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    {
        if (!AttachBGMSource.isPlaying)
        {
            // Nếu chưa phát nhạc -> phát ngay
            nextBGMName = "";
            AttachBGMSource.clip = clip;
            AttachBGMSource.Play();
        }
        else if (AttachBGMSource.clip != clip)
        {
            // Nếu đang phát bài khác -> fade out rồi mới phát nhạc mới
            nextBGMName = clip.name;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    // Bắt đầu fade out BGM
    public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        bgmFadeSpeedRate = fadeSpeedRate;
        isFadeOut = true;
    }

    private void Update()
    {
        // Kiểm tra nếu auto-play và nhạc đã dừng
        if (autoPlayRandomInGame && !AttachBGMSource.isPlaying && !isFadeOut)
        {
            PlayNextRandomInGameBGM();
        }

        if (!isFadeOut) return;

        // Giảm volume dần theo thời gian
        AttachBGMSource.volume -= Time.deltaTime * bgmFadeSpeedRate;

        if (AttachBGMSource.volume <= 0)
        {
            // Khi volume = 0 -> dừng phát
            AttachBGMSource.Stop();
            AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFAULT);
            isFadeOut = false;

            // Nếu có nhạc mới thì phát ngay sau khi fade out xong
            if (!string.IsNullOrEmpty(nextBGMName))
            {
                if (menuBgmDic.ContainsKey(nextBGMName))
                    PlayBGM(menuBgmDic[nextBGMName]);
                else if (ingameBgmDic.ContainsKey(nextBGMName))
                    PlayBGM(ingameBgmDic[nextBGMName]);
            }
        }
    }

    // ==== Phát SE (Sound Effect) ====
    public void PlaySE(string seName)
    {
        if (!seDic.ContainsKey(seName))
        {
            Debug.LogWarning($"Không tìm thấy SE {seName}");
            return;
        }
        Debug.Log("player sound: " + seName);
        AttachSESource.PlayOneShot(seDic[seName]);
    }

    // ==== Điều chỉnh Volume ====
    public void ChangeBGMVolume(float volume)
    {
        AttachBGMSource.volume = volume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
    }

    public void ChangeSEVolume(float volume)
    {
        AttachSESource.volume = volume;
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, volume);
    }

    // ==== Điều chỉnh Mute ====
    public void MuteBGM(bool isMute)
    {
        AttachBGMSource.mute = isMute;
        PlayerPrefs.SetInt(BGM_MUTE_KEY, isMute ? 1 : 0);
    }

    public void MuteSE(bool isMute)
    {
        AttachSESource.mute = isMute;
        PlayerPrefs.SetInt(SE_MUTE_KEY, isMute ? 1 : 0);
    }

    // ==== Dừng hẳn nhạc BGM (ví dụ khi thoát game, pause) ====
    public void StopAllBGM()
    {
        AttachBGMSource.Stop();
        nextBGMName = "";
        isFadeOut = false;
    }

    public void PlayClickEffect()
    {
        PlaySE("mouseClick");
    }
}
