using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechController : MonoBehaviour
{
    private EventController _eventController;
    private KeywordRecognizer _keywordRecognizer;
    private readonly Dictionary<string, System.Action> _keywords = new Dictionary<string, System.Action>();

    private void Awake()
    {
        _eventController = GetComponent<EventController>();
    }

    private void Start()
    {
        _keywords.Add("Reset", () => _eventController.NotifyKeywordRecognized("Reset"));
        _keywords.Add("Rotate", () => _eventController.NotifyKeywordRecognized("Rotate"));

        _keywordRecognizer = new KeywordRecognizer(_keywords.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (_keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

}
