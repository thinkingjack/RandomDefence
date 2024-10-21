using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    // UI 슬라이더에 연결할 변수
    public Slider volumeSlider;

    private bool isMuted = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 초기 슬라이더 값을 현재 오디오 볼륨으로 설정
        volumeSlider.value = audioSource.volume;

        // 슬라이더 값이 변경될 때마다 SetVolume 함수 실행
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // 볼륨 조절 기능
    void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}