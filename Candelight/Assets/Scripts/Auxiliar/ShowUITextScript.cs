using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace Auxiliar
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class ShowUITextScript : MonoBehaviour
	{
		TextMeshProUGUI _textComponent;
		string _originalText;
		bool _showingText = false;
		[SerializeField] float _speed;

		private void Awake()
		{
			_textComponent = GetComponent<TextMeshProUGUI>();
		}

		private void Start()
		{
			_speed /= 10f;
			_textComponent.text = "";
		}

		public void SetSpeed(float newSlowness)
		{
			_speed = newSlowness;
		}

		public void ShowText(string s)
		{
			_textComponent.text = "";
			StartCoroutine(RevealText(s));
		}

		IEnumerator RevealText(string s)
		{
			_originalText = s;
			_showingText = true;

			yield return new WaitForSeconds(0.5f);

			var numCharsRevealed = 0;
			while (numCharsRevealed < _originalText.Length)
			{
				while (_originalText[numCharsRevealed] == ' ')
					++numCharsRevealed;

				++numCharsRevealed;
				_textComponent.text = _originalText.Substring(0, numCharsRevealed);

				yield return new WaitForSeconds(0.025f / _speed);

				switch (_originalText[numCharsRevealed - 1])
				{
					case ',':
						yield return new WaitForSeconds(0.08f / _speed);
						break;
					case '.':
						yield return new WaitForSeconds(0.2f / _speed);
						break;
					case '?':
						if (_originalText.Length > numCharsRevealed)
						{
							if (_originalText[numCharsRevealed] != '!') yield return new WaitForSeconds(0.2f / _speed);
						}
						break;
					case '!':
						yield return new WaitForSeconds(0.2f / _speed);
						break;
					default:
						break;
				}
			}
			_showingText = false;
			yield return null;
		}

		public bool IsShowingText()
		{
			return _showingText;
		}

		public void Interrupt()
		{
			StopAllCoroutines();
			_textComponent.text = _originalText;
			_showingText = false;
		}
	}
}