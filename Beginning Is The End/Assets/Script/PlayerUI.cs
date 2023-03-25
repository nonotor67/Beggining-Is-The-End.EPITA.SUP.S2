using Cinemachine;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerUI : Player
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject dieMenu;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform healthBarImage;
    private float _currentHealth;
    private float _maxiHealth;

    private PlayerSetup _playerSetup;
    private PlayerMovement _playerMovement;
    [SerializeField] private CinemachineFreeLook CinemachineFreeLook;
    [SerializeField] private PlayerAnimations playerAnimations;
    private NetworkManager _networkManager;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSetup = GetComponent<PlayerSetup>();
        _networkManager = NetworkManager.singleton;
        _maxiHealth = GetComponent<Player>().maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        UpdateHealth();
    }

    //Gestion du menu pause
    public void Resume()
    {
        if (isLocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);
            _playerSetup.enabled = true;
            playerAnimations.enabled = true;
            healthBar.gameObject.SetActive(true);
            healthBarImage.gameObject.SetActive(true);
            _playerMovement.enabled = true;
            CinemachineFreeLook.enabled = true;
        }
    }

    public void Pause()
    {
        if (isLocalPlayer)
        {
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
            _playerSetup.enabled = false;
            playerAnimations.enabled = false;
            healthBar.gameObject.SetActive(false);
            healthBarImage.gameObject.SetActive(false);
            _playerMovement.enabled = false;
            CinemachineFreeLook.enabled = false;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LeaveButton()
    {
        if (isClientOnly)
        {
            _networkManager.StopClient();
        }
        else
        {
            _networkManager.StopHost();
        }
    }
    
    private bool _isHosting;
    
    public void HostButton()
    {
        if (_isHosting)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StartHost();
        }
        _isHosting = !_isHosting;
    }

   
    public void JoinButton()
    {
        var ipAddress = GameObject.Find("IpAdress").GetComponent<TMP_InputField>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
        NetworkManager.singleton.StartClient();

    }
    
    private void UpdateHealth()
    {
        _currentHealth = GetComponent<Player>().health;
        healthBar.sizeDelta = new Vector2(healthBar.sizeDelta.x, _currentHealth / _maxiHealth * 300);
    }
    
    public void Die()
    {
        if (!isLocalPlayer) return;
        Cursor.lockState = CursorLockMode.None;
        dieMenu.SetActive(true);
        _playerSetup.enabled = false;
        playerAnimations.enabled = false;
        _playerMovement.enabled = false;
        CinemachineFreeLook.enabled = false;
    }
    
    public void respawn()
    {
        if (!isLocalPlayer) return;
        GetComponent<Player>().Respawn();
        Cursor.lockState = CursorLockMode.Locked;
        dieMenu.SetActive(false);
        _playerSetup.enabled = true;
        playerAnimations.enabled = true;
        _playerMovement.enabled = true;
        CinemachineFreeLook.enabled = true;
    }
}
