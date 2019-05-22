using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
	public string chinese;
	public string english;
	public string greek;
	public string russian;
	public string ukrainian;

	public string Get(SystemLanguage language)
	{
		switch (language)
		{
			case SystemLanguage.Chinese: return chinese;
			case SystemLanguage.English: return english;
			case SystemLanguage.Greek: return greek;
			case SystemLanguage.Russian: return russian;
			case SystemLanguage.Ukrainian: return ukrainian;

			default:
			{
				Debug.LogError("Localization for language " + language.ToString() + " is not supported");
				return string.Empty;
			}
		}
	}
}
