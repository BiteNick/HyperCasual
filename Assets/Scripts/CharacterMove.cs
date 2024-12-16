using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedX; //can to change
    private float _defaultSpeedX; //no change
    [SerializeField] private float minAngleY;
    [SerializeField] private float maxAngleY;

    [SerializeField] private float maxLeftBound;
    [SerializeField] private float maxRightBound;

    [SerializeField] private float _rotationToForward;

    [SerializeField] public int MaximumMoneysOnLevel; //how much moneys need for maximum status

    public int currentIndex = 0;
    private int _indexDivision;
    [SerializeField] private List<GameObject> models; //0 - pooriest
    [SerializeField] private List<string> modelsName; //name for every model
    [SerializeField] private List<Animation> animations; //animation for each model

    [SerializeField] private TextMeshProUGUI _modelNameText;

    [SerializeField] private List<Color> _colorsList;

    [SerializeField] Transform parentTransform;

    [SerializeField] private Animator _animator;
    [SerializeField] private Slider _richSlider;
    [SerializeField] private Image sliderFill;

    [SerializeField] private InterfaceManager _interfaceManager;

    private float _oldMousePositionX;
    private float _deltaX;
    private float _eulerY;
    private Vector3 newPosition;

    private bool finished = false;

    [SerializeField] private ParticleSystem _moneysSpreadParticle;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _modelUpClip;
    [SerializeField] private AudioClip _finishClip;

    [SerializeField] private FloatingText FloatingText;


    void Awake()
    {
        _defaultSpeedX = _speedX;
        if (models.Count != modelsName.Count && models.Count != animations.Count)
        {
            Debug.Log("Please set Equal count for models, modelsName, colors and animations lists!!");
        }
        _indexDivision = MaximumMoneysOnLevel / models.Count;

        RichUIEnable(false);
    }


    public void Init() //calls when character starts running
    {
        RichUIEnable(true);

        MoneysCheck();
        ModelChange();
    }

    void Update()
    {

        newPosition = parentTransform.position + parentTransform.forward * _speed * Time.deltaTime;
        parentTransform.position = newPosition;



        if (Input.GetMouseButton(0) && !finished)
        {
            if (parentTransform.eulerAngles.y < -45 || parentTransform.eulerAngles.y > 260)
            {
                newPosition = transform.localPosition + transform.right * _speedX * Time.deltaTime;
            }
            else if(parentTransform.eulerAngles.y > 45)
            {
                newPosition = transform.localPosition + transform.right * -1 * _speedX * Time.deltaTime;
            }
            else
            {
                newPosition = transform.localPosition + transform.forward * _speedX * Time.deltaTime;
            }
            newPosition.x = Mathf.Clamp(newPosition.x, maxLeftBound, maxRightBound);
            transform.localPosition = new Vector3(newPosition.x, transform.localPosition.y, transform.localPosition.z);

            _deltaX = Input.mousePosition.x - _oldMousePositionX;
            _oldMousePositionX = Input.mousePosition.x;

            _eulerY += _deltaX;
            _eulerY = Mathf.Clamp(_eulerY, minAngleY, maxAngleY);
            transform.localEulerAngles = new Vector3(0, _eulerY, 0);
        }
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), _rotationToForward * Time.deltaTime);

    }


    public void PickUp(int moneysForPick, AudioClip[] clips)
    {
        MoneysManager.PickUpMoney(moneysForPick);

        _moneysSpreadParticle.gameObject.SetActive(true);
        _moneysSpreadParticle.Play();

        AudioPlay(clips);


        FloatingText.gameObject.SetActive(true);
        FloatingText.FloatingTextCall(moneysForPick);

        MoneysCheck();
    }

    private void MoneysCheck()
    {
        _richSlider.value = (float)MoneysManager.currentMoneys / MaximumMoneysOnLevel;
        int index = MoneysManager.currentMoneys / _indexDivision;
        if (currentIndex == index)
            return;
        else
        {
            if (index > currentIndex)
            {
                _modelNameText.transform.DOScale(0.05f, 0.5f).From(0).SetEase(Ease.OutBounce);
                AudioPlay(_modelUpClip);
                _animator.SetTrigger("Jump");
            }
            currentIndex = index;
        }

        ModelChange();
    }

    private void ModelChange()
    {
        if (currentIndex >= models.Count)
        {
            currentIndex = models.Count - 1;
        }

        foreach (var model in models)
        {
            model.SetActive(false);
        }
        models[currentIndex].SetActive(true);

        ClearAnimations();
        
        _animator.SetBool($"Walk{currentIndex}", true);

        _modelNameText.text = modelsName[currentIndex];
        _modelNameText.color = _colorsList[currentIndex];
        sliderFill.color = _colorsList[currentIndex];
    }

    private void ClearAnimations()
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, false);
        }
    }

    public void CheckpointComplete(AudioClip sound)
    {
        AudioPlay(sound);
    }

    public void ChangeRotation(float degrees, Vector3 CenterPosition)
    {
        _speedX = 0;
        transform.DOLocalMoveX(0f, 0.5f);

        if (degrees == 90f || degrees == 270f)
            parentTransform.DOMoveZ(CenterPosition.z, 0.5f);
        else if (degrees == 0f || degrees == 180f)
            parentTransform.DOMoveX(CenterPosition.x, 0.5f);

        parentTransform.DORotate(new Vector3(0, degrees, 0), 0.5f);
        Invoke("ChangedRotation", 0.5f);
    }

    private void ChangedRotation()
    {
        _speedX = _defaultSpeedX;
    }

    private void AudioPlay(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    private void AudioPlay(AudioClip[] clips)
    {
        foreach (AudioClip clip in clips)
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    public void DoorOpened(AudioClip clip)
    {
        AudioPlay(clip);
        RichUIEnable(false);
    }

    private void RichUIEnable(bool isEnabled)
    {
        _modelNameText.gameObject.SetActive(isEnabled);
        _richSlider.gameObject.SetActive(isEnabled);
    }

    public void Finish()
    {
        AudioPlay(_finishClip);
        _speed = 0;
        _animator.SetTrigger("Dancing");
        finished = true;
        _interfaceManager.Finish();
    }

}
