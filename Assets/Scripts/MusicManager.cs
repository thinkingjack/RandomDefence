using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    // UI �����̴��� ������ ����
    public Slider volumeSlider;

    private bool isMuted = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // �ʱ� �����̴� ���� ���� ����� �������� ����
        volumeSlider.value = audioSource.volume;

        // �����̴� ���� ����� ������ SetVolume �Լ� ����
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // ���� ���� ���
    void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}