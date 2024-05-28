using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ProjectBase.Timeline
{
	[Serializable]
	public class TextClip : PlayableAsset, ITimelineClipAsset
	{
		public TextBehaviour template = new TextBehaviour();
		public ClipCaps clipCaps => ClipCaps.None;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<TextBehaviour>.Create(graph, template);
			return playable;
		}
	}
}

