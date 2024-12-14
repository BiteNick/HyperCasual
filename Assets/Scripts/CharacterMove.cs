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
    [SerializeField] private float minAngleY;
    [SerializeField] private float maxAngleY;

    [SerializeField] private float maxLeftBound;
    [SerializeField] private float maxRightBound;

    [SerializeField] private int MaximumMoneysOnLevel; //how much moneys need for maximum status

    public int currentIndex = 0;
    private int _indexDivision;
    [SerializeField] private List<GameObject> models; //0 - pooriest
    [SerializeField] private List<string> modelsName; //name for every model
    [SerializeField] private List<Animation> animations; //animation for each model

    [SerializeField] private TextMeshProUGUI _modelNameText;

    [SerializeField] private List<Color> _colorsList;

    [SerializeField] Transform parentTransform;

    [SerializeField] private Animator _animator;
    [SerializeField] private Slider richSlider;
    [SerializeField] private Image sliderFill;
    [SerializeField] private FloiatingText _floiatingText;

    [SerializeField] private InterfaceManager _interfaceManager;

    private float _oldMousePositionX;
    private float _deltaX;
    private float _eulerY;
    private Vector3 newPosition;

    private bool finished = false;


    void Awake()
    {
        if (models.Count != modelsName.Count && models.Count != animations.Count)
        {
            Debug.Log("Please set Equal count for models, modelsName, colors and animations lists!!");
        }
        _indexDivision = MaximumMoneysOnLevel / models.Count;

    }


    public void Init()
    {
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
                newPosition = transform.localPosition + transform.right * _speed * Time.deltaTime;
            }
            else if(parentTransform.eulerAngles.y > 45)
            {
                newPosition = transform.localPosition + transform.right * -1 * _speed * Time.deltaTime;
            }
            else
            {
                newPosition = transform.localPosition + transform.forward * _speed * Time.deltaTime;
            }
            newPosition.x = Mathf.Clamp(newPosition.x, maxLeftBound, maxRightBound);
            transform.localPosition = new Vector3(newPosition.x, transform.localPosition.y, transform.localPosition.z);

            _deltaX = Input.mousePosition.x - _oldMousePositionX;
            _oldMousePositionX = Input.mousePosition.x;

            _eulerY += _deltaX;
            _eulerY = Mathf.Clamp(_eulerY, minAngleY, maxAngleY);
            transform.localEulerAngles = new Vector3(0, _eulerY, 0);
        }

    }


    public void PickUp(int moneysForPick, Vector3 position)
    {
        MoneysManager.PickUpMoney(moneysForPick);

        //_floiatingText.gameObject.SetActive(true);
        //_floiatingText.Init(moneysForPick, position);

        MoneysCheck();
    }

    private void MoneysCheck()
    {
        richSlider.value = (float)MoneysManager.currentMoneys / MaximumMoneysOnLevel;
        int index = MoneysManager.currentMoneys / _indexDivision;
        if (currentIndex == index)
            return;
        else
        {
            if (index > currentIndex)
            {
                _modelNameText.transform.DOScale(0.05f, 0.5f).From(0).SetEase(Ease.OutBounce);
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
        _modelNameText.color = _colorsList[currentIndex];
    }

    private void ClearAnimations()
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, false);
        }
    }

    public void ChangeRotation(float degrees)
    {
        transform.DOLocalMoveX(0f, 0.5f);
        parentTransform.DORotate(new Vector3(0, degrees, 0), 0.5f);
    }

    public void DoorOpened()
    {
        _modelNameText.gameObject.SetActive(false);
        richSlider.gameObject.SetActive(false);
    }

    public void Finish()
    {
        _speed = 0;
        _animator.SetTrigger("Dancing");
        finished = true;
        _interfaceManager.Finish();
    }

}
