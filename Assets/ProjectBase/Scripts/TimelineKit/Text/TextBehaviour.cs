using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace ProjectBase.Timeline
{
	[Serializable]
	public class TextBehaviour : PlayableBehaviour
	{
		[HideInInspector] public bool IsUsed;

		[TextArea] public string newContent;
	}
}

