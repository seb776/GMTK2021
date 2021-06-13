using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandlemenuClicks : MonoBehaviour
{
    private static void _menu()
    {
        SceneManager.LoadScene("Menu");
    }
    private static void _credits()
    {
        SceneManager.LoadScene("Credits");
    }
    private static void _game()
    {
        SceneManager.LoadScene("InterfaceAndGame");
    }

    public void Menu()
    {
        _menu();
    }
    public void Game()
    {
        _game();
    }
    public void Credits()
    {
        _credits();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
