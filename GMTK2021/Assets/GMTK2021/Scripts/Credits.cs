using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text;
using TMPro;

public class Credits : MonoBehaviour
{

    [System.Serializable]
    public class CreditFileObject
    {

        public List<string> Music;
        public List<string> Programming;
        public List<string> Art2D;
        public List<string> SpecialThanks;


        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"\n{nameof(Music)}:\n");
            foreach (var item in Music)
            {
                result.Append(" - " + item + "\n");
            }
            result.Append($"\n2D Art:\n");
            foreach (var item in Art2D)
            {
                result.Append(" - " + item + "\n");
            }
            result.Append($"\n{nameof(Programming)}:\n");
            foreach (var item in Programming)
            {
                result.Append(" - " + item + "\n");
            }
            result.Append($"\nSpecial Thanks :\n");
            foreach (var item in SpecialThanks)
            {
                result.Append(" - " + item + "\n");
            }
            return result.ToString();
        }
    }

    public TMP_Text TextData { get; set; }
    public TextAsset CreditFile;

    // Start is called before the first frame update
    void Start()
    {
        TextData = GetComponent<TMP_Text>();
        if (!TextData)
            return;

        if (!CreditFile)
            return;

       var creditData = JsonUtility.FromJson<CreditFileObject>(CreditFile.text);

        TextData.text = creditData.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
