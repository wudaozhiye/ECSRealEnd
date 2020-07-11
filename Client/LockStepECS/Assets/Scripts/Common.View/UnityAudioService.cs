using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.Game
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public class UnityAudioService : UnityBaseService, IAudioService, IService
	{
		// Token: 0x0600007B RID: 123 RVA: 0x0000517E File Offset: 0x0000337E
		public override void Backup(int tick)
		{
			this._curFramePlayeredCount.Clear();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005190 File Offset: 0x00003390
		public override void DoStart()
		{
			this._source = base.gameObject.GetComponent<AudioSource>();
			bool flag = this._source == null;
			if (flag)
			{
				this._source = base.gameObject.AddComponent<AudioSource>();
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000051D4 File Offset: 0x000033D4
		public void PlayClip(ushort id)
		{
			bool flag = id == 0;
			if (!flag)
			{
				AudioClip clip;
				bool flag2 = UnityAudioService._id2Clip.TryGetValue(id, out clip);
				if (flag2)
				{
					this.PlayClip(clip);
				}
				else
				{
					TableData<Table_Assets>.Load();
					string assetPath = this._resService.GetAssetPath(id);
					AudioClip audioClip = Resources.Load<AudioClip>(assetPath);
					UnityAudioService._id2Clip.Add(id, audioClip);
					this.PlayClip(audioClip);
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005240 File Offset: 0x00003440
		public void PlayClip(AudioClip clip)
		{
			bool flag = clip != null;
			if (flag)
			{
				bool isPursueFrame = this._globalStateService.IsPursueFrame;
				if (!isPursueFrame)
				{
					bool flag2 = this._globalStateService.IsVideoMode && !this._globalStateService.IsRunVideo;
					if (!flag2)
					{
						int num;
						bool flag3 = this._curFramePlayeredCount.TryGetValue(clip, out num);
						if (flag3)
						{
							this._curFramePlayeredCount[clip] = num + 1;
						}
						else
						{
							this._curFramePlayeredCount.Add(clip, 1);
						}
						bool flag4 = this._curFramePlayeredCount[clip] >= 2;
						if (!flag4)
						{
							this._source.PlayOneShot(clip);
						}
					}
				}
			}
		}

		// Token: 0x04000092 RID: 146
		private AudioSource _source;

		// Token: 0x04000093 RID: 147
		private Dictionary<AudioClip, int> _curFramePlayeredCount = new Dictionary<AudioClip, int>();

		// Token: 0x04000094 RID: 148
		private static Dictionary<ushort, AudioClip> _id2Clip = new Dictionary<ushort, AudioClip>();
	}
}
